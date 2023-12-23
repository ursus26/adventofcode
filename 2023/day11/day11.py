from collections import namedtuple


Vec2 = namedtuple("Vec2", "x, y")


def main():
    f = open("input.txt")
    data = f.read().split("\n")

    answer_part1 = 0
    answer_part2 = 0

    # Column shift detection.
    x = 0
    dx = []
    for i in range(len(data[0])):
        if all([line[i] == '.' for line in data]):
            x += 1
        dx.append(x)

    galaxies_part1 = []
    galaxies_part2 = []
    dy = 0
    expansion_factor = 1000000
    for y, line in enumerate(data):
        if(all([c == '.' for c in line])):
            dy += 1
        else:
            for x, c in enumerate(line):
                if c == '#':
                    galaxies_part1.append(Vec2(x + dx[x], y + dy))
                    galaxies_part2.append(Vec2(x + (expansion_factor - 1) * dx[x], y + (expansion_factor - 1) * dy))

    # Calculate manhatten distance.
    for i in range(len(galaxies_part1) - 1):
        galaxy1 = galaxies_part1[i]
        galaxy3 = galaxies_part2[i]
        for j in range(i+1, len(galaxies_part1)):
            galaxy2 = galaxies_part1[j]
            galaxy4 = galaxies_part2[j]

            answer_part1 += abs(galaxy1.x - galaxy2.x) + abs(galaxy1.y - galaxy2.y)
            answer_part2 += abs(galaxy3.x - galaxy4.x) + abs(galaxy3.y - galaxy4.y)

    print("Day 11 part 1, result: {}".format(answer_part1))
    print("Day 11 part 2, result: {}".format(answer_part2))


if __name__ == "__main__":
    main();