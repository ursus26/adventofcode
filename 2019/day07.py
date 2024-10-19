#!/usr/bin/env python3

import copy
import queue
from itertools import permutations
import threading
from intcode import Intcode


def part1(data):
    max_amplification = 0
    for permutation in permutations(range(5)):
        queues = [queue.Queue() for _ in range(6)]
        computers = [Intcode(copy.deepcopy(data), stdin=queues[i], stdout=queues[i+1]) for i in range(5)]

        for i in range(5):
            queues[i].put(permutation[i])

        queues[0].put(0)
        for i in range(5):
            computers[i].run()

        output = queues[-1].get()
        if output > max_amplification:
            max_amplification = output

    return max_amplification


def part2(data):
    max_amplification = 0
    for permutation in permutations(range(5, 10)):
        queues = [queue.Queue() for _ in range(5)]
        computers = [Intcode(copy.deepcopy(data), stdin=queues[i], stdout=queues[(i+1) % 5]) for i in range(5)]

        for i in range(5):
            queues[i].put(permutation[i])

        threads = []
        queues[0].put(0)
        for i in range(5):
            thread = threading.Thread(target=computers[i].run)
            thread.start()
            threads.append(thread)

        for i in range(5):
            threads[i].join()

        output = queues[0].get()
        if output > max_amplification:
            max_amplification = output

    return max_amplification


with open("./input/day7.txt", "r") as f:
    data = [int(x) for x in f.read().strip().split(',')]

solution_part1 = part1(data)
print("Day 07 part 1, result: {}".format(solution_part1))

solution_part2 = part2(data)
print("Day 07 part 2, result: {}".format(solution_part2))
