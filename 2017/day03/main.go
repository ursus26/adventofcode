package main

import (
	"fmt"
)

type Coord struct {
	x int
	y int
}

func getRing(input int) int {
	if input <= 0 {
		return -1
	}

	var ring int = 0
	for true {
		/* The maximum value of the ring = width * width = (2 * ring + 1)^2 */
		maxRingValue := (2*ring + 1) * (2*ring + 1)
		if input <= maxRingValue {
			break
		}
		ring += 1
	}
	return ring
}

func rotate(input int, ring int) int {
	if input <= 0 {
		return -1
	}

	prevRingWidth := 2*(ring-1) + 1
	minRingValue := prevRingWidth*prevRingWidth + 1
	rotateValue := 2 * ring

	for true {
		if input-rotateValue < minRingValue {
			break
		}

		input -= rotateValue
	}

	return input
}

func main() {
	input := 361527

	/* Part 1 */
	ring := getRing(input)
	rotatedInput := rotate(input, ring) /* Rotate around the spiral but within the ring to find a value on the right side of the square. */

	prevRingWidth := 2*(ring-1) + 1
	minDistanceValue := prevRingWidth*prevRingWidth + ring /* If you go ring distance to the right of the center square, then you will find this value. */

	verticalDistance := rotatedInput - minDistanceValue
	if verticalDistance < 0 {
		verticalDistance *= -1
	}

	distance := verticalDistance + ring
	fmt.Printf("Day 3 part 1, answer: %d\n", distance)

	/* Part 2 */
	spiralMem := make(map[Coord]int)
	spiralMem[Coord{0, 0}] = 1
	var x, y int = 0, 0
	var dx, dy int = 1, 0
	index := 1
	prevRing := 0

	for true {
		x += dx
		y += dy
		index += 1
		ring := getRing(index)

		value := spiralMem[Coord{x + 1, y}] + spiralMem[Coord{x + 1, y + 1}] + spiralMem[Coord{x, y + 1}] +
			spiralMem[Coord{x - 1, y + 1}] + spiralMem[Coord{x - 1, y}] + spiralMem[Coord{x - 1, y - 1}] +
			spiralMem[Coord{x, y - 1}] + spiralMem[Coord{x + 1, y - 1}]

		spiralMem[Coord{x, y}] = value

		if value > input {
			break
		}

		/* Check if we need to turn our direction. */
		if (x == ring && y == ring) || (x == -ring && y == ring) || (x == -ring && y == -ring) || (prevRing != ring) {
			tmp := dy
			dy = dx
			dx = -tmp
		}
		prevRing = ring
	}

	fmt.Printf("Day 3 part 2, answer: %d\n", spiralMem[Coord{x, y}])
}
