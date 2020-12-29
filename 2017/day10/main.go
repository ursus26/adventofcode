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

type Knot struct {
	list [256]int
}

func (knot *Knot) Reverse(start int, length int) {
	steps := int(math.Floor(float64(length) / 2.0))
	for i := 0; i < steps; i++ {
		index1 := (start + i) % 256
		index2 := (start + length - i - 1 + 256) % 256
		knot.list[index1], knot.list[index2] = knot.list[index2], knot.list[index1]
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
	data := strings.Trim(string(bytes), "\n")

	knot := Knot{}
	for i := 0; i < 256; i++ {
		knot.list[i] = i
	}

	currentPosition := 0
	skipSize := 0
	stringLengths := strings.Split(data, ",")
	for _, length := range stringLengths {
		lengthInt, _ := strconv.Atoi(length)
		knot.Reverse(currentPosition, lengthInt)
		currentPosition = (currentPosition + lengthInt + skipSize) % 256
		skipSize = (skipSize + 1) % 256
	}

	checkSumPart1 := knot.list[0] * knot.list[1]
	fmt.Printf("Day 10 part 1, result: %d\n", checkSumPart1)
}
