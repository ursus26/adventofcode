use std::fs;

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input = fs::read_to_string(puzzle_input_file_name)
        .expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input
        .trim();

    /* Count the calories per elf. */
    let mut elves = Vec::new();
    let mut calories_counter: i32 = 0;
    for line in puzzle_input.lines() {
        if line == "" {
            elves.push(calories_counter);
            calories_counter = 0;
            continue;
        }

        let calories: i32 = line.parse().expect("Not a number!");
        calories_counter += calories;
    }

    elves.sort();
    let elf_count = elves.len();
    let part1 = elves.last().expect("There should be atleast 1 elf in the data set");
    let part2 = elves[elf_count - 1] + elves[elf_count - 2] + elves[elf_count - 3];

    println!("Day 01 part 1, result: {}", part1);
    println!("Day 01 part 2, result: {}", part2);
}
