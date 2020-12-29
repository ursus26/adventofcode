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
	data := strings.Trim(string(bytes), "\n")
	stringOffsets := strings.Split(data, "\n")
	jumpOffsets := make([]int, len(stringOffsets))
	for i, val := range stringOffsets {
		v, _ := strconv.Atoi(val)

		jumpOffsets[i] = v
	}

	/* Part 1 */
	steps := 0
	index := 0
	upperBound := len(stringOffsets)
	for true {
		if index < 0 || index >= upperBound {
			break
		}

		jump := jumpOffsets[index]
		jumpOffsets[index] += 1
		index += jump
		steps += 1
	}

	fmt.Printf("Day 5 part 1, answer: %d\n", steps)

	/* Part 2 */
	/* Offset reset. */
	for i, val := range stringOffsets {
		v, _ := strconv.Atoi(val)

		jumpOffsets[i] = v
	}

	steps, index = 0, 0
	for true {
		if index < 0 || index >= upperBound {
			break
		}

		jump := jumpOffsets[index]
		if jump >= 3 {
			jumpOffsets[index] -= 1
		} else {
			jumpOffsets[index] += 1
		}

		index += jump
		steps += 1
	}

	fmt.Printf("Day 5 part 2, answer: %d\n", steps)
}
