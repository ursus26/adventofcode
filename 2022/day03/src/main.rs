use std::fs;
use std::collections::HashSet;

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let answer_part1 = part1(&puzzle_input);
    let answer_part2 = part2(&puzzle_input);

    println!("Day 03 part 1, result: {}", answer_part1);
    println!("Day 03 part 2, result: {}", answer_part2);
}

fn part1(puzzle_input: &str) -> i32 {
    let mut priority_count: i32 = 0;
    for line in puzzle_input.lines() {
        let mid: usize = line.len() / 2;
        let compartment1 = &line[0..mid];
        let compartment2 = &line[mid..line.len()];

        let mut set_compartment1 = HashSet::new();
        for i in compartment1.chars() {
            set_compartment1.insert(i);
        }

        let mut set_compartment2 = HashSet::new();
        for i in compartment2.chars() {
            set_compartment2.insert(i);
        }

        let mut intersection = set_compartment1.intersection(&set_compartment2);
        let shared_item = intersection.next().unwrap();
        priority_count += item_value(shared_item);
    }

    return priority_count;
}

fn part2(puzzle_input: &str) -> i32 {

    let mut priority_count: i32 = 0;
    let mut lines = puzzle_input.lines();

    for line1 in puzzle_input.lines().step_by(3) {
        let line2 = lines.nth(1).unwrap();
        let line3 = lines.nth(0).unwrap();
 
        let mut set1 = HashSet::new();
        for i in line1.chars() {
            set1.insert(i);
        }

        let mut set2 = HashSet::new();
        for i in line2.chars() {
            set2.insert(i);
        }

        let mut set3 = HashSet::new();
        for i in line3.chars() {
            set3.insert(i);
        }

        /* Perform an intersection on set 1, 2, and 3 to find the common item. */
        let intersection1: HashSet<char> = set1.intersection(&set2).map(|x| *x).collect::<HashSet<char>>();
        let mut intersection2 = intersection1.intersection(&set3);
        let shared_item = intersection2.next().unwrap();
        priority_count += item_value(shared_item);
    }
    return priority_count;
}

fn item_value(item: &char) -> i32 {
    return match item {
        'a'..='z' => (*item as i32) - 96,
        'A'..='Z' => (*item as i32) - 38,
        _ => 0,
    };
}