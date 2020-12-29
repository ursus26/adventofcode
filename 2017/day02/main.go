package main

import (
	"container/list"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

func charToDigit(input byte) uint {
	if input < 48 || input > 57 {
		log.Fatal("charToInt invalid input character ", input)
	}

	var out uint = uint(input) - 48
	return out
}

func main() {
	var file, err = os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	/* Data parsing. */
	bytes, err := ioutil.ReadAll(file)
	data := strings.Trim(string(bytes), "\n")

	/* Parse data row by row. */
	spreadsheet := list.New()
	for _, rowString := range strings.Split(data, "\n") {
		items := strings.Split(rowString, "\t")
		row := make([]int, len(items))

		for i, value := range items {
			v, _ := strconv.Atoi(value)
			row[i] = v
		}
		spreadsheet.PushBack(row)
	}

	/* Part 1 calculation */
	var rowLarge int = 0
	var rowSmall int = 2 << 31
	var checksum int = 0
	for e := spreadsheet.Front(); e != nil; e = e.Next() {
		row := e.Value.([]int)

		for _, value := range row {
			if value > rowLarge {
				rowLarge = value
			}

			if value < rowSmall {
				rowSmall = value
			}
		}

		checksum += rowLarge - rowSmall
		rowLarge = 0
		rowSmall = 2 << 31
	}

	fmt.Printf("Day 2 part 1, answer: %d\n", checksum)

	/* Part 2 calculation */
	var checksumPart2 int = 0
	for e := spreadsheet.Front(); e != nil; e = e.Next() {
		row := e.Value.([]int)
		rowLength := len(row)
		rowSolutionFound := false

		for i := 0; i < rowLength-1; i++ {
			for j := i + 1; j < rowLength; j++ {
				var high int = 0
				var low int = 0
				if row[i] > row[j] {
					high = row[i]
					low = row[j]
				} else {
					high = row[j]
					low = row[i]
				}

				if high%low == 0 {
					checksumPart2 += (high / low)
					rowSolutionFound = true
					break
				}
			}
			if rowSolutionFound == true {
				break
			}
		}
	}

	fmt.Printf("Day 2 part 2, answer: %d\n", checksumPart2)
}
