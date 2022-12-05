use std::fs;

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input;

    let splitted_input: Vec<&str> = puzzle_input.split("\n\n").collect();
    let initial_state: Vec<&str> = splitted_input[0].lines().collect();
    let moves: Vec<&str> = splitted_input[1].lines().collect();

    let mut stacks_part1: Vec<Vec<char>> = Vec::new();
    let mut stacks_part2: Vec<Vec<char>> = Vec::new();

    for i in 1..(initial_state.len()) {
        let line: Vec<char> = initial_state[initial_state.len() - i - 1].chars().collect();
        for j in (1..line.len()).step_by(4) {
            let k = (j-1) / 4;
            while stacks_part1.len() < k + 1 {
                stacks_part1.push(Vec::new());
                stacks_part2.push(Vec::new());
            }
            
            if line[j] != ' ' {
                stacks_part1[k].push(line[j]);
                stacks_part2[k].push(line[j]);
            }
        }
    }

    for move_command in moves {
        let splitted_move_command: Vec<&str> = move_command.split(" ").collect();
        let count: usize = splitted_move_command[1].parse().unwrap();
        let stack_id_source: usize = splitted_move_command[3].parse().unwrap();
        let stack_id_destination: usize = splitted_move_command[5].parse().unwrap();

        for _ in 0..count {
            let c = stacks_part1[stack_id_source-1].pop().unwrap();
            stacks_part1[stack_id_destination-1].push(c);
        }

        let mut buffer_stack: Vec<char> = Vec::new();
        for _ in 0..count {
            let c = stacks_part2[stack_id_source-1].pop().unwrap();
            buffer_stack.push(c);
        }

        for _ in 0..count {
            let c = buffer_stack.pop().unwrap();
            stacks_part2[stack_id_destination-1].push(c);
        }
    }

    let mut answer_part1: String = String::new();
    for s in stacks_part1 {
        answer_part1 += &s.last().unwrap().to_string();
    }

    let mut answer_part2: String = String::new();
    for s in stacks_part2 {
        answer_part2 += &s.last().unwrap().to_string();
    }

    println!("Day 05 part 1, result: {}", answer_part1);
    println!("Day 05 part 2, result: {}", answer_part2);
}
