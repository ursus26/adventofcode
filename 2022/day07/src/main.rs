use std::fs;
use std::collections::HashMap;

struct File {
    _name: String,
    size: u64
}

impl File {
    fn new(n: String, s: u64) -> Self {
        return Self {
            _name: n,
            size: s,
        };
    }
}

struct Directory {
    _name: String,
    parent: usize,
    children: Vec<usize>,
    files: Vec<File>
}

impl Directory {
    fn new(n: String, parent: usize) -> Self {
        return Self {
            _name: n,
            parent: parent,
            children: Vec::new(),
            files: Vec::new(),
        };
    }

    fn add_file(&mut self, f: File) {
        self.files.push(f);
    }

    fn add_dir(&mut self, child: usize) {
        self.children.push(child);
    }

    fn get_direct_file_size(&self) -> u64 {
        let mut size = 0;
        for f in &self.files {
            size += f.size;
        }
        return size;
    }
}


fn main() {
    /* Get puzzle input from file. */
    let puzzle_input_file_name = "input.txt";
    let raw_puzzle_input =
        fs::read_to_string(puzzle_input_file_name).expect("Should have been able to read the file");
    let mut puzzle_input: Vec<&str> = raw_puzzle_input.trim()
                                                  .split("$ ")
                                                  .skip(1)
                                                  .collect();

    for i in 0..puzzle_input.len() {
        puzzle_input[i] = puzzle_input[i].trim();
    }

    let mut directories: Vec<Directory> = Vec::new();
    let mut map: HashMap<String, usize> = HashMap::new();

    let root = Directory::new(String::from("/"), 0);
    directories.push(root);
    map.insert(String::from("/"), 0);

    /* Create file directory tree */
    let mut idx: usize = 0;
    for prompt in puzzle_input {
        let lines = prompt.split("\n").collect::<Vec<&str>>();
        let args: Vec<&str> = lines[0].split(" ").collect();

        if args[0] == "cd" {
            if args[1] == "/" {
                idx = 0;
            }
            else if args[1] == ".." {
                let dir = &mut directories[idx];
                idx = dir.parent;
            }
            else {
                let child_idx = directories.len();
                let parent_idx = idx;

                let dir = &mut directories[idx];
                dir.add_dir(child_idx);

                let child_name = String::from(args[1]);
                let new_dir = Directory::new(child_name, parent_idx);
                directories.push(new_dir);
                map.insert(String::from(args[1]), directories.len() - 1);

                idx = directories.len() - 1;
            }
        }
        else if args[0] == "ls" {
            let dir = &mut directories[idx];
            for ls_result in lines.iter().skip(1) {
                let words: Vec<&str> = ls_result.split_whitespace().collect();
                if words[0] == "dir" {
                }
                else {
                    let file_name = String::from(words[1]);
                    let file_size: u64 = words[0].parse().unwrap();
                    let new_file = File::new(file_name, file_size);
                    dir.add_file(new_file);
                }
            }
        }
    }

    let mut stack: Vec<usize> = Vec::new();
    stack.push(0);
    let mut directory_sizes: HashMap<usize, u64> = HashMap::new();

    while let Some(dir_idx) = stack.pop() {
        let dir = &directories[dir_idx];
        let mut children_size = 0;
        let mut all_children_sizes_are_known = true;
        for child in &(dir.children) {
            if !directory_sizes.contains_key(child) {
                stack.push(dir_idx);
                stack.push(*child);
                all_children_sizes_are_known = false;
                break;
            }

            children_size += directory_sizes.get(child).unwrap();
        }

        if !all_children_sizes_are_known {
            continue;
        }

        let size = dir.get_direct_file_size() + children_size;
        directory_sizes.insert(dir_idx, size);
    }

    let mut answer_part1: u64 = 0;
    for (_, val) in directory_sizes.iter() {
        if *val <= 100000 {
            answer_part1 += val;
        }
    }
    println!("Day 07 part 1, result: {}", answer_part1);

    let max_size: u64 = 70000000;
    let update_size: u64 = 30000000;
    let used_size: u64 = directory_sizes[&0];
    let unused_size: u64 = max_size - used_size;
    let size_needed = update_size - unused_size;

    let mut answer_part2: u64 = used_size;
    for (_, val) in directory_sizes.iter() {
        if *val >= size_needed && *val < answer_part2 {
            answer_part2 = *val;
        }
    }

    println!("Day 07 part 2, result: {}", answer_part2);
}