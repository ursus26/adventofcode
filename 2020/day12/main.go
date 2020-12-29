package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

const (
	NORTH = iota
	EAST  = iota
	SOUTH = iota
	WEST  = iota
)

type Ship struct {
	x         int
	y         int
	direction int
}

type Vec2 struct {
	x int
	y int
}

type ShipV2 struct {
	position Vec2
	waypoint Vec2
}

func AbsInt(a int) int {
	if a >= 0 {
		return a
	}
	return -1 * a
}

func (ship *Ship) Forward(steps int) {
	switch ship.direction {
	case NORTH:
		ship.y += steps
	case EAST:
		ship.x += steps
	case SOUTH:
		ship.y -= steps
	case WEST:
		ship.x -= steps
	}
}

func (ship *Ship) RotateRight(degrees int) {
	if degrees == 0 {
		return
	}

	ship.direction = (ship.direction + 1) % 4
	if degrees > 90 {
		ship.RotateRight(degrees - 90)
	}
}

func (ship *Ship) RotateLeft(degrees int) {
	if degrees == 0 {
		return
	}

	ship.direction = (ship.direction + 3) % 4
	if degrees > 90 {
		ship.RotateLeft(degrees - 90)
	}
}

func (ship *ShipV2) RotateRight(degrees int) {
	if degrees == 0 {
		return
	}

	ship.waypoint.x, ship.waypoint.y = ship.waypoint.y, -ship.waypoint.x
	ship.RotateRight(degrees - 90)
}

func (ship *ShipV2) RotateLeft(degrees int) {
	if degrees == 0 {
		return
	}

	ship.waypoint.x, ship.waypoint.y = -ship.waypoint.y, ship.waypoint.x
	ship.RotateLeft(degrees - 90)
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

	ship := Ship{0, 0, EAST}
	for _, line := range data {
		op := line[0]
		param, _ := strconv.Atoi(line[1:])

		switch op {
		case 'N':
			ship.y += param
		case 'E':
			ship.x += param
		case 'S':
			ship.y -= param
		case 'W':
			ship.x -= param
		case 'F':
			ship.Forward(param)
		case 'L':
			ship.RotateLeft(param)
		case 'R':
			ship.RotateRight(param)
		default:
			fmt.Printf("Error, unknown or unimplemented operation: '%c %d'\n", op, param)
		}
	}
	part1 := AbsInt(ship.x) + AbsInt(ship.y)
	fmt.Printf("Day 10 part 1, answer: %d\n", part1)

	/* Part 2. */
	shipPart2 := ShipV2{Vec2{0, 0}, Vec2{10, 1}}
	for _, line := range data {
		op := line[0]
		param, _ := strconv.Atoi(line[1:])

		switch op {
		case 'N':
			shipPart2.waypoint.y += param
		case 'E':
			shipPart2.waypoint.x += param
		case 'S':
			shipPart2.waypoint.y -= param
		case 'W':
			shipPart2.waypoint.x -= param
		case 'F':
			shipPart2.position.x += param * shipPart2.waypoint.x
			shipPart2.position.y += param * shipPart2.waypoint.y
		case 'L':
			shipPart2.RotateLeft(param)
		case 'R':
			shipPart2.RotateRight(param)
		default:
			fmt.Printf("Error, unknown or unimplemented operation: '%c %d'\n", op, param)
		}
	}

	part2 := AbsInt(shipPart2.position.x) + AbsInt(shipPart2.position.y)
	fmt.Printf("Day 10 part 2, answer: %d\n", part2)
}
