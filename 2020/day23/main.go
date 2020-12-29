package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

type Cups struct {
	currentCup int
	cupCount   int
	cups       []int
}

func (c *Cups) Move() {
	// Pick up 3 cups.
	slice := []int{0, 0, 0}
	slice[0] = c.cups[c.currentCup]
	for i := 1; i < 3; i++ {
		slice[i] = c.cups[slice[i-1]]
	}

	// Find the destination.
	destination := (c.currentCup - 1 + c.cupCount) % c.cupCount
	for {
		if slice[0] == destination || slice[1] == destination || slice[2] == destination {
			destination = (destination - 1 + c.cupCount) % c.cupCount
		} else {
			break
		}
	}

	// Perform the move operation of the cups.
	c.cups[c.currentCup] = c.cups[slice[2]]
	c.cups[slice[2]] = c.cups[destination]
	c.cups[destination] = slice[0]
	c.currentCup = c.cups[c.currentCup]
}

func (c Cups) Result() string {
	out := strconv.Itoa(c.cups[0] + 1)
	for i := c.cups[0]; c.cups[i] != 0; i = c.cups[i] {
		out += strconv.Itoa(c.cups[i] + 1)
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
	data := strings.Trim(string(bytes), "\n")

	/* Initial setup. */
	max := 0
	iv := make([]int, len(data))
	for i, character := range data {
		val := int(character-'0') - 1
		iv[i] = val

		if val > max {
			max = val
		}
	}

	// Part 1
	c := Cups{
		currentCup: iv[0],
		cupCount:   len(data),
		cups:       make([]int, len(data)),
	}
	for i, val := range iv[:len(iv)-1] {
		c.cups[val] = iv[i+1]
	}
	c.cups[iv[len(iv)-1]] = iv[0]

	for i := 0; i < 100; i++ {
		c.Move()
	}
	part1 := c.Result()
	fmt.Printf("Day 23 part 1, answer: %s\n", part1)

	// Part 2
	cupCount := 1000000
	iterations := 10000000

	// Setting up the game.
	c2 := Cups{
		currentCup: iv[0],
		cupCount:   cupCount,
		cups:       make([]int, cupCount),
	}
	for i, val := range iv[:len(iv)-1] {
		c2.cups[val] = iv[i+1]
	}
	c2.cups[iv[len(iv)-1]] = max + 1
	for i := max + 1; i < cupCount; i++ {
		c2.cups[i] = i + 1
	}
	c2.cups[c2.cupCount-1] = iv[0]

	// Move the cups and calculate the result.
	for i := 0; i < iterations; i++ {
		c2.Move()
	}
	val1 := c2.cups[0]
	val2 := c2.cups[val1]
	part2 := (val1 + 1) * (val2 + 1)
	fmt.Printf("Day 23 part 2, answer: %d\n", part2)
}
