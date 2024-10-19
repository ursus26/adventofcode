#!/usr/bin/env python3

import sys
import numpy as np

def read_input():
    with open("./input/day3.txt", "r") as f:
        data = f.read()

    data = data.split('\n')
    data.remove("")

    out = []
    for wire in data:
        out.append(wire.split(","))

    return out


def trace_paths(paths):
    route = {}
    intersections = []
    wire_id = -1

    for path in paths:
        x = 0
        y = 0
        wire_id += 1
        wire_length = 0

        for instr in path:
            dir = instr[0]
            steps = int(instr[1:])

            for i in range(1, steps + 1):
                wire_length += 1

                if dir == 'L':
                    x -= 1
                elif dir == 'R':
                    x +=1
                elif dir == 'U':
                    y += 1
                elif dir == 'D':
                    y -= 1
                else:
                    print("PANIC! Invalid direction: {}".format(dir))
                    sys.exit(1)

                if route.setdefault((x, y), None) is not None:
                    route[(x, y)].setdefault(wire_id, wire_length)

                    if len(route[(x, y)]) > 1:
                        intersections.append((x, y))
                else:
                    route[(x, y)] = {}
                    route[(x, y)][wire_id] = wire_length


    return route, intersections


def shortest_manhattan_distance(points):
    distances = [abs(x) + abs(y) for x, y in points]
    return np.amin(distances)


def shortest_path_length(route, intersections):
    min_dist = 1 << 31
    distance = 0

    for x, y in intersections:
        distance = route[(x, y)][0] + route[(x, y)][1]
        if distance < min_dist:
            min_dist = distance

    return min_dist


def main():
    paths = read_input()

    route, intersections = trace_paths(paths)
    min_manhattan_distance = shortest_manhattan_distance(intersections)
    print("Day 03 part 1, result: {}".format(min_manhattan_distance))

    min_path_distance = shortest_path_length(route, intersections)
    print("Day 03 part 2, result: {}".format(min_path_distance))

if __name__ == "__main__":
    main()
