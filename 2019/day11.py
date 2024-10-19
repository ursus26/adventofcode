#!/usr/bin/env python

import sys
import numpy as np
from collections import defaultdict
import matplotlib.pyplot as plt


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
                # if halt_on_stdout:
                #     self.ip += 2
                #     return (self.intcode[0], self.stdout, 0)
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
    with open("./input/day11.txt", "r") as f:
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


def day11a():
    intcode = read_input()
    computer = IntcodeComputer(intcode)
    stdin = [0]
    stdin_expected = 3
    ret_code = 0
    stdout = None
    x = 0
    y = 0
    dx = 0
    dy = 1
    colour_map = defaultdict(int)
    visited_panels = {}

    while stdin_expected:
        stdin = [colour_map[(x, y)]]
        ret_code, stdout, stdin_expected = computer.run_intcode(stdin)
        colour = stdout[-2]
        turn_direction = stdout[-1]

        # Update the colour map and the visited locations dictionary.
        colour_map[(x, y)] = colour
        visited_panels[(x, y)] = 1

        # Calculate direction vector based on the turning direction. 0 = left, 1 = right
        if turn_direction == 0:
            tmp = dx
            dx = -dy
            dy = tmp
        else:
            tmp = dx
            dx = dy
            dy = -tmp

        # Change the position of the robot.
        x += dx
        y += dy


    total_visited_panels = 0
    for k in visited_panels:
        total_visited_panels += 1


    print("[DAY11 CHALLENGE A] total painted panels: {}".format(total_visited_panels))

def day11b():
    intcode = read_input()
    computer = IntcodeComputer(intcode)
    stdin = [0]
    stdin_expected = 3
    ret_code = 0
    stdout = None
    x = 0
    y = 0
    dx = 0
    dy = 1
    colour_map = defaultdict(int)
    colour_map[(x, y)] = 1 # Starting panel.
    visited_panels = {}

    while stdin_expected:
        stdin = [colour_map[(x, y)]]
        ret_code, stdout, stdin_expected = computer.run_intcode(stdin)
        colour = stdout[-2]
        turn_direction = stdout[-1]

        # Update the colour map and the visited locations dictionary.
        colour_map[(x, y)] = colour
        visited_panels[(x, y)] = 1

        # Calculate direction vector based on the turning direction. 0 = left, 1 = right
        if turn_direction == 0:
            tmp = dx
            dx = -dy
            dy = tmp
        else:
            tmp = dx
            dx = dy
            dy = -tmp

        # Change the position of the robot.
        x += dx
        y += dy


    total_visited_panels = 0
    min_x = 0
    max_x = 0
    min_y = 0
    max_y = 0
    for k in visited_panels:
        total_visited_panels += 1
        min_x = min(min_x, k[0])
        min_y = min(min_y, k[1])
        max_x = max(max_x, k[0])
        max_y = max(max_y, k[1])


    width = max_x - min_x + 1
    height = max_y - min_y + 1
    panel_image = np.zeros((height, width), dtype=int)
    for x, y in colour_map:
        panel_image[-1 * y][x + abs(min_x) - 1] = colour_map[(x, y)]


    print("[DAY11 CHALLENGE B] registration identifier:")
    for row in panel_image:
        row_str = ""
        for panel_colour in row:
            if panel_colour == 1:
                row_str += "##"
            else:
                row_str += "  "

        print(row_str)



def main():
    print("Hello Day 11!")
    day11a()
    day11b()


if __name__ == "__main__":
    main()
