package main

import (
	"fmt"
	"log"
	"os"
	"slices"
	"strconv"
	"strings"
)

func SliceToNumber(slice []int) int64 {
	var output int64 = 0
	for i := range slice {
		var num int64 = int64(slice[i])
		output = (10 * output) + num
	}
	return output
}

func main() {
	data, err := os.ReadFile("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	text := string(data)

	var answerPart1 int = 0
	var answerPart2 int64 = 0

	banks := strings.Split(text, "\n")
	for i := range banks {
		bank := banks[i]
		numbers := make([]int, len(bank))
		for j := range bank {
			numbers[j], _ = strconv.Atoi(bank[j : j+1])
		}

		/* Part 1 */
		high := slices.Max(numbers[:len(numbers)-1])
		low := slices.Max(numbers[slices.Index(numbers, high)+1:])
		answerPart1 += (10*high + low)

		/* Part 2 */
		var joltageNumbers [12]int
		searchStart := 0
		slice := numbers
		for j := range 12 {
			highPart2 := slices.Max(slice[searchStart : len(slice)-12+j+1])
			slice = slice[slices.Index(slice, highPart2)+1:]
			joltageNumbers[j] = highPart2
		}
		answerPart2 += SliceToNumber(joltageNumbers[:])
	}

	fmt.Printf("Day 03 part 1, result: %d\n", answerPart1)
	fmt.Printf("Day 03 part 2, result: %d\n", answerPart2)
}
