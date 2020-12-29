package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"sort"
	"strconv"
	"strings"
)

func MinInt(a int, b int) int {
	if a < b {
		return a
	} else {
		return b
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
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n")
	joltageAdapters := make([]int, len(data)+1)
	for i, line := range data {
		num, _ := strconv.Atoi(line)
		joltageAdapters[i] = num
	}
	joltageAdapters[len(joltageAdapters)-1] = 0
	sort.Ints(joltageAdapters)

	/* Part 1 */
	previousJoltage := 0
	diff1JoltCount := 0
	diff3JoltCount := 1
	for _, joltage := range joltageAdapters {
		diff := joltage - previousJoltage
		if diff == 1 {
			diff1JoltCount++
		} else if diff == 3 {
			diff3JoltCount++
		}
		previousJoltage = joltage
	}
	part1 := diff1JoltCount * diff3JoltCount
	fmt.Printf("Day 10 part 1, answer: %d\n", part1)

	/* Part 2 */
	pathCount := make([]uint64, len(joltageAdapters))
	pathCount[0] = 1
	for i, joltage := range joltageAdapters[:len(joltageAdapters)-1] {
		for j, nextJoltage := range joltageAdapters[i+1:] {
			/* Check if we can branch out to the next node. If possible add the current number of
			 * paths to the next node.  */
			if nextJoltage-joltage <= 3 {
				pathCount[i+j+1] += pathCount[i]
			} else {
				break
			}
		}
	}
	part2 := pathCount[len(pathCount)-1]
	fmt.Printf("Day 10 part 2, answer: %d\n", part2)
}
