#!/usr/bin/env python3

import copy
import queue
from intcode import Intcode


with open("./input/day5.txt", "r") as f:
    data = [int(x) for x in f.read().strip().split(',')]

# Part 1
stdin = queue.Queue()
stdin.put(1)
intcode_part1 = Intcode(copy.deepcopy(data), stdin=stdin)
intcode_part1.run()
while not intcode_part1.stdout.empty():
    solution_part1 = intcode_part1.stdout.get()
print("Day 05 part 1, result: {}".format(solution_part1))

# Part 2
stdin = queue.Queue()
stdin.put(5)
intcode_part2 = Intcode(copy.deepcopy(data), stdin=stdin)
intcode_part2.run()
solution_part2 = 0
while not intcode_part2.stdout.empty():
    solution_part2 = intcode_part2.stdout.get()
print("Day 05 part 2, result: {}".format(solution_part2))
