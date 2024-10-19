#!/usr/bin/env python

import sys
import numpy as np
from collections import defaultdict


def read_input():
    with open("./input/day14.txt", "r") as f:
        data = f.read()

    data = data.strip().replace(',', '').split('\n')
    out = {}

    for row in data:
        tokens = row.split(' ')
        required_chemicals = []
        for i in range(0, len(tokens) - 3, 2):
            required_chemicals.append((tokens[i + 1], int(tokens[i])))

        out[tokens[-1]] = (required_chemicals, int(tokens[-2]))

    return out


def day14a():
    reactions = read_input()
    reactions["ORE"] = ([], 1)
    chemicals_in_sys = {}
    for reaction in reactions:
        chemicals_in_sys[reaction] = (0, 0)

    queue = [("FUEL", 1)]

    while queue:
        product, product_needed = queue.pop()
        product_used, product_available = chemicals_in_sys[product]

        # Check if we had some left over chemicals.
        if product_available > 0:
            if product_needed <= product_available:
                chemicals_in_sys[product] = (product_used + product_needed, product_available - product_needed)
                product_needed = 0
                continue
            else:
                chemicals_in_sys[product] = (product_used + product_available, 0)
                product_needed -= product_available

        in_reaction, output = reactions[product]
        product_used, product_available = chemicals_in_sys[product]
        multiplier = int(np.ceil(product_needed / float(output)))
        left_over = multiplier * output - product_needed
        chemicals_in_sys[product] = (product_used + product_needed, product_available + left_over)

        for chem, amount in in_reaction:
            queue.append((chem, multiplier * amount))


    ore_required = chemicals_in_sys["ORE"][0] + chemicals_in_sys["ORE"][1]
    print("[DAY14 CHALLENGE A] ORE required: {}".format(ore_required))



def day14b():
    reactions = read_input()
    reactions["ORE"] = ([], 0)
    chemicals_in_sys = {}
    for reaction in reactions:
        chemicals_in_sys[reaction] = (0, 0)

    chemicals_in_sys['ORE'] = (0, 1000000000000)
    stop_loop = False
    while True:
        if chemicals_in_sys['ORE'][1] < 50000000:
            queue = [("FUEL", 1)]
            print(chemicals_in_sys['ORE'], 1)
        else:
            print(chemicals_in_sys['ORE'], 1000)
            queue = [("FUEL", 1000)]

        while queue:
            product, product_needed = queue.pop()
            product_used, product_available = chemicals_in_sys[product]

            # Check if we had some left over chemicals.
            if product_available > 0:
                if product_needed <= product_available:
                    chemicals_in_sys[product] = (product_used + product_needed, product_available - product_needed)
                    product_needed = 0
                    continue
                else:
                    chemicals_in_sys[product] = (product_used + product_available, 0)
                    product_needed -= product_available

            in_reaction, output = reactions[product]
            if output == 0:
                stop_loop = True
                print(product, chemicals_in_sys[product], chemicals_in_sys)
                break

            product_used, product_available = chemicals_in_sys[product]
            multiplier = int(np.ceil(product_needed / float(output)))
            left_over = multiplier * output - product_needed
            chemicals_in_sys[product] = (product_used + product_needed, product_available + left_over)

            for chem, amount in in_reaction:
                queue.append((chem, multiplier * amount))

        if stop_loop:
            break

    fuel_produced = chemicals_in_sys["FUEL"][0] + chemicals_in_sys["FUEL"][1] - 1
    print("[DAY14 CHALLENGE B] Fuel produced: {}".format(fuel_produced))


def main():
    print("Hello Day 14!")
    day14a()
    day14b()


if __name__ == "__main__":
    main()
