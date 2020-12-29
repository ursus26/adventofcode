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
	data := strings.Trim(string(bytes), "\n")

	score := 0
	depth := 0
	garbageBlock := false
	cancelNextCharacter := false
	cancelledCharactersCount := 0
	for _, character := range data {
		if cancelNextCharacter == true {
			cancelNextCharacter = false
			continue
		}

		if garbageBlock == true {
			switch character {
			case '>':
				garbageBlock = false
			case '!':
				cancelNextCharacter = true
			default:
				cancelledCharactersCount += 1
			}
		} else {
			switch character {
			case '{':
				depth += 1
			case '}':
				score += depth
				depth -= 1
			case '<':
				garbageBlock = true
			case ',':
				continue
			default:
				fmt.Printf("Error, unknown character: %c\n", character)
			}
		}
	}
	fmt.Printf("Day 9 part 1, score: %d\n", score)
	fmt.Printf("Day 9 part 2, cancelled character count: %d\n", cancelledCharactersCount)
}
