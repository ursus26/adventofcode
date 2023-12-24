from collections import namedtuple
import sys

RangeMapping = namedtuple("RangeMapping", "source_start, dest_start, length")


def map_walker(mappings, initial_value):
    current_value = initial_value
    next_value = -1
    for mapping in mappings:
        for range_map in mapping:
            if current_value >= range_map.source_start and current_value < range_map.source_start + range_map.length:
                next_value = range_map.dest_start + current_value - range_map.source_start
                break

        if next_value == -1:
            next_value = current_value

        current_value = next_value
        next_value = -1

    return current_value


def map_walker_range(mappings, start, end):
    assert end > start
    
    if(len(mappings) == 0):
        return start
    
    mapping = mappings[0]
    length = end - start
    for range_map in mapping:
        source_start = range_map.source_start
        dest_start = range_map.dest_start
        range_length = range_map.length
        source_end = source_start + range_length

        # Bound checking
        start_in_bound = start >= source_start and start < source_end
        end_in_bound = end > source_start and end <= source_end
        search_range_contains_map_range = source_start >= start and source_end <= end

        if not start_in_bound and not end_in_bound and not search_range_contains_map_range:
            continue
        
        if start_in_bound and end_in_bound:
            next_start = dest_start + start - source_start
            return map_walker_range(mappings[1:], next_start, next_start + length)
        elif start_in_bound and not end_in_bound:
            next_start = dest_start + start - source_start
            next_length = source_end - start

            out = map_walker_range(mappings[1:], next_start, next_start + next_length)
            out = min(map_walker_range(mappings, source_end, end), out)
            return out
        elif not start_in_bound and end_in_bound:
            out = map_walker_range(mappings, start, source_start)
            next_range = end - source_start
            return min(map_walker_range(mappings[1:], dest_start, dest_start + next_range), out)
        elif search_range_contains_map_range:
            out = map_walker_range(mappings, start, source_start)
            out = min(map_walker_range(mappings[1:], dest_start, dest_start + range_length), out)
            out = min(map_walker_range(mappings, source_end, end), out)
            return out
        
    return map_walker_range(mappings[1:], start, end)


def main():
    f = open("input.txt")
    data = f.read().split("\n\n")

    seeds = [int(x) for x in data[0][7:].split(" ")]

    mappings = []
    for mapping in data[1:]:
        ranges = []
        lines = mapping.splitlines()
        for line in lines[1:]:
            numbers = [int(x) for x in line.split(" ")]
            ranges.append(RangeMapping(source_start=numbers[1], dest_start=numbers[0], length=numbers[2]))

        mappings.append(ranges)

    answer_part1 = min([map_walker(mappings, x) for x in seeds])
    print("Day 05 part 1, result: {}".format(answer_part1))


    answer_part2 = sys.maxsize
    for i in range(0, len(seeds), 2):
        seed_start = seeds[i]
        seed_length = seeds[i + 1]

        result = map_walker_range(mappings, seed_start, seed_start + seed_length)
        if result < answer_part2:
            answer_part2 = result

    print("Day 05 part 2, result: {}".format(answer_part2))


if __name__ == "__main__":
    main();