#!/usr/bin/env python

import sys
import numpy as np
from collections import defaultdict

EMPTY_ID = 0
WALL_ID = 1
BLOCK_ID = 2
HORIZONTAL_PADDLE_ID = 3
BALL_ID = 4

JOYSTICK_LEFT = -1
JOYSTICK_NEUTRAL = 0
JOYSTICK_RIGHT = 1


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
    with open("./input/day13.txt", "r") as f:
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


def day13a():
    code = read_input()
    computer = IntcodeComputer(code)
    _, stdout, _ = computer.run_intcode([])
    tile_map = defaultdict(int)

    for i in np.arange(0, len(stdout), 3):
        x = stdout[i]
        y = stdout[i + 1]
        tile_id = stdout[i + 2]
        tile_map[(x, y)] = tile_id


    block_count = 0
    for pos in tile_map:
        if tile_map[pos] == BLOCK_ID:
            block_count += 1

    print("[DAY13 CHALLENGE A] Block count: {}".format(block_count))


def draw_map(tile_map):

    screen = [[0 for _ in range(40)] for _ in range(20)]
    # print(screen)
    for pos in tile_map:
        x, y = pos
        if x == -1 and y == 0:
            continue

        sprite = ' '
        tile = tile_map[pos]
        if tile == WALL_ID:
            sprite = 'W'
        elif tile == BLOCK_ID:
            sprite = 'B'
        elif tile == HORIZONTAL_PADDLE_ID:
            sprite = '_'
        elif tile == BALL_ID:
            sprite = '+'


        screen[y][x] = sprite


    for row in screen:
        # print(row)
        row_str = ""
        for sprite in row:
            row_str += sprite

        print(row_str)


def day13b():
    code = read_input()
    code[0] = 2
    computer = IntcodeComputer(code)
    tile_map = defaultdict(int)
    stdin = []
    score = 0

    # for i in range(10):
    while True:
        _, stdout, interrupt = computer.run_intcode(stdin)
        for i in np.arange(0, len(stdout), 3):
            x = stdout[i]
            y = stdout[i + 1]
            tile_id = stdout[i + 2]
            tile_map[(x, y)] = tile_id

            if x == -1 and y == 0:
                score = tile_id

        # draw_map(tile_map)

        block_count = 0
        x_ball = 0
        x_paddle = 0
        for pos in tile_map:
            if tile_map[pos] == BLOCK_ID:
                block_count += 1
            elif tile_map[pos] == BALL_ID:
                x_ball = pos[0]
            elif tile_map[pos] == HORIZONTAL_PADDLE_ID:
                x_paddle = pos[0]

        # Keep the paddle on the same x-axis as the ball.
        if x_ball == x_paddle:
            stdin = [JOYSTICK_NEUTRAL]
        elif x_ball < x_paddle:
            stdin = [JOYSTICK_LEFT]
        else:
            stdin = [JOYSTICK_RIGHT]


        if block_count == 0:
            break


    print("[DAY13 CHALLENGE B] Score: {}".format(score))


def main():
    print("Hello Day 13!")
    day13a()
    day13b()


if __name__ == "__main__":
    main()
