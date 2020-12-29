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
	round int
	deck1 []int
	deck2 []int
}

func NewGame(player1 string, player2 string) *Game {
	game := new(Game)
	game.round = 0

	for _, num := range strings.Split(player1, "\n")[1:] {
		value, _ := strconv.Atoi(num)
		game.deck1 = append(game.deck1, value)
	}

	for _, num := range strings.Split(player2, "\n")[1:] {
		value, _ := strconv.Atoi(num)
		game.deck2 = append(game.deck2, value)
	}

	return game
}

func countScore(deck []int) int {
	score := 0
	deckSize := len(deck)
	for i := 0; i < deckSize; i++ {
		score += (deck[i] * (deckSize - i))
	}
	return score
}

func (game *Game) Simulate() int {
	for len(game.deck1) > 0 && len(game.deck2) > 0 {
		card1 := game.deck1[0]
		card2 := game.deck2[0]
		game.deck1 = game.deck1[1:]
		game.deck2 = game.deck2[1:]

		if card1 > card2 {
			game.deck1 = append(game.deck1, card1)
			game.deck1 = append(game.deck1, card2)
		} else {
			game.deck2 = append(game.deck2, card2)
			game.deck2 = append(game.deck2, card1)
		}
		game.round++
	}

	if len(game.deck1) > 0 {
		return 0
	} else {
		return 1
	}
}

func hash(deck1 []int, deck2 []int) string {
	cards1 := make([]string, len(deck1))
	for i, value := range deck1 {
		cards1[i] = strconv.Itoa(value)
	}
	joinedDeck1 := strings.Join(cards1, ",")

	cards2 := make([]string, len(deck2))
	for i, value := range deck2 {
		cards2[i] = strconv.Itoa(value)
	}
	joinedDeck2 := strings.Join(cards2, ",")

	return joinedDeck1 + "|" + joinedDeck2
}

func (game *Game) SimulateRecursive() int {
	states := make(map[string]bool)

	for len(game.deck1) > 0 && len(game.deck2) > 0 {
		stateHash := hash(game.deck1, game.deck2)
		if _, ok := states[stateHash]; ok {
			return 0
		}
		states[stateHash] = true

		card1 := game.deck1[0]
		card2 := game.deck2[0]
		game.deck1 = game.deck1[1:]
		game.deck2 = game.deck2[1:]

		roundWinner := 0
		if len(game.deck1) >= card1 && len(game.deck2) >= card2 {
			// Play a recursive game to determine the winner.
			recursiveGame := Game{
				round: 0,
				deck1: make([]int, card1),
				deck2: make([]int, card2),
			}
			copy(recursiveGame.deck1, game.deck1[:card1])
			copy(recursiveGame.deck2, game.deck2[:card2])
			roundWinner = recursiveGame.SimulateRecursive()
		} else if card1 < card2 {
			roundWinner = 1
		}

		if roundWinner == 0 {
			game.deck1 = append(game.deck1, []int{card1, card2}...)
		} else {
			game.deck2 = append(game.deck2, []int{card2, card1}...)
		}
		game.round++
	}

	if len(game.deck1) > 0 {
		return 0
	} else {
		return 1
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
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n\n")

	// Part 1
	game1 := NewGame(data[0], data[1])
	winnerGame1 := game1.Simulate()
	part1 := 0
	if winnerGame1 == 0 {
		part1 = countScore(game1.deck1)
	} else {
		part1 = countScore(game1.deck2)
	}
	fmt.Printf("Day 22 part 1, answer: %d\n", part1)

	// Part 2
	game2 := NewGame(data[0], data[1])
	winnerGame2 := game2.SimulateRecursive()
	part2 := 0
	if winnerGame2 == 0 {
		part2 = countScore(game2.deck1)
	} else {
		part2 = countScore(game2.deck2)
	}
	fmt.Printf("Day 22 part 2, answer: %d\n", part2)
}
