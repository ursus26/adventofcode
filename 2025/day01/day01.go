package main

import (
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
)

func main() {
	data, err := os.ReadFile("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	text := string(data)

	var dial int = 50
	var answerPart1 int = 0
	var answerPart2 int = 0

	lines := strings.Split(text, "\n")
	for i := range lines {
		/* Parsing */
		var line string = lines[i]
		amount, _ := strconv.Atoi(line[1:])
		if line[0] == 'L' {
			amount = -1 * amount
		}

		dialPrev := dial
		dial += amount

		if dialPrev > 0 && dial < 0 {
			for dial < 0 {
				dial += 100
				answerPart2++
			}
		} else if dialPrev == 0 && dial < 0 {
			for dial < 0 {
				dial += 100
				answerPart2++
			}
			answerPart2--
		} else if dial > 100 {
			for dial > 100 {
				dial -= 100
				answerPart2++
			}
		}

		dial = dial % 100
		if dial == 0 {
			answerPart1++
			answerPart2++
		}
	}

	fmt.Printf("Day 01 part 1, result: %d\n", answerPart1)
	fmt.Printf("Day 01 part 2, result: %d\n", answerPart2)
}
