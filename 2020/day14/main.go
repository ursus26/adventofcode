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

/* Returns 3 masks. The and mask has all bits set to 1 except for bits which are 0 in the mask.
 * The or mask has all bits set to 0 except for the bits that 1 in the mask. The xMask has all bits
 * set to 0 except for the bits that are an X in the mask. */
func parseMask(word string) (uint64, uint64, uint64) {
	var andMask uint64 = math.MaxUint64
	var orMask uint64 = 0
	var xMask uint64 = 0
	for i, c := range word {
		switch c {
		case '0':
			andMask &= (math.MaxUint64 ^ (1 << uint64(len(word)-1-i)))
		case '1':
			orMask |= (1 << uint64((len(word) - 1 - i)))
		case 'X':
			xMask |= (1 << uint64((len(word) - 1 - i)))
		default:
			continue
		}
	}
	return andMask, orMask, xMask
}

func countSetBits(value uint64) uint64 {
	var count uint64 = 0
	for count = 0; value > 0; value >>= 1 {
		count += (value & 1)
	}
	return count
}

func maskedAddresses(baseAddress uint64, xMask uint64) []uint64 {
	/* Create a list with the unchanged address. The size is the number of set bits in the xMask. */
	maskedAddresses := []uint64{}
	addressCount := 1 << countSetBits(xMask)
	for i := 0; i < addressCount; i++ {
		maskedAddresses = append(maskedAddresses, baseAddress)
	}

	/* Iteratively search for set bits in the xMask. */
	activeBit := 1
	for i := uint64(1); i != 0; i <<= 1 {
		if i&xMask > 0 {
			/* Check if we need to set or unset the active bit for every address. */
			for j, _ := range maskedAddresses {
				if activeBit&j > 0 {
					maskedAddresses[j] |= i
				} else {
					maskedAddresses[j] &= (^i)
				}
			}
			activeBit <<= 1
		}
	}
	return maskedAddresses
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

	var andMask uint64 = math.MaxUint64
	var orMask uint64 = 0
	var xMask uint64 = 0
	memoryPart1 := make(map[uint64]uint64)
	memoryPart2 := make(map[uint64]uint64)
	for _, line := range data {
		words := strings.Split(line, " = ")
		if words[0] == "mask" {
			andMask, orMask, xMask = parseMask(words[1])
		} else {
			address, _ := strconv.ParseUint(words[0][4:len(words[0])-1], 10, 64)
			value, _ := strconv.ParseUint(words[1], 10, 64)

			/* Part 1 write. */
			memoryPart1[address] = (value | orMask) & andMask

			/* Part 2 write. */
			for _, addressPart2 := range maskedAddresses((address | orMask), xMask) {
				memoryPart2[addressPart2] = value
			}
		}
	}

	var part1 uint64 = 0
	for _, value := range memoryPart1 {
		part1 += value
	}
	var part2 uint64 = 0
	for _, value := range memoryPart2 {
		part2 += value
	}
	fmt.Printf("Day 14 part 1, answer: %d\n", part1)
	fmt.Printf("Day 14 part 2, answer: %d\n", part2)
}
