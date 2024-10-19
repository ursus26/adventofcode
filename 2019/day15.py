#!/usr/bin/env python

import sys
import numpy as np
from collections import defaultdict


class IntcodeComputer:
    def __init__(self, intcode):
        self.intcode = intcode
        self.ip = 0
        self.stdin_idx = 0
        self.stdin = []
        self.stdout = []
        self.relative_base = 0
        self.memory_size = len(intcode)

    def extend_memory(self, n):
        extra_memory = [0 for _ in range(n)]
        self.intcode.extend(extra_memory)
        self.memory_size += n

    def parse_instr(self, instr):
        digits = [int(x) for x in str(instr)[:-2]]
        opcode = int(str(instr)[-2:])
        length = len(digits)

        if length == 0:
            return (opcode, 0, 0, 0)
        if length == 1:
            return (opcode, digits[0], 0, 0)
        if length == 2:
            return (opcode, digits[1], digits[0], 0)
        if length == 3:
            return (opcode, digits[2], digits[1], digits[0])

        print("PANIC!")
        sys.exit(1)

    def get_parameter(self, ip, mode):
        arg_ptr = 0

        if mode == 0:
            arg_ptr = self.intcode[ip]
        elif mode == 1:
            arg_ptr = ip
        elif mode == 2:
            arg_ptr = self.relative_base + self.intcode[ip]
        else:
            print("get_parameter::PANIC!")
            sys.exit(1)

        if arg_ptr >= self.memory_size:
            self.extend_memory(arg_ptr - self.memory_size + 1)

        return self.intcode[arg_ptr]

    def write_memory(self, ptr, value, mode):
        mem_ptr = 0
        if mode == 0:
            mem_ptr = ptr
        elif mode == 2:
            mem_ptr = self.relative_base + ptr
        else:
            print("write_memory::PANIC! Invalid mode")
            sys.exit(1)

        if mem_ptr >= self.memory_size:
            self.extend_memory(mem_ptr - self.memory_size + 1)

        self.intcode[mem_ptr] = value

    def run_intcode(self, stdin, halt_on_stdout=False):
        arg1 = 0
        arg2 = 0
        dst = 0
        arg1_mode = 0
        arg2_mode = 0
        arg3_mode = 0
        opcode = 0
        self.stdin.extend(stdin)
        self.stdout = []

        while True:
            opcode, arg1_mode, arg2_mode, arg3_mode = self.parse_instr(self.intcode[self.ip])
            # print("Opcode: {}, mode: {} {} {}".format(opcode, arg1_mode, arg2_mode, arg3_mode))

            if opcode == 1:
                arg1 = self.get_parameter(self.ip + 1, arg1_mode)
                arg2 = self.get_parameter(self.ip + 2, arg2_mode)
                dst = self.intcode[self.ip + 3]
                self.write_memory(dst, arg1 + arg2, arg3_mode)
                self.ip += 4
            elif opcode == 2:
                arg1 = self.get_parameter(self.ip + 1, arg1_mode)
                arg2 = self.get_parameter(self.ip + 2, arg2_mode)
                dst = self.intcode[self.ip + 3]
                self.write_memory(dst, arg1 * arg2, arg3_mode)
                self.ip += 4
            elif opcode == 3:
                if self.stdin_idx < len(self.stdin):
                    arg1 = self.stdin[self.stdin_idx]
                    self.stdin_idx += 1
                else:
                    # arg1 = input("Intcode requires input: ")
                    return (self.intcode[0], self.stdout, 1) # Wait till we have enough input.
                dst = self.intcode[self.ip + 1]
                self.write_memory(dst, int(arg1), arg1_mode)
                self.ip += 2
            elif opcode == 4:
                arg1 = self.get_parameter(self.ip + 1, arg1_mode)
                self.stdout.append(arg1)
                if halt_on_stdout:
                    self.ip += 2
                    return (self.intcode[0], self.stdout, 2)
                self.ip += 2
            elif opcode == 5:
                arg1 = self.get_parameter(self.ip + 1, arg1_mode)
                arg2 = self.get_parameter(self.ip + 2, arg2_mode)
                if arg1 != 0:
                    self.ip = arg2
                else:
                    self.ip += 3
            elif opcode == 6:
                arg1 = self.get_parameter(self.ip + 1, arg1_mode)
                arg2 = self.get_parameter(self.ip + 2, arg2_mode)
                if arg1 == 0:
                    self.ip = arg2
                else:
                    self.ip += 3
            elif opcode == 7:
                arg1 = self.get_parameter(self.ip + 1, arg1_mode)
                arg2 = self.get_parameter(self.ip + 2, arg2_mode)
                dst = self.intcode[self.ip + 3]
                if arg1 < arg2:
                    self.write_memory(dst, 1, arg3_mode)
                else:
                    self.write_memory(dst, 0, arg3_mode)
                self.ip += 4
            elif opcode == 8:
                arg1 = self.get_parameter(self.ip + 1, arg1_mode)
                arg2 = self.get_parameter(self.ip + 2, arg2_mode)
                dst = self.intcode[self.ip + 3]

                if arg1 == arg2:
                    self.write_memory(dst, 1, arg3_mode)
                else:
                    self.write_memory(dst, 0, arg3_mode)
                self.ip += 4
            elif opcode == 9: # relative base
                arg1 = self.get_parameter(self.ip + 1, arg1_mode)
                self.relative_base += arg1
                self.ip += 2
            elif opcode == 99:
                break
            else:
                print("INVALID OPCODE ENCOUNTERED, opcode: " + str(self.intcode[ip]) + " on ip: " + str(self.ip) + "\nABORTING PROGRAM")
                sys.exit(1)
                break

        return (self.intcode[0], self.stdout, 0)


