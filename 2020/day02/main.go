package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
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
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n")

	validPasswordsPart1 := 0
	validPasswordsPart2 := 0
	for _, line := range data {
		/* Input parsing. */
		splitLine := strings.Split(line, ": ")
		password := splitLine[1]
		splitPolicy := strings.Split(splitLine[0], " ")
		numberRange := strings.Split(splitPolicy[0], "-")
		lowerLimit, _ := strconv.Atoi(numberRange[0])
		upperLimit, _ := strconv.Atoi(numberRange[1])
		letter := splitPolicy[1]

		/* Part 1: check if password letter count is valid. */
		letterCount := strings.Count(password, letter)
		if letterCount >= lowerLimit && letterCount <= upperLimit {
			validPasswordsPart1 += 1
		}

		/* Part 2: check if letter only occurs once in the two given positions. */
		if (password[lowerLimit-1] == letter[0]) != (password[upperLimit-1] == letter[0]) {
			validPasswordsPart2++
		}
	}

	fmt.Printf("Day 2 part 1, result: %d\n", validPasswordsPart1)
	fmt.Printf("Day 2 part 2, result: %d\n", validPasswordsPart2)
}
