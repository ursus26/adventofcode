package main

import (
	"fmt"
	"log"
	"os"
	"sort"
	"strconv"
	"strings"
)

type freshRange struct {
	low  int64
	high int64
}

func newFreshRange(text string) *freshRange {
	splitIndex := strings.Index(text, "-")
	low, _ := strconv.ParseInt(text[:splitIndex], 10, 64)
	high, _ := strconv.ParseInt(text[splitIndex+1:], 10, 64)
	return &freshRange{low: low, high: high}
}

func main() {
	data, err := os.ReadFile("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	text := string(data)
	splitIndex := strings.Index(text, "\n\n")

	rangeText := strings.Split(text[:splitIndex], "\n")
	ranges := make([]freshRange, len(rangeText))
	for i, line := range rangeText {
		ranges[i] = *newFreshRange(line)
	}
	sort.Slice(ranges, func(i, j int) bool {
		return ranges[i].low < ranges[j].low
	})

	ingredientText := strings.Split(text[splitIndex+2:], "\n")
	ingredients := make([]int64, len(ingredientText))
	for i, line := range ingredientText {
		ingredients[i], _ = strconv.ParseInt(line, 10, 64)
	}

	var answerPart1 int = 0
	var answerPart2 int64 = 0

	/* Part 1 */
	for _, ingredient := range ingredients {
		spoiled := true
		for _, f := range ranges {
			if ingredient >= f.low && ingredient <= f.high {
				spoiled = false
				break
			}
		}

		if !spoiled {
			answerPart1++
		}
	}

	/* Part 2, remove overlap in the freshness ranges. */
	newRanges := make([]freshRange, 0)
	r := freshRange{low: ranges[0].low, high: ranges[0].high}
	for i := 0; i < len(ranges); i++ {
		curr := ranges[i]

		if curr.low > r.high {
			newRanges = append(newRanges, r)
			r = freshRange{low: curr.low, high: curr.high}
			continue
		}

		if curr.low >= r.low && curr.low <= r.high && curr.high > r.high {
			r.high = curr.high
		}
	}
	newRanges = append(newRanges, r)

	/* Part 2, calculate the number of fresh indices. */
	for _, item := range newRanges {
		diff := item.high - item.low + 1
		answerPart2 += diff
	}

	fmt.Printf("Day 05 part 1, result: %d\n", answerPart1)
	fmt.Printf("Day 05 part 2, result: %d\n", answerPart2)
}
