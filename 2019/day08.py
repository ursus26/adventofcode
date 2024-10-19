#!/usr/bin/env python3

import sys
import numpy as np
from itertools import permutations
import matplotlib.pyplot as plt


def read_input():
    with open("./input/day8.txt", "r") as f:
        data = f.read()

    data = data.split('\n')
    out = [x for x in data if x != ""]
    return out


def day8a():
    data = read_input()
    IMG_WIDTH = 25
    IMG_HEIGHT = 6
    LAYER_SIZE = IMG_WIDTH * IMG_HEIGHT

    data = [int(x) for x in data[0]]
    data = np.array(data).reshape((int(len(data) / LAYER_SIZE), LAYER_SIZE))

    least_zero_layer = 0
    min_zero = LAYER_SIZE

    for i, layer in enumerate(data):
        zero_counter = np.sum(layer == 0)
        if zero_counter < min_zero:
            min_zero = zero_counter
            least_zero_layer = i

    one_count = np.sum(data[least_zero_layer] == 1)
    two_count = np.sum(data[least_zero_layer] == 2)

    solution_part1 = one_count * two_count
    print("Day 08 part 1, result: {}".format(solution_part1))


def day8b():
    data = read_input()
    IMG_WIDTH = 25
    IMG_HEIGHT = 6
    LAYER_SIZE = IMG_WIDTH * IMG_HEIGHT

    data = [int(x) for x in data[0]]
    data = np.array(data).reshape((int(len(data) / LAYER_SIZE), LAYER_SIZE))

    image = np.copy(data[0])
    for layer in data[1:]:
        for i, pixel in enumerate(layer):
            if image[i] == 2:
                image[i] = pixel

    image = image.reshape((IMG_HEIGHT, IMG_WIDTH))
    print("Day 08 part 2, image:")
    print(image)
    plt.imshow(image, cmap='gray')
    plt.show()


def main():
    day8a()
    day8b()


if __name__ == "__main__":
    main()
