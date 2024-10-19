#!/usr/bin/env python3

# import sys
import numpy as np


def read_input():
    with open("./input/day10.txt", "r") as f:
        data = f.read()

    data = data.strip().split('\n')
    out = []

    for row in data:
        out.append([0 if x == '.' else 1 for x in row])

    return out


def detect_astroids(astroid_map, x_astr, y_astr):
    width = len(astroid_map[0])
    height = len(astroid_map)

    for y, row in enumerate(astroid_map):
        for x, elem in enumerate(row):
            if (x == x_astr and y == y_astr) or elem == 0:
                continue

            dx = x - x_astr
            dy = y - y_astr
            gcd = np.gcd(dx, dy)
            dx = int(dx / gcd)
            dy = int(dy / gcd)

            cur_x = x + dx
            cur_y = y + dy
            # print("Detected new astroid: {}, {}, dx: {}, dy: {}, start_cur_x: {}, start_cur_y: {}".format(x, y, dx, dy, cur_x, cur_y))
            while cur_x >= 0 and cur_x < width and cur_y >= 0 and cur_y < height:
                # print("!!!Detected new astroid: {}, {}, dx: {}, dy: {}, start_cur_x: {}, start_cur_y: {}".format(x, y, dx, dy, cur_x, cur_y))
                astroid_map[cur_y][cur_x] = 0
                cur_x += dx
                cur_y += dy

            cur_x = x - dx
            cur_y = y - dy
            visible_x = x
            visible_y = y
            while cur_x != x_astr and cur_y != y_astr:
                if astroid_map[cur_y][cur_x] > 0:
                    # print("current pos - dx/dy: {}, {}".format(visible_x, visible_y))
                    astroid_map[visible_y][visible_x] = 0
                    visible_x = cur_x
                    visible_y = cur_y

                cur_x -= dx
                cur_y -= dy


    # astroid_map[y_astr][x_astr] = 6
    # print(astroid_map)
    astroid_map[y_astr][x_astr] = 0
    return np.sum(astroid_map), astroid_map

def day10a():
    data =  read_input()

    best_astroid = None
    max_astroids_detected = 0

    for y, row in enumerate(data):
        for x, elem in enumerate(row):
            if elem == 1:
                map_copy = np.array(data, copy=True)
                astroids_detected, _ = detect_astroids(map_copy, x, y)
                # print("x: {}, y: {}, visible astroids: {}".format(x, y, astroids_detected))
                if astroids_detected > max_astroids_detected:
                    max_astroids_detected = astroids_detected
                    best_astroid = (x, y)

    print("[DAY10 CHALLENGE A] best location: {}, visible astroids: {}".format(best_astroid, max_astroids_detected))


def day10b():
    data =  read_input()
    width = len(data[0])
    height = len(data)

    best_astroid = None
    max_astroids_detected = 0
    best_map = None

    for y, row in enumerate(data):
        for x, elem in enumerate(row):
            if elem == 1:
                map_copy = np.array(data, copy=True)
                astroids_detected, astroid_map = detect_astroids(map_copy, x, y)
                # print("x: {}, y: {}, visible astroids: {}".format(x, y, astroids_detected))
                if astroids_detected > max_astroids_detected:
                    max_astroids_detected = astroids_detected
                    best_astroid = (x, y)
                    best_map = astroid_map


    astroids_vaporized = 0
    x_laser = best_astroid[0]
    y_laser = best_astroid[1]
    top_right = np.sum(best_map[0:y_laser, x_laser:width])
    bottom_right = np.sum(best_map[y_laser:height, x_laser:width])
    bottom_left = np.sum(best_map[y_laser:height, 0:x_laser])
    top_left = np.sum(best_map[0:y_laser, 0:x_laser])

    assert(top_left + bottom_left + bottom_right + top_right == max_astroids_detected)

    astroids_vaporized += top_right
    print("1st quarter: {}".format(astroids_vaporized))
    astroids_vaporized += bottom_right
    print("2nd quarter: {}".format(astroids_vaporized))
    astroids_vaporized += bottom_left
    print("3rd quarter: {}".format(astroids_vaporized))
    astroids_vaporized += top_left
    print("4th quarter: {}".format(astroids_vaporized))


    astroids_vaporized -= top_left
    region = best_map[0:y_laser, 0:x_laser]


    two_hundredth = None
    orderded_astroids = []

    for y, row in enumerate(region):
        for x, elem in enumerate(row):
            if elem != 0:
                dx = x_laser - x
                dy = y_laser - y
                gcd = np.gcd(dx, dy)
                dx = dx / gcd
                dy = dy / gcd
                dydx = float(dy) / float(dx)

                orderded_astroids.append((dydx, dx, dy, x, y))


    # print(orderded_astroids)
    orderded_astroids = sorted(orderded_astroids, key=lambda tup: tup[0])
    idx = 200 - astroids_vaporized - 1
    two_hundredth = orderded_astroids[idx]
    print("[DAY10 CHALLENGE B] 200th vaporized astroid: {}, {}".format(two_hundredth[3], two_hundredth[4]))


def main():
    print("Hello Day 10!")
    day10a()
    day10b()


if __name__ == "__main__":
    main()
