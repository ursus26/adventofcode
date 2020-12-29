package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strings"
)

func main() {
	var file, err = os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	/* Data parsing. */
	bytes, err := ioutil.ReadAll(file)
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n\n")
	sumPart1 := 0
	sumPart2 := 0

	for _, group := range data {
		groupAnswers := map[rune]int{}
		people := strings.Split(group, "\n")

		for _, person := range people {
			for _, answer := range person {
				_, ok := groupAnswers[answer]
				if !ok {
					groupAnswers[answer] = 1
					sumPart1 += 1
				} else {
					groupAnswers[answer] += 1
				}
			}
		}

		for _, answerCount := range groupAnswers {
			if answerCount == len(people) {
				sumPart2 += 1
			}
		}
	}

	fmt.Printf("Day 6 part 1, answer: %d\n", sumPart1)
	fmt.Printf("Day 6 part 2, answer: %d\n", sumPart2)
}
