package main

import (
	"fmt"
	"log"
	"os"
	"strings"
)

func neighbourCount(grid [][]byte, x int, y int) int {
	neighbours := 0
	leftBoundary := x == 0
	rightBoundary := x == len(grid[0])-1

	if y > 0 {
		if !leftBoundary && grid[y-1][x-1] == '@' {
			neighbours++
		}
		if grid[y-1][x] == '@' {
			neighbours++
		}
		if !rightBoundary && grid[y-1][x+1] == '@' {
			neighbours++
		}
	}

	if !leftBoundary && grid[y][x-1] == '@' {
		neighbours++
	}
	if !rightBoundary && grid[y][x+1] == '@' {
		neighbours++
	}

	if y < len(grid)-1 {
		if !leftBoundary && grid[y+1][x-1] == '@' {
			neighbours++
		}
		if grid[y+1][x] == '@' {
			neighbours++
		}
		if !rightBoundary && grid[y+1][x+1] == '@' {
			neighbours++
		}
	}

	return neighbours
}

func makeGrid(input string) [][]byte {
	rows := strings.Split(input, "\n")
	height := len(rows)
	width := len(rows[0])

	grid := make([][]byte, height)
	for i := range height {
		grid[i] = make([]byte, width)
		for j := range width {
			grid[i][j] = rows[i][j]
		}
	}
	return grid
}

func main() {
	data, err := os.ReadFile("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	text := string(data)

	var answerPart1 int = 0
	var answerPart2 int64 = 0

	/* Part 1 */
	grid := makeGrid(text)
	for y := range grid {
		row := grid[y]
		for x := range row {
			if row[x] == '@' {
				if neighbourCount(grid, x, y) < 4 {
					answerPart1++
				}
			}
		}
	}

	/* Part 2 */
	for {
		gridChanged := false
		for y := range grid {
			for x := range grid[y] {
				if grid[y][x] == '@' {
					if neighbourCount(grid, x, y) < 4 {
						answerPart2++
						grid[y][x] = '.'
						gridChanged = true
					}
				}
			}
		}

		if !gridChanged {
			break
		}
	}

	fmt.Printf("Day 04 part 1, result: %d\n", answerPart1)
	fmt.Printf("Day 04 part 2, result: %d\n", answerPart2)
}
