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

func AbsInt(a int) int {
	if a >= 0 {
		return a
	}
	return -1 * a
}

func gcd(a int, b int) int {
	if a == 0 {
		return b
	} else if b == 0 {
		return a
	}

	if a > b {
		return gcd(a%b, b)
	} else if a < b {
		return gcd(a, b%a)
	} else {
		return a
	}
}

func lcm(a int, b int) int {
	return AbsInt(a*b) / gcd(a, b)
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

	/* Part 1 */
	myDepartureTime, _ := strconv.Atoi(data[0])
	minWaitTime := math.MaxInt64
	part1 := 0
	for _, val := range strings.Split(data[1], ",") {
		if val == "x" {
			continue
		}

		busID, _ := strconv.Atoi(val)
		remainder := myDepartureTime % busID
		if remainder == 0 {
			minWaitTime = 0
			part1 = 0
			break
		}

		waitTime := busID - remainder
		if waitTime < minWaitTime {
			minWaitTime = waitTime
			part1 = busID * waitTime
		}

	}
	fmt.Printf("Day 13 part 1, answer: %d\n", part1)

	/* Part 2 */
	part2 := math.MaxInt64
	t := 0
	stride := 1
	offset := 0
	for _, val := range strings.Split(data[1], ",") {
		if val == "x" {
			offset++
			continue
		}

		busID, _ := strconv.Atoi(val)

		for true {
			if (t+offset)%busID == 0 {
				break
			}

			t += stride
		}
		stride = lcm(stride, busID)
		offset++

	}
	part2 = t
	fmt.Printf("Day 13 part 2, answer: %d\n", part2)
}