def read_input():
    with open("./input/day15.txt", "r") as f:
        data = f.read()

    data = data.strip().split(',')
    out = []
    for elem in data:
        try:
            val = int(elem)
            out.append(val)
        except:
            pass

    return out


def draw_map(location_map, x_droid, y_droid):
    y_min = 0
    y_max = 0
    x_min = 0
    x_max = 0
    for x, y in location_map:
        x_min = min(x_min, x)
        x_max = max(x_max, x)

        y_min = min(y_min, y)
        y_max = max(y_max, y)

    width = x_max - x_min + 1
    height = y_max - y_min + 1
    y_shift = 0 - y_min
    x_shift = 0 - x_min

    image = [[' ' for _ in range(width)] for _ in range(height)]
    for x, y in location_map:
        character = ' '
        if x == x_droid and y == y_droid:
            character = 'D'
        elif location_map[(x, y)] == 0:
            character = '#'
        elif location_map[(x, y)] == 1:
            character = '.'
        elif location_map[(x, y)] == 1:
            character = '.'
        elif location_map[(x, y)] == 2:
            character = 'O'

        image[y + y_shift][x + x_shift] = character

    for row in image:
        row_str = ""
        for character in row:
            row_str += character
        print(row_str)


# def shortest_path(location_map, x_start, y_start, x_end, y_end):
#     visited_nodes = defaultdict(lambda:10000000)
#     queue = [(x_start, y_start, 0)]
#
#     for x, y in location_map:
#         if x == x_start and y == y_start:
#             continue
#         queue.append((x, y, 100000000))
#
#
#     while True:
#         queue = sorted(queue, key=lambda tup: tup[2], reverse=True)
#         x, y, w = queue.pop()
#
#         if x == x_end and y == y_end:
#             return w
#
#         if w < visited_nodes[(x, y)]:
#             visited_nodes[(x, y)] = w
#
#             if visited_nodes[(x, y - 1)] != 10000000:
#



def day15a():
    code = read_input()
    computer = IntcodeComputer(code)
    location_map = defaultdict(lambda:-1)

    trace = []

    x = 0
    y = 0
    dx = 1
    dy = 0
    x_oxygen = 0
    y_oxygen = 0
    stdin = []
    backtraced = False
    route_length = 0

    while True:
        # Choose direction.
        if location_map[(x, y - 1)] == -1:  # Check north
            dx = 0
            dy = -1
            stdin = [1]
        elif location_map[(x, y + 1)] == -1:    # South
            dx = 0
            dy = 1
            stdin = [2]
        elif location_map[(x - 1, y)] == -1:    # West
            dx = -1
            dy = 0
            stdin = [3]
        elif location_map[(x + 1, y)] == -1:    # East
            dx = 1
            dy = 0
            stdin = [4]
        else:   # Backtrace
            if len(trace) == 0:
                break
            x_old, y_old = trace.pop()
            dx = x_old - x
            dy = y_old - y
            backtraced = True
            if dx != 0:
                if dx == 1:
                    stdin = [4]
                else:
                    stdin = [3]
            else:
                if dy == 1:
                    stdin = [2]
                else:
                    stdin = [1]

        # Signal droid.
        _, stdout, interrupt_code = computer.run_intcode(stdin)

        # Process response.
        droid_response = stdout[-1]
        if droid_response == 0: # Droid hit a wall.
            location_map[(x + dx, y + dy)] = droid_response
        elif droid_response == 1:   # Droid moved successfully.
            location_map[(x + dx, y + dy)] = droid_response
            if not backtraced:
                trace.append((x, y))
            else:
                backtraced = False
            x += dx
            y += dy
        elif droid_response == 2:
            location_map[(x + dx, y + dy)] = droid_response
            if not backtraced:
                trace.append((x, y))
            else:
                backtraced = False
            x += dx
            y += dy

            x_oxygen = x
            y_oxygen = y
            route_length = len(trace)
        else:
            assert False


    draw_map(location_map, x, y)
    print("[DAY15 CHALLENGE A] Arrived at the oxygen tank after: {} steps".format(route_length))
    return location_map


def day15b(location_map):
    queue = []
    adjacent_loc = {}
    for loc in location_map:
        if location_map[loc] == 2:
            queue = [loc]
            break

    time = -1
    while queue:
        for x, y in queue:
            location_map[(x, y)] = 2

            if location_map[(x, y + 1)] == 1:
                adjacent_loc[(x, y + 1)] = 1
            if location_map[(x, y - 1)] == 1:
                adjacent_loc[(x, y - 1)] = 1
            if location_map[(x - 1, y)] == 1:
                adjacent_loc[(x - 1, y)] = 1
            if location_map[(x + 1, y)] == 1:
                adjacent_loc[(x + 1, y)] = 1

        time += 1

        queue = []
        for loc in adjacent_loc:
            queue.append(loc)

        adjacent_loc = {}


    print("[DAY15 CHALLENGE B] Time passed: {}".format(time))


    pass


def main():
    print("Hello Day 15!")
    location_map = day15a()
    day15b(location_map)


if __name__ == "__main__":
    main()
