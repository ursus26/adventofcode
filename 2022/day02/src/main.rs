use std::fs;

#[derive(PartialEq, Clone)]
enum Shape {
    Rock,
    Paper,
    Scissor,
}

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let mut score_counter_part1: i32 = 0;
    let mut score_counter_part2: i32 = 0;
    for line in puzzle_input.lines() {
        let opponent_guess = match line.chars().nth(0).unwrap() {
            'A' => Shape::Rock,
            'B' => Shape::Paper,
            'C' => Shape::Scissor,
            _ => panic!("Opponent made an unexpectged guess"),
        };
        let my_guess = match line.chars().nth(2).unwrap() {
            'X' => Shape::Rock,
            'Y' => Shape::Paper,
            'Z' => Shape::Scissor,
            _ => panic!("I made an unexpectged guess"),
        };
        let my_guess_elf_strategy = match line.chars().nth(2).unwrap() {
            'X' => get_losing_shape(&opponent_guess),
            'Y' => opponent_guess.clone(),
            'Z' => get_wining_shape(&opponent_guess),
            _ => panic!("I made an unexpectged guess"),
        };

        score_counter_part1 += rock_paper_scissors(&my_guess, &opponent_guess);
        score_counter_part2 += rock_paper_scissors(&my_guess_elf_strategy, &opponent_guess);
    }

    println!("Day 02 part 1, result: {}", score_counter_part1);
    println!("Day 02 part 2, result: {}", score_counter_part2);
}

fn rock_paper_scissors(my_guess: &Shape, opponent_guess: &Shape) -> i32 {
    let loss = 0;
    let draw = 3;
    let win = 6;
    let shape_points = get_shape_points(&my_guess);
    if *my_guess == Shape::Rock {
        match opponent_guess {
            Shape::Rock => shape_points + draw,
            Shape::Paper => shape_points + loss,
            Shape::Scissor => shape_points + win,
        }
    } else if *my_guess == Shape::Paper {
        match opponent_guess {
            Shape::Rock => shape_points + win,
            Shape::Paper => shape_points + draw,
            Shape::Scissor => shape_points + loss,
        }
    } else {
        match opponent_guess {
            Shape::Rock => shape_points + loss,
            Shape::Paper => shape_points + win,
            Shape::Scissor => shape_points + draw,
        }
    }
}

fn get_shape_points(shape: &Shape) -> i32 {
    match shape {
        Shape::Rock => 1,
        Shape::Paper => 2,
        Shape::Scissor => 3,
    }
}

fn get_wining_shape(opponent_shape: &Shape) -> Shape {
    match opponent_shape {
        Shape::Rock => Shape::Paper,
        Shape::Paper => Shape::Scissor,
        Shape::Scissor => Shape::Rock,
    }
}

fn get_losing_shape(opponent_shape: &Shape) -> Shape {
    match opponent_shape {
        Shape::Rock => Shape::Scissor,
        Shape::Paper => Shape::Rock,
        Shape::Scissor => Shape::Paper,
    }
}
