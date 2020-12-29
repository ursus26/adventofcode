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
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n")

	seatOccupancy := make([]bool, 2<<10)
	maxSeatID := 0
	minSeatID := 2 << 10
	for _, line := range data {
		column := 0
		row := 0

		/* Row */
		var shiftCount uint = 6
		for _, character := range line[:7] {
			if character == 'B' {
				row += 1 << shiftCount
			}
			shiftCount--
		}

		/* Column */
		shiftCount = 2
		for _, character := range line[7:10] {
			if character == 'R' {
				column += 1 << shiftCount
			}
			shiftCount--
		}

		/* Update seat occupancy and, the minimum and maximum range of the sead ID's. */
		seatID := 8*row + column
		seatOccupancy[seatID] = true
		if seatID > maxSeatID {
			maxSeatID = seatID
		}
		if seatID < minSeatID {
			minSeatID = seatID
		}
	}

	mySeatID := 0
	if minSeatID == 0 {
		minSeatID = 1
	}
	for i := minSeatID; i < maxSeatID; i++ {
		if seatOccupancy[i] == false && seatOccupancy[i-1] == true && seatOccupancy[i+1] == true {
			mySeatID = i
		}
	}
	fmt.Printf("Day 5 part 1, highest seat ID: %d\n", maxSeatID)
	fmt.Printf("Day 5 part 2, my seat ID: %d\n", mySeatID)
}
