#!/usr/bin/env python3

import sys
import numpy as np


class SpaceObject:
    def __init__(self, id):
        self.id = id
        self.parent = None
        self.children = []

    def add_child(self, new_child):
        self.children.append(new_child)

    def __eq__(self, other):
        if self.id == other:
            return True
        else:
            return False


def read_input():
    with open("./input/day6.txt", "r") as f:
        data = f.read()

    data = data.split('\n')
    return [x for x in data if x != ""]

def day6a():
    data = read_input()
    orbital_data = []
    for elem in data:
        orbit = elem.split(")")
        orbital_data.append((orbit[0], orbit[1]))

    orbital_map = {}
    for A, B in orbital_data:
        orbital_map.setdefault(A, SpaceObject(A))
        orbital_map.setdefault(B, SpaceObject(B))

        orbital_map[A].add_child(B)
        orbital_map[B].parent = A


    orbital_count = 0
    work_queue = [("COM", 0)]

    while len(work_queue) > 0:
        body_id, depth = work_queue.pop()
        if body_id not in orbital_map:
            continue

        body = orbital_map[body_id]
        for new_body in body.children:
            work_queue.append((new_body, depth + 1))

        orbital_count += len(body.children) * (depth + 1)

    print("Day 06 part 1, result: {}".format(orbital_count))

    return orbital_map


def day6b(orbital_map):
    start = orbital_map["YOU"]
    destination = orbital_map["SAN"]
    visited_locations = {}

    work = [(start, 0)]
    path_length = 0
    while(len(work) > 0):
        obj, length = work.pop()
        visited_locations[obj.id] = 1

        if obj == destination:
            path_length = length
            break

        if obj.parent is not None and obj.parent not in visited_locations:
            work.insert(0, (orbital_map[obj.parent], length + 1))

        for child in obj.children:
            if child not in visited_locations:
                work.insert(0, (orbital_map[child], length + 1))

    orbital_transfers = path_length - 2
    print("Day 06 part 2, result: {}".format(orbital_transfers))


def main():
    orbital_map = day6a()
    day6b(orbital_map)


if __name__ == "__main__":
    main()
