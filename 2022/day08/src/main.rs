use std::fs;
use std::collections::HashSet;

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input = fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let mut forest_area: Vec<Vec<u32>> = Vec::new();
    for line in puzzle_input.lines() {
        let mut forest_row: Vec<u32> = Vec::new();
        for c in line.chars() {
            let height = c.to_digit(10).unwrap();
            forest_row.push(height);
        }
        forest_area.push(forest_row);
    }

    let mut visible_trees: HashSet<(usize, usize)> = HashSet::new();
    for y in 1..forest_area.len()-1 {
        let forest_row = &forest_area[y];
        let mut tallest_height = forest_row[0];

        for x in 1..forest_row.len()-1 {
            let height = &forest_row[x];
            if *height > tallest_height {
                tallest_height = *height;
                visible_trees.insert((x, y));
            }
        }

        tallest_height = *forest_row.last().unwrap();
        let last_index = forest_row.len() - 1;
        for x in 1..forest_row.len()-1 {
            let height = &forest_row[last_index - x];
            if *height > tallest_height {
                tallest_height = *height;
                visible_trees.insert((last_index - x, y));
            }
        }
    }

    for x in 1..forest_area[0].len()-1 {
        let mut tallest_height = forest_area[0][x];
        for y in 1..forest_area.len()-1 {
            let height = forest_area[y][x];
            if height > tallest_height {
                tallest_height = height;
                visible_trees.insert((x, y));
            }
        }

        tallest_height = forest_area.last().unwrap()[x];
        let last_index = forest_area.len() - 1;
        for y in 1..forest_area.len()-1 {
            let height = forest_area[last_index - y][x];
            if height > tallest_height {
                tallest_height = height;
                visible_trees.insert((x, last_index - y));
            }
        }
    }
    let answer_part1 = visible_trees.len() + 2 * forest_area.len() + 2 * (forest_area[0].len() - 2);

    /* Part 2*/
    let mut max_scenic_score = 0;
    for y in 1..forest_area.len()-1 {
        for x in 1..forest_area[y].len()-1 {
            let tree_height = forest_area[y][x];

            let mut visible_trees_right = 0;
            for x2 in x+1..forest_area[y].len() {
                if forest_area[y][x2] >= tree_height {
                    visible_trees_right = x2 - x;
                    break;
                }
            }
            if visible_trees_right == 0 {
                visible_trees_right = forest_area[y].len() - x - 1;
            }

            let mut visible_trees_left = 0;
            for x2 in 1..=x {
                if forest_area[y][x - x2] >= tree_height {
                    visible_trees_left = x2;
                    break;
                }
            }
            if visible_trees_left == 0 {
                visible_trees_left = x;
            }

            let mut visible_trees_bottom = 0;
            for y2 in y+1..forest_area.len() {
                if forest_area[y2][x] >= tree_height {
                    visible_trees_bottom = y2 - y;
                    break;
                }
            }
            if visible_trees_bottom == 0 {
                visible_trees_bottom = forest_area.len() - y - 1;
            }

            let mut visible_trees_top = 0;
            for y2 in 1..=y {
                if forest_area[y - y2][x] >= tree_height {
                    visible_trees_top = y2;
                    break;
                }
            }
            if visible_trees_top == 0 {
                visible_trees_top = y;
            }

            let scenic_score = visible_trees_left * visible_trees_right * visible_trees_bottom * visible_trees_top;
            if scenic_score > max_scenic_score {
                max_scenic_score = scenic_score;
            }
        }
    }

    println!("Day 08 part 1, result: {}", answer_part1);
    println!("Day 08 part 2, result: {}", max_scenic_score);
}