use std::fs;
use std::collections::VecDeque;

#[derive(Copy, Clone)]
struct MixingElement {
    original_idx: usize,
    value: i64
}

impl MixingElement {
    fn new(idx: usize, value: i64) -> Self {
        return Self { original_idx: idx, value: value };
    }
}

fn mix(data: &Vec<i64>, key: i64, rounds: i32) -> i64 {
    let mut mixing: VecDeque<MixingElement> = VecDeque::new();
    for i in 0..data.len() {
        let number = data[i] * key;
        mixing.push_back(MixingElement::new(i, number));
    }

    let ring_size = mixing.len() as i64;    
    for _ in 0..rounds {
        for i in 0..ring_size {
            let idx = mixing.iter().position(|&x| x.original_idx == (i as usize)).unwrap();
            let element: MixingElement = mixing.remove(idx).unwrap();
    
            let new_index: usize = ((idx as i64) + element.value).rem_euclid(ring_size - 1) as usize;
            mixing.insert(new_index, element);
        }
    }

    /* Calculate mixing result. */
    let i0 = mixing.iter().position(|&x| x.value == 0).unwrap();
    let i1: usize = (i0 + 1000) % mixing.len();
    let i2: usize = (i0 + 2000) % mixing.len();
    let i3: usize = (i0 + 3000) % mixing.len();
    let answer = mixing[i1].value + mixing[i2].value + mixing[i3].value;
    return answer;
}

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let mut numbers: Vec<i64> = Vec::new();
    for x in puzzle_input.lines() {
        let number: i64 = x.parse().unwrap();
        numbers.push(number);
    }

    let answer_part1 = mix(&numbers, 1, 1);
    println!("Day 20 part 1, result: {}", answer_part1);

    let answer_part2 = mix(&numbers, 811589153, 10);
    println!("Day 20 part 2, result: {}", answer_part2);
}