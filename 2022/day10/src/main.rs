use std::fs;

#[derive(PartialEq, Copy, Clone)]
enum Mnemonic {
    ADDX,
    NOOP,
}

fn get_instruction_cycles(op: Mnemonic) -> usize {
    match op {
        Mnemonic::ADDX => 2,
        Mnemonic::NOOP => 1,
    }
}

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let mut answer_part1 = 0;
    let mut register: i32 = 1;
    let mut cycle: i32 = 0;
    let mut CRT_line: String = String::from("");

    for line in puzzle_input.lines() {
        let words: Vec<&str> = line.split_whitespace().collect();
        let op: Mnemonic = match words[0] {
            "addx" => Mnemonic::ADDX,
            "noop" => Mnemonic::NOOP,
            _   => panic!("Invalid direction"),
        };

        for _ in 0..get_instruction_cycles(op) {
            cycle += 1;

            if (cycle - 20) % 40 == 0 {
                answer_part1 += cycle * register;
            }

            let screen_x = (cycle - 1) % 40;
            if register - 1 <= screen_x && screen_x <= register + 1 {
                CRT_line += "# ";
            }
            else {
                CRT_line += "  ";
            }

            if cycle % 40 == 0 {
                println!("{}", CRT_line);
                CRT_line.clear();
            }
        }

        if op == Mnemonic::ADDX {
            let value: i32 = words[1].parse().unwrap();
            register += value;
        }
    }

    println!("Day 10 part 1, result: {}", answer_part1);
    println!("Day 10 part 2, Look at the printed screen and find the 8 capital letters");
}