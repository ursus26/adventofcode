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

func checkCond(operator string, param1 int, param2 int) bool {
	result := false
	switch operator {
	case "==":
		result = param1 == param2
	case "!=":
		result = param1 != param2
	case "<":
		result = param1 < param2
	case "<=":
		result = param1 <= param2
	case ">":
		result = param1 > param2
	case ">=":
		result = param1 >= param2
	default:
		fmt.Printf("Unimplemented conditional operator: %s\n", operator)
	}

	return result
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

	registers := make(map[string]int)
	allTimeMaxRegValue := math.MinInt32
	for _, line := range strings.Split(data, "\n") {
		/* Instruction parsing. */
		words := strings.Split(line, " ")
		register := words[0]
		operation := words[1]
		parameter, _ := strconv.Atoi(words[2])
		condParam1 := registers[words[4]]
		condParam2, _ := strconv.Atoi(words[6])
		condOperator := words[5]

		conditionResult := checkCond(condOperator, condParam1, condParam2)
		if conditionResult == true {
			switch operation {
			case "inc":
				registers[register] += parameter
			case "dec":
				registers[register] -= parameter
			default:
				fmt.Printf("Unimplemented operation: %s\n", operation)
			}

			if allTimeMaxRegValue < registers[register] {
				allTimeMaxRegValue = registers[register]
			}
		}
	}

	maxRegName := ""
	maxRegValue := math.MinInt32
	for reg, val := range registers {
		if val > maxRegValue {
			maxRegName = reg
			maxRegValue = val
		}
	}
	fmt.Printf("Day 8 part 1, register '%s' has the highest value: %d\n", maxRegName, maxRegValue)
	fmt.Printf("Day 8 part 2, all time highest value: %d\n", allTimeMaxRegValue)
}
