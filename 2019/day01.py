#!/usr/bin/env python3

import math

with open("./input/day1.txt", "r") as f:
    data = [int(x) for x in f.read().strip().split('\n')]

solution_part1 = 0
solution_part2 = 0
for moduleMass in data:
    fuelMass = math.floor(moduleMass / 3.0) - 2
    solution_part1 += fuelMass
    while fuelMass > 0:
        solution_part2 += fuelMass
        fuelMass = math.floor(fuelMass / 3.0) - 2

print("Day 01 part 1, result: {}".format(solution_part1))
print("Day 01 part 2, result: {}".format(solution_part2))
