#!/usr/bin/env python

import sys
import numpy as np
from collections import defaultdict


def read_input():
    with open("./input/day16.txt", "r") as f:
        data = f.read()

    data = data.strip().split('\n')
    out = [int(x) for x in data[0]]

    return np.array(out)


def day16a():
    signal = read_input()
    signal_length = len(signal)
    result = np.zeros(signal_length, dtype=int)
    kernel_base = [0, 1, 0, -1]
    kernel_length = len(kernel_base)
    max_phases = 100

    for phase in range(max_phases):
        result = np.zeros(signal_length, dtype=int)
        for i in range(signal_length):
            if i > signal_length / 2:
                result[i] = np.sum(signal[i:])
            else:
                kernel_idx = 1
                for j in range(i, signal_length, i+1):
                    kernel_value = kernel_base[kernel_idx]
                    kernel_idx = (kernel_idx + 1) % kernel_length
                    if kernel_value == 1:
                        result[i] += np.sum(signal[j:j+i+1])
                    elif kernel_value == -1:
                        result[i] -= np.sum(signal[j:j+i+1])

            result[i] = abs(result[i]) % 10

        signal = result

    result_number = 0
    for i, digit in enumerate(reversed(signal[:8])):
        result_number += 10**i * digit

    print("[DAY16 CHALLENGE A] Phase: {}, Result: {}".format(max_phases, result_number))


# THIS WORKS ONLY IF THE DESTINATION DIGITS IS IN THE SECOND HALF OF THE SIGNAL!!!.
def day16b():
    signal_original = read_input()

    signal = np.array([], dtype=int)
    for _ in range(10000):
        signal = np.append(signal, signal_original)

    result_idx = 0
    for i, digit in enumerate(reversed(signal_original[:7])):
        result_idx += 10**i * digit

    print(result_idx)

    signal_length = len(signal)
    optimized_offset = (signal_length + 1) // 2
    result = np.zeros(signal_length, dtype=int)
    kernel_base = [0, 1, 0, -1]
    kernel_length = len(kernel_base)
    max_phases = 100

    for phase in range(max_phases):
        # print("PHASE: {}".format(phase))

        # Initial setup.
        result[-1] = signal[-1]

        # Start running on where the output value is going to be.
        for i in range(signal_length - 2, result_idx - 1, -1):
            result[i] = result[i + 1] + signal[i]

        # Get the last digit of every element.
        result = np.absolute(result) % 10

        # Swap buffers.
        tmp = signal
        signal = result
        result = tmp

    result_number = 0
    for i, digit in enumerate(reversed(signal[result_idx:result_idx+8])):
        result_number += 10**i * digit

    print("[DAY16 CHALLENGE B] Phase: {}, Result: {}".format(max_phases, result_number))


def main():
    print("Hello Day 16!")
    day16a()

    # THIS WORKS ONLY IF THE DESTINATION DIGITS IS IN THE SECOND HALF OF THE SIGNAL!!!.
    day16b()


if __name__ == "__main__":
    main()
