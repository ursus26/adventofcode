#!/usr/bin/env python3

import copy
from intcode import Intcode


with open("./input/day09.txt", "r") as f:
    data = [int(x) for x in f.read().strip().split(',')]

# Part 1
intcode_part1 = Intcode(copy.deepcopy(data))
intcode_part1.stdin.put(1)
intcode_part1.run()
solution_part1 = intcode_part1.stdout.get()
print("Day 09 part 1, result: {}".format(solution_part1))

# Part 2
intcode_part2 = Intcode(copy.deepcopy(data))
intcode_part2.stdin.put(2)
intcode_part2.run()
solution_part2 = intcode_part1.stdout.get()
print("Day 09 part 2, result: {}".format(solution_part2))
