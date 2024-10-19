#!/usr/bin/env python3

from threading import Thread
from intcode import Intcode
import numpy as np


def read_input():
    with open("./input/day17.txt", "r") as f:
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


def get_intersections(view):
    count = 0
    intersections = []
    for y in range(1, len(view) - 1):
        for x in range(1, len(view[0]) - 1):
            if view[y][x] == '#' and view[y-1][x] == '#' and view[y+1][x] == '#' and view[y][x-1] == '#' and view[y][x+1] == '#':
                count += 1
                intersections.append((x, y))

    return count, intersections


def encode_to_ascii(l):
    output = []
    for c in l:
        output.append(ord(c))
    output.append(ord('\n'))
    return output


def day17a():
    # Start intcode program.
    code = read_input()
    program = Intcode(code)
    t = Thread(target=program.run)
    t.start()

    # Monitor stdout for the camera view.
    camera_view = ""
    while t.is_alive():
        try:
            while True:
                value =  program.stdout.get_nowait()
                camera_view += chr(value)
        except:
            pass

    # print(camera_view)

    # Count the intersections.
    camera_view = camera_view.strip()
    view_lines = camera_view.splitlines()
    intersection_count, intersections = get_intersections(view_lines)
    solution_part1 = np.sum([x * y for x, y in intersections])

    print("Day 17 part 1, result: {}".format(solution_part1))


def day17b():
    """
    My scaffolding:

    R,6,L,10,R,8,R,8,R,12,L,8,L,8,R,6,L,10,R,8,R,8,R,12,L,8,L,8,
    L,10,R,6,R,6,L,8,R,6,L,10,R,8,R,8,R,12,L,8,L,8,L,10,R,6,R,6,
    L,8,R,6,L,10,R,8,L,10,R,6,R,6,L,8

    ->

    A,B,A,B,C,A,B,C,A,C

    A: R,6,L,10,R,8
    B: R,8,R,12,L,8,L,8
    C: L,10,R,6,R,6,L,8
    """

    # Start intcode program.
    code = read_input()
    code[0] = 2
    program = Intcode(code)
    t = Thread(target=program.run)
    t.start()

    # Craft input.
    main_routine = encode_to_ascii("A,B,A,B,C,A,B,C,A,C")
    A_routine = encode_to_ascii("R,6,L,10,R,8")
    B_routine = encode_to_ascii("R,8,R,12,L,8,L,8")
    C_routine = encode_to_ascii("L,10,R,6,R,6,L,8")
    video_feed = encode_to_ascii("n")
    program_input = main_routine + A_routine + B_routine + C_routine + video_feed

    # Write input to stdin.
    for elem in program_input:
        program.stdin.put(elem)

    # Monitor stdout for the camera view.
    camera_view = ""
    answer_part2 = 0
    while t.is_alive():
        try:
            while True:
                value =  program.stdout.get_nowait()

                if(value < 256):
                    camera_view += chr(value)
                else:
                    answer_part2 = value
        except:
            pass

    # print(camera_view)
    print("Day 17 part 2, result: {}".format(answer_part2))


def main():
    day17a()
    day17b()


if __name__ == "__main__":
    main()