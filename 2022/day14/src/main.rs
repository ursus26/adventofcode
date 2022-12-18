use std::fs;
use std::collections::HashMap;
use regex::Regex;

#[derive(Eq, Hash, PartialEq, Copy, Clone)]
struct Vec2 {
    x: i32,
    y: i32,
}

#[derive(PartialEq, Clone)]
enum GridType {
    Rock,
    Sand,
    Empty,
}

fn part1(grid: &mut HashMap<Vec2, GridType>, max_y: i32) -> i32 {
    let insert_point = Vec2{ x: 500, y: 0 };
    let mut sand = insert_point;
    let mut sand_count = 0;

    /* Part 1 */
    loop {
        let choice1 = Vec2{ x: sand.x, y: sand.y + 1 };
        let choice2 = Vec2{ x: sand.x - 1, y: sand.y + 1 };
        let choice3 = Vec2{ x: sand.x + 1, y: sand.y + 1 };

        if !grid.contains_key(&choice1) {
            grid.insert(choice1, GridType::Empty);
        }
        if !grid.contains_key(&choice2) {
            grid.insert(choice2, GridType::Empty);
        }
        if !grid.contains_key(&choice3) {
            grid.insert(choice3, GridType::Empty);
        }

        if grid[&choice1] == GridType::Empty {
            sand = choice1;
        }
        else if grid[&choice2] == GridType::Empty {
            sand = choice2;
        }
        else if grid[&choice3] == GridType::Empty {
            sand = choice3;
        }
        else {
            grid.insert(sand, GridType::Sand);
            sand = insert_point;
            sand_count += 1;
            continue;
        }

        if sand.y > max_y {
            return sand_count;
        }
    }
}

fn part2(grid: &mut HashMap<Vec2, GridType>, max_y: i32) -> i32 {
    let insert_point = Vec2{ x: 500, y: 0 };
    grid.insert(insert_point, GridType::Empty);
    let mut sand = insert_point;
    let mut sand_count = 0;

    /* Part 1 */
    while grid[&insert_point] != GridType::Sand {

        if sand.y + 1 == max_y {
            grid.insert(sand, GridType::Sand);
            sand = insert_point;
            sand_count += 1;
            continue;
        }

        let choice1 = Vec2{ x: sand.x, y: sand.y + 1 };
        let choice2 = Vec2{ x: sand.x - 1, y: sand.y + 1 };
        let choice3 = Vec2{ x: sand.x + 1, y: sand.y + 1 };

        if !grid.contains_key(&choice1) {
            grid.insert(choice1, GridType::Empty);
        }
        if !grid.contains_key(&choice2) {
            grid.insert(choice2, GridType::Empty);
        }
        if !grid.contains_key(&choice3) {
            grid.insert(choice3, GridType::Empty);
        }

        if grid[&choice1] == GridType::Empty {
            sand = choice1;
        }
        else if grid[&choice2] == GridType::Empty {
            sand = choice2;
        }
        else if grid[&choice3] == GridType::Empty {
            sand = choice3;
        }
        else {
            grid.insert(sand, GridType::Sand);
            sand = insert_point;
            sand_count += 1;
            continue;
        }
    }

    return sand_count;
}

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let re = Regex::new(r"(?P<x>\d+),(?P<y>\d+)").unwrap();

    let mut grid_part1: HashMap<Vec2, GridType> = HashMap::new();
    let mut max_y = 0;
    for line in puzzle_input.lines() {
        let mut points: Vec<Vec2> = Vec::new();
        for caps in re.captures_iter(line) {
            let x = (&caps["x"]).parse::<i32>().unwrap();
            let y = (&caps["y"]).parse::<i32>().unwrap();

            let pos = Vec2{ x: x, y: y };
            points.push(pos);

            if y > max_y {
                max_y = y;
            }
        }

        let mut start = points.first().unwrap();
        for end in points.iter().skip(1) {
            let dx = end.x - start.x;
            let dy = end.y - start.y;

            if dx != 0 {
                let sign = dx.signum();
                for i in 0..=dx.abs() {
                    let rock_position = Vec2{ x: (sign * i) + start.x, y: start.y };
                    grid_part1.insert(rock_position, GridType::Rock);
                }
            }
            else if dy != 0 {
                let sign = dy.signum();
                for i in 0..=dy.abs() {
                    let rock_position = Vec2{ x: start.x, y: (sign * i) + start.y };
                    grid_part1.insert(rock_position, GridType::Rock);
                }
            }

            start = end;
        }
    }    

    let mut grid_part2: HashMap<Vec2, GridType> = grid_part1.clone(); 

    let answer_part1 = part1(&mut grid_part1, max_y);
    println!("Day 14 part 1, result: {}", answer_part1);

    let answer_part2 = part2(&mut grid_part2, max_y + 2);
    println!("Day 14 part 2, result: {}", answer_part2);
}