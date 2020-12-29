package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"math"
	"os"
	"strconv"
	"strings"
)

func checkXMAS(number int, preamble []int) bool {
	for i := 0; i < len(preamble)-1; i++ {
		for j := i + 1; j < len(preamble); j++ {
			if preamble[i]+preamble[j] == number {
				return true
			}
		}
	}

	return false
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
	stream := make([]int, len(data))
	for i, line := range data {
		num, _ := strconv.Atoi(line)
		stream[i] = num
	}

	/* Part 1 */
	preambleSize := 25
	part1 := 0
	for i := preambleSize; i < len(stream); i++ {
		if !checkXMAS(stream[i], stream[i-preambleSize:i]) {
			part1 = stream[i]
			break
		}
	}

	/* Part 2 */
	part2 := 0
	sum := stream[0]
	for i, j := 0, 1; i < len(stream)-1; {
		if sum > part1 {
			sum -= stream[i]
			i++
		} else if sum < part1 {
			sum += stream[j]
			j++
		} else {
			small := math.MaxInt64
			high := math.MinInt64
			for k := i; k < j; k++ {
				val := stream[k]
				if val > high {
					high = val
				}
				if val < small {
					small = val
				}
			}
			part2 = small + high
			break
		}
	}

	fmt.Printf("Day 9 part 1, answer: %d\n", part1)
	fmt.Printf("Day 9 part 2, answer: %d\n", part2)
}
