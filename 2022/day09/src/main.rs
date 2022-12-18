use std::fs;
use std::collections::HashSet;

#[derive(Eq, Hash, PartialEq, Copy, Clone)]
struct Vec2 {
    x: i32,
    y: i32,
}

#[derive(PartialEq, Copy, Clone)]
enum Direction {
    Up,
    Down,
    Left,
    Right
}

fn move1(item: &mut Vec2, direction: Direction) {
    match direction {
        Direction::Up       => item.y += 1,
        Direction::Down     => item.y -= 1,
        Direction::Left     => item.x -= 1,
        Direction::Right    => item.x += 1,
    } 
}

fn check_tail(head: &Vec2, tail: &mut Vec2) {
    let dx = (head.x - tail.x).abs();
    let dy = (head.y - tail.y).abs();

    if dx == 2 {
        if head.x > tail.x {
            move1(tail, Direction::Right);
        }
        else {
            move1(tail, Direction::Left);
        }

        if head.y > tail.y {
            move1(tail, Direction::Up);
        }
        else if head.y < tail.y {
            move1(tail, Direction::Down);
        }
    }
    else if dy == 2 {
        if head.y > tail.y {
            move1(tail, Direction::Up);
        }
        else {
            move1(tail, Direction::Down);
        }

        if head.x > tail.x {
            move1(tail, Direction::Right);
        }
        else if head.x < tail.x {
            move1(tail, Direction::Left);
        }
    }
}

fn simulate_rope(input: &str, tail_size: usize) -> usize {
    /* Initialize simulation state */
    let mut rope: Vec<Vec2> = Vec::new();    
    for _ in 0..tail_size+1 {
        rope.push(Vec2{ x: 0, y: 0 });
    }
    let mut visited_locations: HashSet<Vec2> = HashSet::new();
    visited_locations.insert(rope[0]);

    /* Execute rope simulation commands. */
    for line in input.lines() {
        let words: Vec<&str> = line.split_whitespace().collect();
        let direction = match words[0] {
            "U" => Direction::Up,
            "D" => Direction::Down,
            "L" => Direction::Left,
            "R" => Direction::Right,
            _   => panic!("Invalid direction"),
        };
        let steps: usize = words[1].parse().unwrap();

        for _ in 0..steps {
            move1(&mut rope[0], direction);
            for i in 0..tail_size {
                let head: Vec2 = *rope.get(i).unwrap();
                let mut tail = rope.get_mut(i+1).unwrap();
                check_tail(&head, &mut tail);
            }
            visited_locations.insert(rope[tail_size]);
        }
    }

    return visited_locations.len();
}

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let answer_part1 = simulate_rope(puzzle_input, 1);
    println!("Day 09 part 1, result: {}", answer_part1);

    let answer_part2 = simulate_rope(puzzle_input, 9);
    println!("Day 09 part 2, result: {}", answer_part2);
}