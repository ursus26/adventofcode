import re
import math

def main():
    f = open("input.txt")
    data = f.read().split("\n")

    answer_part1 = 0
    answer_part2 = 0

    move_instructions = [x for x in data[0]]

    # Parse
    p = re.compile(r"^(\w+) = \((\w+), (\w+)\)$")
    network = {}
    for line in data[2:]:
        regex_match = p.match(line)
        if regex_match == None:
            print("Error, could not match input line")
            continue
        else:
            network[regex_match.group(1)] = (regex_match.group(2), regex_match.group(3))

    # Part 1
    current_node = "AAA"
    instruction_idx = 0
    while current_node != "ZZZ":
        left, right = network[current_node]
        if move_instructions[instruction_idx] == 'L':
            current_node = left
        else:
            current_node = right
        instruction_idx = (instruction_idx + 1) % len(move_instructions)
        answer_part1 += 1

    # Part 2
    end_nodes = {x for x in network.keys() if x[2] == 'Z'}
    instruction_idx = 0
    cycles = []
    for current_node in [x for x in network.keys() if x[2] == 'A']:
        end_node_found = False
        start_cycle = 0
        steps = 0

        while 1:
            left, right = network[current_node]
            if move_instructions[instruction_idx] == 'L':
                current_node = left
            else:
                current_node = right
            instruction_idx = (instruction_idx + 1) % len(move_instructions)
            steps += 1

            if current_node in end_nodes:
                if not end_node_found:
                    end_node_found = True
                    start_cycle = steps
                    continue
                else:
                    print("Cycle found: {}".format(steps - start_cycle))
                    cycles.append(steps - start_cycle)
                    break

    answer_part2 = 1
    for cycle in cycles:
        answer_part2 = math.lcm(answer_part2, cycle)

    print("Day 08 part 2, result: {}".format(answer_part1))
    print("Day 08 part 2, result: {}".format(answer_part2))


if __name__ == "__main__":
    main();