package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
)

func charToInt(input byte) uint {
	if input < 48 || input > 57 {
		log.Fatal("charToInt invalid input character ", input)
	}

	var out uint = uint(input) - 48
	return out
}

func main() {
	var file, err = os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()
	buffer, err := ioutil.ReadAll(file)

	var input = make([]uint, len(buffer)-1)
	for i := 0; i < len(buffer)-1; i++ {
		input[i] = charToInt(buffer[i])
	}

	var sum uint = 0
	for i := 0; i < len(input)-1; i++ {
		if input[i] == input[i+1] {
			sum += input[i]
		}
	}

	/* Edge case of circular buffer. */
	if input[0] == input[len(input)-1] {
		sum += input[0]
	}
	fmt.Printf("Day 01 part 1, answer: %d\n", sum)

	var sumPart2 uint = 0
	var stride int = len(input) / 2
	for i := 0; i < len(input); i++ {
		var j int = (i + stride) % len(input)

		if input[i] == input[j] {
			sumPart2 += input[i]
		}
	}
	fmt.Printf("Day 1 part 2, answer: %d\n", sumPart2)
}
