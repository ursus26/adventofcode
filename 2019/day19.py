#!/usr/bin/env python

import copy
from intcode import Intcode


def read_input():
    with open("./input/day19.txt", "r") as f:
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


def run_tractor_beam(x, y):
    program = Intcode(copy.deepcopy(code))
    program.stdin.put(x)
    program.stdin.put(y)
    program.run()
    return program.stdout.get()


def day19a():
    x_start = 0
    counter = 0
    for y in range(50):
        prev_result = 0
        found_first_1 = False
        
        for x in range(x_start, 50):
            result = run_tractor_beam(x, y)

            # Test if we found the left edge of the tractor beam.
            if not found_first_1 and result == 1:
                x_start = x
                found_first_1 = True

            # Stop if we are past the right edge of the tractor beam.
            if result == 0 and prev_result == 1:
                break
            
            counter += result
            prev_result = result

    print("Day 19 part 1, result: {}".format(counter))


def day19b():
    box_size = 100
    x_start = 0
    y_bottom = box_size - 1
    while True:

        # Find left edge of tractor beam.
        x_left = x_start
        while True:
            result = run_tractor_beam(x_left, y_bottom)
            if result == 1:
                x_start = x_left
                break
            x_left += 1

        # Test if we can fit a box inside the tractor beam.
        x_right = x_left + box_size - 1
        y_top = y_bottom - box_size + 1
        if run_tractor_beam(x_right, y_top) == 1:
            answer_part2 = 10000 * x_left + y_top
            print("Day 19 part 2, result: {}".format(answer_part2))
            break

        y_bottom += 1


code = read_input()

def main():
    day19a()
    day19b()


if __name__ == "__main__":
    main()
