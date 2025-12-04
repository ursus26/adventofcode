package main

import (
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
)

func IsInvalidPart1(num int) bool {
	s := strconv.Itoa(num)
	s1 := s[:len(s)/2]
	s2 := s[len(s)/2:]
	return s1 == s2
}

func IsInvalidPart2(num int) bool {
	s := strconv.Itoa(num)
	length := len(s)
	for sliceLength := 1; sliceLength <= length/2; sliceLength++ {

		/* Skip slice test if we cannot divide the string into equal sized slices. */
		if length%sliceLength != 0 {
			continue
		}

		/* Create a pattern. */
		pattern := s[:sliceLength]
		patternMatch := true

		/* Iterate over the remaining slices and check if they match the pattern. */
		for j := sliceLength; j < length; j += sliceLength {
			slice := s[j : j+sliceLength]

			if pattern != slice {
				patternMatch = false
				break
			}
		}

		if patternMatch {
			return true
		}
	}
	return false
}

func main() {
	data, err := os.ReadFile("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	text := string(data)

	var answerPart1 int = 0
	var answerPart2 int = 0

	lines := strings.Split(text, ",")
	for i := range lines {
		rawRange := lines[i]
		idx := strings.Index(rawRange, "-")
		low, _ := strconv.Atoi(rawRange[0:idx])
		high, _ := strconv.Atoi(rawRange[idx+1:])

		for j := low; j <= high; j++ {
			if IsInvalidPart1(j) {
				answerPart1 += j
			}

			if IsInvalidPart2(j) {
				answerPart2 += j
			}
		}
	}

	fmt.Printf("Day 01 part 1, result: %d\n", answerPart1)
	fmt.Printf("Day 01 part 2, result: %d\n", answerPart2)
}
