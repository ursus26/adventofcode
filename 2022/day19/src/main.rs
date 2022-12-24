use std::fs;
use regex::Regex;

#[derive(Copy, Clone)]
struct Blueprint {
    id: u32,
    ore_robot_cost: u32,
    clay_robot_cost: u32,
    obsidian_robot_ore_cost: u32,
    obsidian_robot_clay_cost: u32,
    geode_robot_ore_cost: u32,
    geode_robot_obsidian_cost: u32
}

#[derive(Copy, Clone)]
struct MiningState {
    ore: u32,
    clay: u32,
    obsidian: u32,
    geode: u32,
    ore_robots: u32,
    clay_robots: u32,
    obsidian_robots: u32,
    geode_robots: u32,
    total_clay: u32
}

impl MiningState {
    fn new() -> Self {
        return Self {
            ore: 0,
            clay: 0,
            obsidian: 0,
            geode: 0,
            ore_robots: 1,
            clay_robots: 0,
            obsidian_robots: 0,
            geode_robots: 0,
            total_clay: 0
        };
    }

    fn step(&mut self) {
        self.ore += self.ore_robots;
        self.clay += self.clay_robots;
        self.obsidian += self.obsidian_robots;
        self.geode += self.geode_robots;
        self.total_clay += self.clay_robots;
    }
}

fn calculate_max_geodes(blueprint: &Blueprint, t_max: u32) -> u32 {
    let mut stack: Vec<(u32, MiningState)> = Vec::new();
    stack.push((0, MiningState::new()));

    let mut max_geode = 0;

    while let Some(stack_element) = stack.pop() {
        let time = stack_element.0;
        let state = stack_element.1;

        if time == t_max {
            max_geode = max_geode.max(state.geode);
            continue;
        }

        /* Stop evaluating this branch if we cannot exceed the current maximum of geodes. */
        let time_left = t_max - time;
        let max_geodes_left = ((time_left * (time_left - 1)) / 2) + (state.geode_robots * time_left);
        if max_geodes_left + state.geode <= max_geode {
            continue;
        }

        /* Build Geode robot decision. */
        if state.ore >= blueprint.geode_robot_ore_cost && state.obsidian >= blueprint.geode_robot_obsidian_cost {
            let mut next_state = state.clone();
            next_state.step();
            next_state.ore -= blueprint.geode_robot_ore_cost;
            next_state.obsidian -= blueprint.geode_robot_obsidian_cost;
            next_state.geode_robots += 1;
            stack.push((time + 1, next_state));
        }

        /* Build Obsidian robot decision. */
        if state.ore >= blueprint.obsidian_robot_ore_cost && state.clay >= blueprint.obsidian_robot_clay_cost {
            let mut next_state = state.clone();
            next_state.step();
            next_state.ore -= blueprint.obsidian_robot_ore_cost;
            next_state.clay -= blueprint.obsidian_robot_clay_cost;
            next_state.obsidian_robots += 1;
            stack.push((time + 1, next_state));
        }

        /* Build clay robot decision. */
        if state.ore >= blueprint.clay_robot_cost && state.total_clay < (blueprint.obsidian_robot_clay_cost * blueprint.geode_robot_obsidian_cost) {
            let mut next_state = state.clone();
            next_state.step();
            next_state.ore -= blueprint.clay_robot_cost;
            next_state.clay_robots += 1;
            stack.push((time + 1, next_state));
        }

        /* Build ore robot decision. */
        if state.ore >= blueprint.ore_robot_cost {
            let mut next_state = state.clone();
            next_state.step();
            next_state.ore -= blueprint.ore_robot_cost;
            next_state.ore_robots += 1;
            stack.push((time + 1, next_state));
        }

        /* Idle decision. */
        let mut next_state = state.clone();
        next_state.step();
        stack.push((time + 1, next_state));
    }

    return max_geode;
}

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let re = Regex::new(r"\D*(?P<id>\d+)\D*(?P<x1>\d+)\D*(?P<x2>\d+)\D*(?P<x3>\d+)\D*(?P<x4>\d+)\D*(?P<x5>\d+)\D*(?P<x6>\d+)[^\n]*").unwrap();
    let mut blueprints: Vec<Blueprint> = Vec::new();
    for caps in re.captures_iter(puzzle_input) {
        let id = (&caps["id"]).parse::<u32>().unwrap();
        let x1 = (&caps["x1"]).parse::<u32>().unwrap();
        let x2 = (&caps["x2"]).parse::<u32>().unwrap();
        let x3 = (&caps["x3"]).parse::<u32>().unwrap();
        let x4 = (&caps["x4"]).parse::<u32>().unwrap();
        let x5 = (&caps["x5"]).parse::<u32>().unwrap();
        let x6 = (&caps["x6"]).parse::<u32>().unwrap();
        let blueprint = Blueprint {
            id: id,
            ore_robot_cost: x1,
            clay_robot_cost: x2,
            obsidian_robot_ore_cost: x3,
            obsidian_robot_clay_cost: x4,
            geode_robot_ore_cost: x5,
            geode_robot_obsidian_cost: x6,
        };
        blueprints.push(blueprint);
    }

    /* Part 1 */
    let mut answer_part1 = 0;
    for i in 0..blueprints.len() {
        let blueprint = blueprints.get(i).unwrap();
        let max_geode = calculate_max_geodes(&blueprint, 24);
        println!("Blueprint {} opens a maximum of {} geodes", blueprint.id, max_geode);
        answer_part1 += blueprint.id * max_geode;
    }

    /* Part 2 */
    let mut answer_part2 = 1;
    for i in 0..3 {
        let blueprint = blueprints.get(i).unwrap();
        let max_geode = calculate_max_geodes(&blueprint, 32);
        println!("Part 2 Blueprint {} opens a maximum of {} geodes", blueprint.id, max_geode);
        answer_part2 *= max_geode;
    }

    println!("Day 19 part 1, result: {}", answer_part1);
    println!("Day 19 part 2, result: {}", answer_part2);
}