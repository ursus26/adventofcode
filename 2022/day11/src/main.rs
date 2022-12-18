use std::collections::VecDeque;

// #[derive(Clone)]
#[allow(dead_code)]
struct Monkey
{
    id: i32,
    items: VecDeque<u128>,
    inspection_count: u128,
    operation: Box<dyn Fn(u128) -> u128>,
    throw_to: Box<dyn Fn(u128) -> usize>,
}

fn part1(mut monkeys: Vec<Monkey>) -> u128 {
    for _ in 0..20 {
        for i in 0..monkeys.len() {
            while !monkeys[i].items.is_empty() {
                let item = monkeys[i].items.pop_front().unwrap();
                monkeys[i].inspection_count += 1;
                let new_item_value = (*monkeys[i].operation)(item) / 3;
                let throw_to_id = (*monkeys[i].throw_to)(new_item_value);
                monkeys[throw_to_id].items.push_back(new_item_value);
            }
        }
    }

    let mut counts = Vec::new();
    for i in 0..monkeys.len() {
        counts.push(monkeys[i].inspection_count);
    }
    counts.sort_by(|a, b| b.cmp(a));
    return counts[0] * counts[1];
}

fn part2(mut monkeys: Vec<Monkey>) -> u128 {

    let lcm = 2 * 3 * 5 * 7 * 11 * 13 * 17 * 19;

    for _ in 0..10000 {
        for i in 0..monkeys.len() {
            while !monkeys[i].items.is_empty() {
                let item = monkeys[i].items.pop_front().unwrap();
                monkeys[i].inspection_count += 1;
                let new_item_value = (*monkeys[i].operation)(item) % lcm;
                let throw_to_id = (*monkeys[i].throw_to)(new_item_value);
                monkeys[throw_to_id].items.push_back(new_item_value);
            }
        }
    }

    let mut counts = Vec::new();
    for i in 0..monkeys.len() {
        counts.push(monkeys[i].inspection_count);
    }
    counts.sort_by(|a, b| b.cmp(a));
    return counts[0] * counts[1];
}

fn get_monkeys() -> Vec<Monkey> {
    let monkey0 = Monkey {
        id: 0,
        items: VecDeque::from([75, 63]),
        inspection_count: 0,
        operation: Box::new(|val| { 3 * val }),
        throw_to: Box::new(|x| { if (x % 11) == 0 { 7 } else { 2 } })
    };
    let monkey1 = Monkey {
        id: 1,
        items: VecDeque::from([65, 79, 98, 77, 56, 54, 83, 94]),
        inspection_count: 0,
        operation: Box::new(|val| { val + 3 }),
        throw_to: Box::new(|val| { if (val % 2) == 0 { 2 } else { 0 } })
    };
    let monkey2 = Monkey {
        id: 2,
        items: VecDeque::from([66]),
        inspection_count: 0,
        operation: Box::new(|val| { val + 5 }),
        throw_to: Box::new(|val| { if (val % 5) == 0 { 7 } else { 5 } })
    };
    let monkey3 = Monkey {
        id: 3,
        items: VecDeque::from([51, 89, 90]),
        inspection_count: 0,
        operation: Box::new(|val| { 19 * val }),
        throw_to: Box::new(|val| { if (val % 7) == 0 { 6 } else { 4 } })
    };
    let monkey4 = Monkey {
        id: 4,
        items: VecDeque::from([75, 94, 66, 90, 77, 82, 61]),
        inspection_count: 0,
        operation: Box::new(|val| { val + 1 }),
        throw_to: Box::new(|val| { if (val % 17) == 0 { 6 } else { 1 } })
    };
    let monkey5 = Monkey {
        id: 5,
        items: VecDeque::from([53, 76, 59, 92, 95]),
        inspection_count: 0,
        operation: Box::new(|val| { val + 2 }),
        throw_to: Box::new(|val| { if (val % 19) == 0 { 4 } else { 3 } })
    };
    let monkey6 = Monkey {
        id: 6,
        items: VecDeque::from([81, 61, 75, 89, 70, 92]),
        inspection_count: 0,
        operation: Box::new(|val| { val * val }),
        throw_to: Box::new(|val| { if (val % 3) == 0 { 0 } else { 1 } })
    };
    let monkey7 = Monkey {
        id: 7,
        items: VecDeque::from([81, 86, 62, 87]),
        inspection_count: 0,
        operation: Box::new(|val| { val + 8 }),
        throw_to: Box::new(|val| { if (val % 13) == 0 { 3 } else { 5 } })
    };

    return vec![monkey0, monkey1, monkey2, monkey3, monkey4, monkey5, monkey6, monkey7];
}

fn main() {
    let monkeys_part1 = get_monkeys();
    let answer_part1 = part1(monkeys_part1);
    println!("Day 11 part 1, result: {}", answer_part1);

    let monkeys_part2 = get_monkeys();
    let answer_part2 = part2(monkeys_part2);
    println!("Day 11 part 2, result: {}", answer_part2);
}
