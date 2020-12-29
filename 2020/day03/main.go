package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strings"
)

type TreeMap struct {
	locations [][]int
	width     int
	height    int
}

func NewTreeMap(file *os.File) *TreeMap {
	bytes, _ := ioutil.ReadAll(file)
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n")

	out := new(TreeMap)
	out.height = len(data)
	out.width = len(data[0])
	out.locations = make([][]int, out.height)
	for i, line := range data {
		out.locations[i] = make([]int, out.width)
		for j, character := range line {
			switch character {
			case '.':
				out.locations[i][j] = 0
			case '#':
				out.locations[i][j] = 1
			default:
				fmt.Printf("Error, invalid character '%c' on line: %d, column: %d\n", character, i+1, j+1)
			}
		}
	}
	return out
}

func (treeMap TreeMap) TraverseSlope(dx int, dy int) int {
	x, y, treesEncountered := 0, 0, 0
	for y < treeMap.height {
		if treeMap.locations[y][x] == 1 {
			treesEncountered += 1
		}

		/* Update to next location. */
		x = (x + dx) % treeMap.width
		y += dy
	}
	return treesEncountered
}

func main() {
	var file, err = os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	treeMap := NewTreeMap(file)

	/* Part 1. */
	treesEncountered := treeMap.TraverseSlope(3, 1)
	fmt.Printf("Day 3 part 1, result: %d\n", treesEncountered)

	/* Part 2. */
	part2 := treesEncountered
	part2 *= treeMap.TraverseSlope(1, 1)
	part2 *= treeMap.TraverseSlope(5, 1)
	part2 *= treeMap.TraverseSlope(7, 1)
	part2 *= treeMap.TraverseSlope(1, 2)
	fmt.Printf("Day 3 part 2, result: %d\n", part2)
}
