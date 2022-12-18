use std::fs;
use std::collections::VecDeque;

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input: Vec<char> = raw_puzzle_input.trim().chars().collect();

    let mut answer_part1 = 0;
    let mut answer_part2 = 0;

    let mut deq: VecDeque<char> = VecDeque::new();
    for i in 0..4 {
        deq.push_back(puzzle_input[i]);
    }

    for (i, c) in puzzle_input.iter().enumerate().skip(4) {
        deq.pop_front();
        deq.push_back(*c);

        if deq[0] != deq[1] && deq[0] != deq[2] && deq[0] != deq[3]
        && deq[1] != deq[2] && deq[1] != deq[3] && deq[2] != deq[3] {
            answer_part1 = i + 1;
            break;
        }
    }

    let mut deq2: VecDeque<char> = VecDeque::new();
    for i in 0..14 {
        deq2.push_back(puzzle_input[i]);
    }
    for (i, c) in puzzle_input.iter().enumerate().skip(14) {
        deq2.pop_front();
        deq2.push_back(*c);

        let mut found_start_of_message = true;
        for j in 0..deq2.len() {
            for k in j+1..deq2.len() {
                if deq2[j] == deq2[k] {
                    found_start_of_message = false;
                    break;
                }
            }

            if !found_start_of_message {
                break;
            }
        }

        if found_start_of_message {
            answer_part2 = i + 1;
            break;
        }
    }

    println!("Day 06 part 1, result: {}", answer_part1);
    println!("Day 06 part 2, result: {}", answer_part2);
}