package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"regexp"
	"strings"
)

type Coord struct {
	x int
	y int
}

func AbsInt(x int) int {
	if x < 0 {
		return -1 * x
	} else {
		return x
	}
}

func (c *Coord) Move(dir string) {
	switch dir {
	case "nw":
		c.y++
		if AbsInt(c.y)%2 == 1 {
			c.x -= 1
		}
	case "ne":
		c.y++
		if AbsInt(c.y)%2 == 0 {
			c.x += 1
		}
	case "sw":
		c.y--
		if AbsInt(c.y)%2 == 1 {
			c.x -= 1
		}
	case "se":
		c.y--
		if AbsInt(c.y)%2 == 0 {
			c.x += 1
		}
	case "e":
		c.x++
	case "w":
		c.x--
	}
}

func (c Coord) Neighbours() []Coord {
	out := make([]Coord, 6)
	out[0] = Coord{c.x + 1, c.y} // east
	out[1] = Coord{c.x - 1, c.y} // west
	out[2] = Coord{c.x, c.y - 1}
	out[3] = Coord{c.x, c.y + 1}

	if c.y%2 == 0 {
		out[4] = Coord{c.x - 1, c.y - 1}
		out[5] = Coord{c.x - 1, c.y + 1}
	} else {
		out[4] = Coord{c.x + 1, c.y - 1}
		out[5] = Coord{c.x + 1, c.y + 1}
	}
	return out
}

func main() {
	var file, err = os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	/* Data parsing. */
	bytes, err := ioutil.ReadAll(file)
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n")

	tiles := make(map[Coord]bool)
	re := regexp.MustCompile(`(e|w|ne|nw|se|sw)`)

	// Part 1
	for _, line := range data {
		coord := Coord{0, 0}
		moveActions := re.FindAllString(line, -1)
		for _, action := range moveActions {
			coord.Move(action)
		}

		if _, ok := tiles[coord]; !ok {
			tiles[coord] = true
		} else {
			tiles[coord] = !tiles[coord]
		}
	}

	part1 := 0
	for k, v := range tiles {
		if v {
			part1++
		} else {
			delete(tiles, k)
		}
	}
	fmt.Printf("Day 24 part 1, answer: %d\n", part1)

	// Part 2
	for i := 0; i < 100; i++ {
		// Build a set of tiles to check for the next interation.
		gridNext := make(map[Coord]bool)
		for k, _ := range tiles {
			gridNext[k] = false
			for _, neighbour := range k.Neighbours() {
				if _, ok := gridNext[neighbour]; !ok {
					gridNext[neighbour] = false
				}
			}
		}

		// Check each tile.
		for k, _ := range gridNext {
			tileSet, ok := tiles[k]
			if !ok {
				tileSet = false
			}

			// Count the number of black neighbouring tiles.
			blackCount := 0
			for _, neighbour := range k.Neighbours() {
				neighbourSet, ok2 := tiles[neighbour]
				if !ok2 {
					continue
				}
				if neighbourSet {
					blackCount++
				}
			}

			// Check the rules.
			if tileSet && (blackCount == 0 || blackCount > 2) {
				gridNext[k] = false
			} else if !tileSet && blackCount == 2 {
				gridNext[k] = true
			} else {
				gridNext[k] = tileSet
			}
		}

		// Remove white tiles and prepare the grid for the next iteration.
		for k, v := range gridNext {
			if !v {
				delete(gridNext, k)
			}
		}
		tiles = gridNext
	}

	part2 := 0
	for _, v := range tiles {
		if v {
			part2++
		}
	}
	fmt.Printf("Day 24 part 2, answer: %d\n", part2)
}
