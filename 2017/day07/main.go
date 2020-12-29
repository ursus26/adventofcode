package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

type Program struct {
	name           string
	weight         int
	childrenWeight int
	isBalanced     bool
	parent         string
	children       []string
}

func parseLine(line string) *Program {
	out := new(Program)
	out.parent = ""
	out.children = nil

	/* Parse name */
	words := strings.SplitN(line, " ", 2)
	out.name = words[0]

	/* Parse weight */
	words = strings.SplitN(strings.Trim(words[1], "("), ")", 2)
	w, _ := strconv.Atoi(words[0])
	out.weight = w
	out.childrenWeight = 0
	out.isBalanced = true

	if strings.Contains(line, "->") {
		words = strings.Split(line, " -> ")
		children := strings.Split(words[1], ", ")
		out.children = children
	}

	return out
}

func balanceCheck(programMap map[string]*Program, programName string) int {
	program := programMap[programName]
	lastChildWeight := 0

	for _, child := range program.children {
		childWeight := balanceCheck(programMap, child)

		if lastChildWeight != 0 && childWeight != lastChildWeight {
			program.isBalanced = false
		}

		lastChildWeight = childWeight
		program.childrenWeight += childWeight
	}

	if program.isBalanced == false {
		fmt.Printf("%s is unbalanced\n", programName)

		for _, child := range program.children {
			childProgram := programMap[child]
			total := childProgram.weight + childProgram.childrenWeight
			fmt.Printf("Child %s\thas weight: %d, \tchildrenWeight: %d,\ttotal weight: %d\n", child, childProgram.weight, childProgram.childrenWeight, total)
		}
	}

	out := program.weight + program.childrenWeight
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

	programMap := make(map[string]*Program)
	for _, item := range strings.Split(data, "\n") {
		program := parseLine(item)
		programMap[program.name] = program
	}

	/* Fix parents */
	for _, program := range programMap {
		for _, child := range program.children {
			childProgram := programMap[child]
			childProgram.parent = program.name
		}
	}

	/* Part 1, search for program without a parent. */
	var root string = ""
	for _, program := range programMap {
		if program.parent == "" {
			root = program.name
			break
		}
	}
	fmt.Printf("Day 7 part 1, answer: %s\n", root)

	/* Part 2, finding the error weight */
	balanceCheck(programMap, root)
	fmt.Printf("Answer can be found by looking at the children of the first unbalanced program.\n")
}
