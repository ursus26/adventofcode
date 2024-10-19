#!/usr/bin/env python3

import copy
from intcode import Intcode


with open("./input/day2.txt", "r") as f:
    data = [int(x) for x in f.read().strip().split(',')]

data[1] = 12
data[2] = 2
intcode_part1 = Intcode(copy.deepcopy(data))
solution_part1 = intcode_part1.run()
print("Day 02 part 1, result: {}".format(solution_part1))

solution_part2 = None
for i in range(100):
    data[1] = i
    for j in range(100):
        data[2] = j
        intcode = Intcode(copy.deepcopy(data))
        return_code = intcode.run()

        if return_code == 19690720:
            solution_part2 = 100 * i + j
            break

    if solution_part2:
        break

print("Day 02 part 2, result: {}".format(solution_part2))
