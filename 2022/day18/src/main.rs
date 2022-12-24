use std::fs;
use std::collections::HashSet;
use std::collections::VecDeque;

#[derive(Eq, Hash, PartialEq, Copy, Clone)]
struct Vec3 {
    x: i32,
    y: i32,
    z: i32
}

impl Vec3 {
    fn new(x: i32, y: i32, z: i32) -> Self {
        return Self { x: x, y: y, z: z };
    }

    fn from(other: Vec3) -> Self {
        return Self { x: other.x, y: other.y, z: other.z };
    }

    fn neighbours(&self) -> [Vec3; 6] {
        return [Vec3::new(self.x+1, self.y, self.z), Vec3::new(self.x-1, self.y, self.z),
            Vec3::new(self.x, self.y+1, self.z), Vec3::new(self.x, self.y-1, self.z),
            Vec3::new(self.x, self.y, self.z+1), Vec3::new(self.x, self.y, self.z-1)];
    }

    fn in_bounds(&self, min_bound: Vec3, max_bound: Vec3) -> bool {
        if min_bound.x <= self.x && self.x <= max_bound.x
        && min_bound.y <= self.y && self.y <= max_bound.y
        && min_bound.z <= self.z && self.z <= max_bound.z {
            return true;
        }
        else {
            return false;
        }
    }
}

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let mut obsidian_cubes: HashSet<Vec3> = HashSet::new();
    let mut min_box = Vec3::new(0, 0, 0);
    let mut max_box = Vec3::new(0, 0, 0);
    for line in puzzle_input.lines() {
        let raw_coords: Vec<&str> = line.split(',').collect();
        let x: i32 = raw_coords[0].parse().unwrap();
        let y: i32 = raw_coords[1].parse().unwrap();
        let z: i32 = raw_coords[2].parse().unwrap();

        obsidian_cubes.insert(Vec3::new(x, y, z));

        min_box.x = min_box.x.min(x);
        min_box.y = min_box.y.min(y);
        min_box.z = min_box.z.min(z);

        max_box.x = max_box.x.max(x);
        max_box.y = max_box.y.max(y);
        max_box.z = max_box.z.max(z);
    }

    /* Part 1 */
    let mut answer_part1 = 0;
    let mut air_cubes: HashSet<Vec3> = HashSet::new();
    for cube in obsidian_cubes.iter() {
        for neighbour in cube.neighbours() {
            if !obsidian_cubes.contains(&neighbour) {
                answer_part1 += 1;
                air_cubes.insert(Vec3::from(neighbour));
            }
        }
    }
    println!("Day 18 part 1, result: {}", answer_part1);
    
    /* Part 2 */
    min_box.x -= 1;
    min_box.y -= 1;
    min_box.z -= 1;
    max_box.x += 1;
    max_box.y += 1;
    max_box.z += 1;

    /* Find all the outer air cubes. */
    let mut queue: VecDeque<Vec3> = VecDeque::new();
    queue.push_back(min_box);
    let mut evaluated_cubes: HashSet<Vec3> = HashSet::new();
    let mut outer_air_cubes: HashSet<Vec3> = HashSet::new();
    while let Some(cube) = queue.pop_front() {
        if evaluated_cubes.contains(&cube) {
            continue;
        }
        else {
            evaluated_cubes.insert(cube);
        }

        if air_cubes.contains(&cube) && !outer_air_cubes.contains(&cube) {
            outer_air_cubes.insert(cube);
        }

        for neighbour in cube.neighbours() {
            if !obsidian_cubes.contains(&neighbour) && !evaluated_cubes.contains(&neighbour) && neighbour.in_bounds(min_box, max_box) {
                queue.push_back(neighbour);
            }
        }
    }

    /* Remove all surfaces facing the inner air pockets. */
    let inner_air_cubes: HashSet<&Vec3> = air_cubes.difference(&outer_air_cubes).collect();
    let mut answer_part2 = answer_part1;
    for air_cube in inner_air_cubes.iter() {
        for neighbour in air_cube.neighbours() {
            if obsidian_cubes.contains(&neighbour) {
                answer_part2 -= 1;
            }
        }
    }    
    println!("Day 18 part 2, result: {}", answer_part2);
}