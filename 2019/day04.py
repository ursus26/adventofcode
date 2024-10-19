#!/usr/bin/env python3

import sys
import re
import numpy as np


def check_increase_digit_order(number):
    for i in np.arange(len(number) - 1):
        if number[i] > number[i + 1]:
            return False

    return True


def check_2adjacent_digit(number):
    for i in np.arange(len(number) - 1):
        if number[i] == number[i + 1]:
            return True

    return False


def day4a():
    limit_low = 231832
    limit_high = 767346
    count = 0
    i = limit_low

    while i < limit_high:
        digits = [int(x) for x in str(i)]
        if check_increase_digit_order(digits) and check_2adjacent_digit(digits):
            count += 1

        i += 1

    print("Day 04 part 1, result: {}".format(count))


def check_2adjacent_digit_b(number):
    # digits = [int(x) for x in str(number)]

    for i in np.arange(len(number) - 3):
        w = number[i]
        x = number[i + 1]
        y = number[i + 2]
        z = number[i + 3]
        if x == y and y != z and w != x:
            return True

    if number[4] == number[5] and number[3] != number[4] or number[0] == number[1] and number[1] != number[2]:
        return True

    return False


def day4b():
    limit_low = 231832
    limit_high = 767346
    count = 0
    i = limit_low

    while i < limit_high:
        digits = [int(x) for x in str(i)]
        if check_increase_digit_order(digits) == True and check_2adjacent_digit_b(digits) == True:
            count += 1

        i += 1

    print("Day 04 part 1, result: {}".format(count))


def main():
    day4a()
    day4b()


if __name__ == "__main__":
    main()
