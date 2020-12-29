package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strings"
)

type Vec4 struct {
	x int
	y int
	z int
	w int
}

type Game struct {
	cube     map[Vec4]int
	cycle    int
	maxPoint Vec4
	minPoint Vec4
}

func NewGame(initialSlice []string) Game {
	game := Game{
		cube:     make(map[Vec4]int),
		cycle:    0,
		maxPoint: Vec4{len(initialSlice) + 1, len(initialSlice[0]) + 1, 1, 1},
		minPoint: Vec4{-1, -1, -1, -1},
	}

	for i, line := range initialSlice {
		for j, character := range line {
			switch character {
			case '#':
				game.cube[Vec4{i, j, 0, 0}] = 1
			case '.':
				game.cube[Vec4{i, j, 0, 0}] = 0
			default:
				continue
			}
		}
	}
	return game
}

func (game *Game) Cycle(part1 bool) {
	nextCube := make(map[Vec4]int)
	for x := game.minPoint.x; x <= game.maxPoint.x; x++ {
		for y := game.minPoint.y; y <= game.maxPoint.y; y++ {
			for z := game.minPoint.z; z <= game.maxPoint.z; z++ {
				for w := game.minPoint.w; w <= game.maxPoint.w; w++ {
					coord := Vec4{x, y, z, w}
					nextValue := game.nextCellState(coord, part1)
					nextCube[coord] = nextValue
				}
			}
		}
	}

	if part1 {
		game.maxPoint = Vec4{game.maxPoint.x + 1, game.maxPoint.y + 1, game.maxPoint.z + 1, 0}
		game.minPoint = Vec4{game.minPoint.x - 1, game.minPoint.y - 1, game.minPoint.z - 1, 0}
	} else {
		game.maxPoint = Vec4{game.maxPoint.x + 1, game.maxPoint.y + 1, game.maxPoint.z + 1, game.maxPoint.w + 1}
		game.minPoint = Vec4{game.minPoint.x - 1, game.minPoint.y - 1, game.minPoint.z - 1, game.minPoint.w - 1}
	}
	game.cube = nextCube
}

func (game Game) nextCellState(coord Vec4, part1 bool) int {
	cellValue := game.CellValue(coord)
	activeNeighbours := -cellValue
	for x := coord.x - 1; x <= coord.x+1; x++ {
		for y := coord.y - 1; y <= coord.y+1; y++ {
			for z := coord.z - 1; z <= coord.z+1; z++ {
				if part1 {
					activeNeighbours += game.CellValue(Vec4{x, y, z, 0})
				} else {
					for w := coord.w - 1; w <= coord.w+1; w++ {
						activeNeighbours += game.CellValue(Vec4{x, y, z, w})
					}
				}
			}
		}
	}

	if cellValue == 1 {
		if activeNeighbours == 2 || activeNeighbours == 3 {
			return 1
		} else {
			return 0
		}
	} else {
		if activeNeighbours == 3 {
			return 1
		} else {
			return 0
		}
	}
}

func (game Game) CellValue(coord Vec4) int {
	val, ok := game.cube[coord]
	if !ok {
		return 0
	}
	return val
}

func (game Game) ActiveCells() int {
	activeCells := 0
	for x := game.minPoint.x; x <= game.maxPoint.x; x++ {
		for y := game.minPoint.y; y <= game.maxPoint.y; y++ {
			for z := game.minPoint.z; z <= game.maxPoint.z; z++ {
				for w := game.minPoint.w; w <= game.maxPoint.w; w++ {
					activeCells += game.CellValue(Vec4{x, y, z, w})
				}
			}
		}
	}
	return activeCells
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

	game1 := NewGame(data)
	for i := 0; i < 6; i++ {
		game1.Cycle(true)
	}
	part1 := game1.ActiveCells()

	game2 := NewGame(data)
	for i := 0; i < 6; i++ {
		game2.Cycle(false)
	}
	part2 := game2.ActiveCells()

	fmt.Printf("Day 17 part 1, answer: %d\n", part1)
	fmt.Printf("Day 17 part 2, answer: %d\n", part2)
}
