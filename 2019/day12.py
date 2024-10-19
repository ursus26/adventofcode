#!/usr/bin/env python

import sys
import numpy as np
from collections import defaultdict


def read_input():
    with open("./input/day12.txt", "r") as f:
        data = f.read()

    data = data.strip().split('\n')
    out = []
    for body in data:
        tmp = body.split(',')
        x = int(tmp[0].split("=")[1])
        y = int(tmp[1].split("=")[1])
        z = int(tmp[2].split("=")[1][:-1])
        out.append([x, y, z])

    return out


def day12a():
    bodies = np.array(read_input())
    velocity = np.zeros(bodies.shape, dtype=int)
    simulation_steps = 1000

    for step in range(1, simulation_steps + 1):
        for i, body in enumerate(bodies):
            x_body = body[0]
            y_body = body[1]
            z_body = body[2]

            for j, other in enumerate(bodies):
                if i == j:
                    continue

                x_other = other[0]
                y_other = other[1]
                z_other = other[2]

                if x_body < x_other:
                    velocity[i][0] += 1
                elif x_body > x_other:
                    velocity[i][0] -= 1

                if y_body < y_other:
                    velocity[i][1] += 1
                elif y_body > y_other:
                    velocity[i][1] -= 1

                if z_body < z_other:
                    velocity[i][2] += 1
                elif z_body > z_other:
                    velocity[i][2] -= 1

        # Update the bodies.
        bodies += velocity

        # print("Step: {}".format(step))
        # for i, body in enumerate(bodies):
        #     print("pos=<x={}, y={}, z={}>, vel=<x={}, y={}, z={}>".format(body[0], body[1], body[2], velocity[i][0], velocity[i][1], velocity[i][2]))
        # print(" ")

    total_energy = 0
    for i, body in enumerate(bodies):
        pot = abs(body[0]) + abs(body[1]) + abs(body[2])
        kin = abs(velocity[i][0]) + abs(velocity[i][1]) + abs(velocity[i][2])
        total_energy += pot * kin

    print("[DAY12 CHALLENGE A] total energy: {}".format(total_energy))


def find_cycle(dimension):
    bodies = np.array(read_input())
    velocity = np.zeros(bodies.shape, dtype=int)
    step = 1
    progress_report_steps = 1
    progress_report_steps_needed = 1000000

    states = defaultdict(int)
    cycle = 0

    while True:
        for i, body in enumerate(bodies):
            positon_body = body[dimension]

            for j, other in enumerate(bodies):
                if i == j:
                    continue

                position_other = other[dimension]
                if positon_body < position_other:
                    velocity[i][dimension] += 1
                elif positon_body > position_other:
                    velocity[i][dimension] -= 1

        # Update the bodies.
        bodies += velocity

        key_value = ""
        for i, body in enumerate(bodies):
            key_value += str(body[dimension]) + "," + str(velocity[i][dimension]) + ","

        if states[key_value] == 0:
            states[key_value] = step
        else:
            print("[DAY12 CHALLENGE B] Dimension: {}, revisited a previous state after: {} steps".format(dimension, step -1))
            return step -1

        if progress_report_steps_needed == progress_report_steps:
            print("Steps: {}".format(step))
            progress_report_steps = 1
        else:
            progress_report_steps += 1

        step += 1


def day12b():
    x_cycle = find_cycle(0)
    y_cycle = find_cycle(1)
    z_cycle = find_cycle(2)

    # Cycke calculation.
    lcm = np.lcm(x_cycle, y_cycle)
    universe_cycle = np.lcm(lcm, z_cycle)
    print("[DAY12 CHALLENGE B] Universe cycle: {}".format(universe_cycle))


def main():
    print("Hello Day 12!")
    day12a()
    day12b()


if __name__ == "__main__":
    main()
