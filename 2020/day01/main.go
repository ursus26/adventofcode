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
	dataRows := make([]int, len(data))

	for i, stringValue := range data {
		value, _ := strconv.Atoi(stringValue)
		dataRows[i] = value
	}

	answerPart1 := 0
	answerPart2 := 0

	for i, x := range dataRows[:len(data)-2] {
		for j, y := range dataRows[i+1 : len(data)-1] {
			sumPart1 := x + y
			if sumPart1 == 2020 {
				answerPart1 = x * y
			} else if sumPart1 < 2020 && answerPart2 == 0 {
				for _, z := range dataRows[j+1:] {
					if sumPart1+z == 2020 {
						answerPart2 = x * y * z
					}
				}
			}
		}

		if answerPart1 > 0 && answerPart2 > 0 {
			break
		}
	}

	fmt.Printf("Day 1 part 1, result: %d\n", answerPart1)
	fmt.Printf("Day 1 part 2, result: %d\n", answerPart2)
}
