package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

func encrypt(subjectNumber int, loopSize int) int {
	value := 1
	for i := 0; i < loopSize; i++ {
		value = (value * subjectNumber) % 20201227
	}
	return value
}

func solveLoopSize(subjectNumber int, publicKey int) int {
	value := 1
	loopSize := 0
	for value != publicKey {
		value = (value * subjectNumber) % 20201227
		loopSize++
	}
	return loopSize
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

	publicKey1, _ := strconv.Atoi(data[0])
	publicKey2, _ := strconv.Atoi(data[1])

	loopSize1 := solveLoopSize(7, publicKey1)
	// loopSize2 := solveLoopSize(7, publicKey2)

	privateKey1 := encrypt(publicKey2, loopSize1)
	// privateKey2 := encrypt(publicKey1, loopSize2)
	fmt.Printf("Day 25, answer: %d\n", privateKey1)
}
