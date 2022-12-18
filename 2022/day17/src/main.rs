use std::fs;
use std::collections::HashMap;

#[derive(Eq, Hash, PartialEq, Copy, Clone)]
struct Vec2 {
    x: i64,
    y: i64,
}

impl Vec2 {
    fn new(x: i64, y: i64) -> Self {
        return Self{ x: x, y: y };
    }

    fn add(&mut self, other: Vec2) {
        self.x += other.x;
        self.y += other.y;
    }
}


#[derive(PartialEq, Copy, Clone)]
enum GridType {
    Rock,
    Empty,
}

#[derive(PartialEq, Copy, Clone)]
enum ShapeType {
    HorizontalLine,
    Plus,
    MirroredL,
    VerticalLine,
    Square,
}

struct Shape {
    points: Vec<Vec2>,
}

impl Shape {
    fn translate(&mut self, other: Vec2) {
        for point in &mut self.points {
            point.add(other);
        }
    }

    fn push_left(&mut self, tetris: &Tetris) {
        for p in &self.points {
            if p.x == 0 {
                return;
            }

            if tetris.grid[p.y as usize][(p.x - 1) as usize] == GridType::Rock {
                return;
            }
        }

        self.translate(Vec2::new(-1, 0));
    }

    fn push_right(&mut self, tetris: &Tetris) {
        for p in &self.points {
            if p.x == 6 {
                return;
            }

            if tetris.grid[p.y as usize][(p.x + 1) as usize] == GridType::Rock {
                return;
            }
        }

        self.translate(Vec2::new(1, 0));
    }

    fn rock_fall(&mut self, tetris: &Tetris) -> bool {
        for p in &self.points {
            if p.y - 1 < 0 {
                return false;
            }

            if tetris.grid[(p.y - 1) as usize][p.x as usize] == GridType::Rock {
                return false;
            }
        }

        self.translate(Vec2::new(0, -1));
        return true;
    }
}

struct Tetris {
    grid: Vec<[GridType; 7]>,
    height: i64,
}

impl Tetris {
    fn new() -> Self {
        let tetris =  Self {
            grid: Vec::new(),
            height: 0,
        };
        return tetris;
    }

    fn spawn_new(&mut self, shape_type: ShapeType) -> Shape {
        let points: Vec<Vec2> = match shape_type {
            ShapeType::HorizontalLine   => Vec::from([Vec2::new(0, 0), Vec2::new(1, 0), Vec2::new(2, 0), Vec2::new(3, 0)]),
            ShapeType::Plus             => Vec::from([Vec2::new(0, 1), Vec2::new(1, 1), Vec2::new(2, 1), Vec2::new(1, 2), Vec2::new(1, 0)]),
            ShapeType::MirroredL        => Vec::from([Vec2::new(0, 0), Vec2::new(1, 0), Vec2::new(2, 0), Vec2::new(2, 1), Vec2::new(2, 2)]),
            ShapeType::VerticalLine     => Vec::from([Vec2::new(0, 0), Vec2::new(0, 1), Vec2::new(0, 2), Vec2::new(0, 3)]),
            ShapeType::Square           => Vec::from([Vec2::new(0, 0), Vec2::new(1, 0), Vec2::new(0, 1), Vec2::new(1, 1)]),
        };
        let mut shape = Shape { points: points };

        /* Add additional rows on top of the stack. */
        let additional_rows: i64 = self.find_peak() + 4 - self.grid.len() as i64 + match shape_type {
            ShapeType::HorizontalLine   => 1,
            ShapeType::Plus             => 3,
            ShapeType::MirroredL        => 3,
            ShapeType::VerticalLine     => 4,
            ShapeType::Square           => 2,
        };
        for _ in 0..additional_rows {
            let row: [GridType; 7] = [GridType::Empty; 7];
            self.grid.push(row);
        }

        /* Translate the shape to the correct position on top of the stack. */
        shape.translate(Vec2::new(2, self.find_peak() + 4));
        return shape;
    }

    fn finalize_rock(&mut self, shape: Shape) {
        let local_peak = self.find_peak();
        let mut y_max = local_peak;
        for point in shape.points {
            self.grid[point.y as usize][point.x as usize] = GridType::Rock;
            if point.y > y_max - 1 {
                y_max = point.y + 1;
            }
        }

        self.height += y_max - local_peak;
    }

    fn finger_print_top(&self) -> [i64; 7] {
        let local_peak = self.find_peak();
        let mut out: [i64; 7] = [self.height; 7];
        for i in 0..7 {
            for j in 0..self.height {
                if self.grid[(local_peak - j - 1) as usize][i] == GridType::Rock {
                    out[i] =  j;
                    break;
                }
            }
        }
        return out;
    }

    /* Find the peak of the tower in terms of the stack size. */
    fn find_peak(&self) -> i64 {
        for i in 0..self.grid.len() {
            for j in 0..7 {
                if self.grid[(self.grid.len() - i - 1) as usize][j] == GridType::Rock {
                    return (self.grid.len() - i) as i64;
                }
            }
        }
        return 0;
    }
}


fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let puzzle_input = raw_puzzle_input.trim();

    let gas_direction: Vec<char> = puzzle_input.chars().collect();
    let mut gas_index: usize = 0;
    let mut tetris = Tetris::new();
    let shape_order: [ShapeType; 5] = [ShapeType::HorizontalLine, ShapeType::Plus, ShapeType::MirroredL, ShapeType::VerticalLine, ShapeType::Square];
    let mut shape_index: usize = 0;
    
    /* Part 1 */
    for _ in 0..2022 {
        let mut shape = tetris.spawn_new(shape_order[shape_index]);
        shape_index = (shape_index + 1) % 5;

        while shape.rock_fall(&tetris) {

            let gas = gas_direction[gas_index];
            gas_index = (gas_index + 1) % gas_direction.len();
            if gas == '>' {
                shape.push_right(&tetris);
            }
            else {
                shape.push_left(&tetris);
            }
        }

        tetris.finalize_rock(shape);
    }
    let answer_part1 = tetris.height;
    println!("Day 17 part 1, result: {}", answer_part1);


    /* Part 2 - Cycle detection to speed up simulation. */
    let mut cycle_detection: HashMap<(usize, usize, [i64; 7]), (i64, i64)> = HashMap::new();
    let mut i = 2022;
    while i < 1000000000000 {

        /* Detect cycle. */
        let finger_print = tetris.finger_print_top();
        let key = (shape_index, gas_index, finger_print);
        if cycle_detection.contains_key(&key) {
            let (iteration, height) = cycle_detection.get(&key).unwrap();
            println!("Cycle detected on i: {i}, previous cycle on: {}, with previous height {} and current height {}", iteration, height, tetris.height);

            let delta_height = tetris.height - height;
            let delta_i = i - iteration;

            let cycles = (1000000000000 - i) / delta_i;
            tetris.height += cycles * delta_height;
            i += cycles * delta_i;
            cycle_detection.clear();
        }
        else {
            cycle_detection.insert(key, (i, tetris.height));
        }

        let mut shape = tetris.spawn_new(shape_order[shape_index]);
        shape_index = (shape_index + 1) % 5;

        while shape.rock_fall(&tetris) {

            let gas = gas_direction[gas_index];
            gas_index = (gas_index + 1) % gas_direction.len();
            if gas == '>' {
                shape.push_right(&tetris);
            }
            else {
                shape.push_left(&tetris);
            }
        }

        tetris.finalize_rock(shape);
        i += 1;
    }

    let answer_part2 = tetris.height;
    println!("Day 17 part 2, result: {}", answer_part2);
}