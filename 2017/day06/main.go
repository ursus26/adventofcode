package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

func bankToKey(banks []int) string {
	convertedBanks := make([]string, len(banks))
	for i, item := range banks {
		s := strconv.Itoa(item)
		convertedBanks[i] = s
	}

	return strings.Join(convertedBanks, "-")
}

func findMostBlocks(banks []int) int {
	maxBlock := 0
	returnIndex := 0
	for i, val := range banks {
		if val > maxBlock {
			maxBlock = val
			returnIndex = i
		}
	}
	return returnIndex
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
	memoryBankData := strings.Split(data, "\t")
	memoryBankCount := len(memoryBankData)
	banks := make([]int, memoryBankCount)
	for i, val := range memoryBankData {
		v, _ := strconv.Atoi(val)
		banks[i] = v
	}

	bankState := make(map[string]int)
	k := bankToKey(banks)
	bankState[k] = 0
	cycles := 0
	cycleSize := 0

	for true {
		cycles += 1

		/* Redistribution */
		selectedBank := findMostBlocks(banks)
		blocks := banks[selectedBank]
		banks[selectedBank] = 0
		for i := blocks; i > 0; i-- {
			selectedBank = (selectedBank + 1) % memoryBankCount
			banks[selectedBank] += 1
		}

		/* Cycle check */
		k := bankToKey(banks)
		cycleHistory, cycleDetected := bankState[k]
		if cycleDetected {
			cycleSize = cycles - cycleHistory
			break
		}
		bankState[k] = cycles
	}

	fmt.Printf("Day 6 part 1, answer: %d\n", cycles)
	fmt.Printf("Day 6 part 2, answer: %d\n", cycleSize)
}
