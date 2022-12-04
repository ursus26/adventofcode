use std::fs;
use std::collections::HashSet;
use regex::Regex;

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let re = Regex::new(r"(?P<lowerbound1>\d+)-(?P<upperbound1>\d+),(?P<lowerbound2>\d+)-(?P<upperbound2>\d+)").unwrap();
    let mut answer_part1 = 0;
    let mut answer_part2 = 0;
    for caps in re.captures_iter(puzzle_input) {
        let lowerbound1 = (&caps["lowerbound1"]).parse::<i32>().unwrap();
        let upperbound1 = (&caps["upperbound1"]).parse::<i32>().unwrap();
        let lowerbound2 = (&caps["lowerbound2"]).parse::<i32>().unwrap();
        let upperbound2 = (&caps["upperbound2"]).parse::<i32>().unwrap();

        let range1 = (lowerbound1..=upperbound1).collect::<HashSet<i32>>();
        let range2 = (lowerbound2..=upperbound2).collect::<HashSet<i32>>();

        if range1.is_subset(&range2) || range1.is_superset(&range2) {
            answer_part1 += 1;
        }

        let intersection_count = range1.intersection(&range2).count();
        if intersection_count > 0 {
            answer_part2 += 1
        }
    }

    println!("Day 04 part 1, result: {}", answer_part1);
    println!("Day 04 part 2, result: {}", answer_part2);
}