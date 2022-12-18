use std::fs;
use std::cmp::Ordering;
use std::collections::HashMap;
use std::collections::BinaryHeap;

#[derive(Eq, Hash, PartialEq, Copy, Clone)]
struct Vec2 {
    x: usize,
    y: usize,
}

#[derive(Copy, Clone, Eq, PartialEq)]
struct QueueElement {
    distance: u32,
    vertex: Vec2,
}

impl Ord for QueueElement {
    fn cmp(&self, other: &Self) -> Ordering {
        self.distance.cmp(&other.distance).reverse()
    }
}

impl PartialOrd for QueueElement {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

fn dijkstra(grid: &Vec<Vec<u32>>, start: Vec2, end: Vec2) -> u32 {
    /* Initialization */
    let mut priority_queue = BinaryHeap::new();
    let mut dist: HashMap<Vec2, u32> = HashMap::new();
    for (y, row) in grid.iter().enumerate() {
        for (x, _) in row.iter().enumerate() {
            dist.insert(Vec2{ x: x, y: y }, u32::MAX);
        }
    }
    dist.insert(start, 0);
    priority_queue.push(QueueElement{ distance: 0, vertex: start });

    /* Shortest path algorithm. */
    while let Some(QueueElement { distance, vertex }) = priority_queue.pop() {
        /* Stop if we reached the end */
        if vertex == end {
            return distance;
        }

        /* Skip if we already evaluated the vertex */
        if distance > dist[&vertex] {
            continue;
        }

        let current_height = grid[vertex.y][vertex.x];

        /* Get neighbours of vertex. */
        let mut neighbours: Vec<Vec2> = Vec::new();
        if vertex.x > 0 {
            let left = Vec2{ x: vertex.x - 1, y: vertex.y };
            neighbours.push(left);
        }
        if vertex.y < grid.len() && vertex.x + 1 < grid[vertex.y].len() {
            let right = Vec2{ x: vertex.x + 1, y: vertex.y };
            neighbours.push(right);
        }
        if vertex.y > 0 {
            let up = Vec2{ x: vertex.x, y: vertex.y - 1 };
            neighbours.push(up);
        }
        if vertex.y + 1 < grid.len() {
            let down = Vec2{ x: vertex.x, y: vertex.y + 1 };
            neighbours.push(down);
        }

        for neighbour in neighbours {
            let neighbour_height = grid[neighbour.y][neighbour.x];
            if neighbour_height <= current_height + 1 {
                let alternative_distance = distance + 1;
                if alternative_distance < dist[&neighbour] {
                    dist.insert(neighbour, alternative_distance);
                    priority_queue.push(QueueElement{ distance: alternative_distance, vertex: neighbour });
                }
            }
        }
    }

    return u32::MAX;
}

fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let mut grid: Vec<Vec<u32>> = Vec::new();
    let mut start: Vec2 = Vec2{ x: 0, y: 0 };
    let mut end: Vec2 = Vec2{ x: 0, y: 0 };
    let mut start_part2: Vec<Vec2> = Vec::new();

    for (y, line) in puzzle_input.lines().enumerate() {
        let mut row = vec![0; line.len()];
        for (x, c) in line.chars().enumerate() {
            let height: u32 = match c {
                'S' => 'a' as u32,
                'E' => 'z' as u32,
                _ => c as u32,
            };

            if c == 'S' {
                start.x = x;
                start.y = y;
            }
            if c == 'E' {
                end.x = x;
                end.y = y;
            }

            if c == 'S' || c == 'a' {
                start_part2.push(Vec2{ x: x, y: y });
            }

            row[x] = height;
        }
        grid.push(row);
    }

    let answer_part1 = dijkstra(&grid, start, end);

    let mut answer_part2 = u32::MAX;
    for s in start_part2 {
        let cost = dijkstra(&grid, s, end);
        answer_part2 = answer_part2.min(cost);
    }
    
    println!("Day 12 part 1, result: {}", answer_part1);
    println!("Day 12 part 2, result: {}", answer_part2);
}