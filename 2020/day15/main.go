package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

type Game struct {
	memory     []int
	turn       int
	mostRecent int
}

func NewGame(iv []int, maxGameSize int) Game {
	game := Game{
		memory:     make([]int, maxGameSize),
		turn:       0,
		mostRecent: 0,
	}

	for _, value := range iv {
		game.turn++
		game.memory[value] = game.turn
		game.mostRecent = value
	}
	return game
}

func (game *Game) Loop(stopTurn int) {
	spokenNumber := 0
	for ; game.turn < stopTurn; game.turn++ {
		lastSpoken := game.memory[game.mostRecent]
		if lastSpoken > 0 {
			spokenNumber = game.turn - lastSpoken
		} else {
			spokenNumber = 0
		}

		game.memory[game.mostRecent] = game.turn
		game.mostRecent = spokenNumber
	}
}

func main() {
	var file, err = os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	/* Data parsing. */
	bytes, err := ioutil.ReadAll(file)
	data := strings.Split(strings.Trim(string(bytes), "\n"), ",")
	iv := make([]int, len(data))
	for i, num := range data {
		value, _ := strconv.Atoi(num)
		iv[i] = value
	}
	stopTurnPart1 := 2020
	stopTurnPart2 := 30000000
	game := NewGame(iv, stopTurnPart2)

	game.Loop(stopTurnPart1)
	fmt.Printf("Day 15 part 1, answer: %d\n", game.mostRecent)

	game.Loop(stopTurnPart2)
	fmt.Printf("Day 15 part 2, answer: %d\n", game.mostRecent)
}
