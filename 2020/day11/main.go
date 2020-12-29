package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strings"
)

func checkSeat(seatOccupancy [][]int, i int, j int) int {
	if i < 0 || j < 0 || i >= len(seatOccupancy) || j >= len(seatOccupancy[0]) {
		return 0
	}
	return seatOccupancy[i][j]
}

func adjacentSeatCount(seatOccupancy [][]int, i int, j int) int {
	seatCount := checkSeat(seatOccupancy, i-1, j-1) + checkSeat(seatOccupancy, i-1, j) + checkSeat(seatOccupancy, i-1, j+1)
	seatCount += checkSeat(seatOccupancy, i, j-1) + checkSeat(seatOccupancy, i, j+1)
	seatCount += checkSeat(seatOccupancy, i+1, j-1) + checkSeat(seatOccupancy, i+1, j) + checkSeat(seatOccupancy, i+1, j+1)
	return seatCount
}

func checkSeatInVision(seatOccupancy [][]int, seatLocations [][]bool, y int, x int, dy int, dx int) int {
	if x < 0 || y < 0 || y >= len(seatOccupancy) || x >= len(seatOccupancy[0]) {
		return 0
	}

	if seatLocations[y][x] == true {
		return seatOccupancy[y][x]
	} else {
		return checkSeatInVision(seatOccupancy, seatLocations, y+dy, x+dx, dy, dx)
	}
}

func visibleSeatCount(seatOccupancy [][]int, seatLocations [][]bool, i int, j int) int {
	seatCount := checkSeatInVision(seatOccupancy, seatLocations, i-1, j-1, -1, -1)
	seatCount += checkSeatInVision(seatOccupancy, seatLocations, i-1, j, -1, 0)
	seatCount += checkSeatInVision(seatOccupancy, seatLocations, i-1, j+1, -1, 1)
	seatCount += checkSeatInVision(seatOccupancy, seatLocations, i, j-1, 0, -1)
	seatCount += checkSeatInVision(seatOccupancy, seatLocations, i, j+1, 0, 1)
	seatCount += checkSeatInVision(seatOccupancy, seatLocations, i+1, j-1, 1, -1)
	seatCount += checkSeatInVision(seatOccupancy, seatLocations, i+1, j, 1, 0)
	seatCount += checkSeatInVision(seatOccupancy, seatLocations, i+1, j+1, 1, 1)
	return seatCount
}

func applyEmptyRule(seatOccupancy [][]int, i int, j int) int {
	if adjacentSeatCount(seatOccupancy, i, j) == 0 {
		return 1
	} else {
		return 0
	}
}

func applyEmptyRuleV2(seatOccupancy [][]int, seatLocations [][]bool, i int, j int) int {
	if visibleSeatCount(seatOccupancy, seatLocations, i, j) == 0 {
		return 1
	} else {
		return 0
	}
}

func applyOccupiedRule(seatOccupancy [][]int, i int, j int) int {
	if adjacentSeatCount(seatOccupancy, i, j) >= 4 {
		return 0
	} else {
		return 1
	}
}

func applyOccupiedRuleV2(seatOccupancy [][]int, seatLocations [][]bool, i int, j int) int {
	if visibleSeatCount(seatOccupancy, seatLocations, i, j) >= 5 {
		return 0
	} else {
		return 1
	}
}

func seatCount(seatOccupancy [][]int) int {
	count := 0
	for _, row := range seatOccupancy {
		for _, val := range row {
			count += val
		}
	}
	return count
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

	seatLocations := make([][]bool, len(data))
	seatOccupancy := make([][]int, len(data))
	seatOccupancy2 := make([][]int, len(data))
	current := &seatOccupancy
	next := &seatOccupancy2
	for i, row := range data {
		seatLocations[i] = make([]bool, len(row))
		seatOccupancy[i] = make([]int, len(row))
		seatOccupancy2[i] = make([]int, len(row))
		for j, character := range row {
			switch character {
			case '.':
				seatLocations[i][j] = false
				seatOccupancy[i][j] = 0
			case 'L':
				seatLocations[i][j] = true
				seatOccupancy[i][j] = 0
			case '#':
				seatLocations[i][j] = true
				seatOccupancy[i][j] = 1
			default:
				fmt.Printf("Error, invalid character '%c' on line %d, column %d\n", character, i+1, j+1)
				os.Exit(1)
			}
		}
	}

	for true {
		seatsStabalized := true
		for i, row := range seatLocations {
			for j, isSeat := range row {
				if !isSeat {
					continue
				}

				/* Apply the seating rules for the next iteration. */
				if (*current)[i][j] == 0 {
					(*next)[i][j] = applyEmptyRule(*current, i, j)
				} else {
					(*next)[i][j] = applyOccupiedRule(*current, i, j)
				}

				/* Check if seats occupancy has changed. */
				if (*current)[i][j] != (*next)[i][j] {
					seatsStabalized = false
				}
			}
		}

		/* Swap buffers if system has not stabalized. */
		if seatsStabalized {
			break
		}
		current, next = next, current
	}

	part1 := seatCount(*next)
	fmt.Printf("Day 11 part 1, answer: %d\n", part1)

	/* Part 2 */
	current = &seatOccupancy
	next = &seatOccupancy2
	for i, row := range data {
		for j, character := range row {
			switch character {
			case '.':
				seatOccupancy[i][j] = 0
			case 'L':
				seatOccupancy[i][j] = 0
			case '#':
				seatOccupancy[i][j] = 1
			default:
				fmt.Printf("Error, invalid character '%c' on line %d, column %d\n", character, i+1, j+1)
				os.Exit(1)
			}
		}
	}

	for true {
		seatsStabalized := true
		for i, row := range seatLocations {
			for j, isSeat := range row {
				if !isSeat {
					continue
				}

				/* Apply the seating rules for the next iteration. */
				if (*current)[i][j] == 0 {
					(*next)[i][j] = applyEmptyRuleV2(*current, seatLocations, i, j)
				} else {
					(*next)[i][j] = applyOccupiedRuleV2(*current, seatLocations, i, j)
				}

				/* Check if seats occupancy has changed. */
				if (*current)[i][j] != (*next)[i][j] {
					seatsStabalized = false
				}
			}
		}

		/* Swap buffers if system has not stabalized. */
		if seatsStabalized {
			break
		}
		current, next = next, current
	}
	part2 := seatCount(*next)
	fmt.Printf("Day 10 part 2, answer: %d\n", part2)
}
