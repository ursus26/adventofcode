package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

func findMatchingBracket(expr string) string {
	startIndex := strings.Index(expr, "(")
	endIndex := startIndex
	level := 0
	for i, character := range expr[startIndex:] {
		if character == '(' {
			level++
		} else if character == ')' {
			level--
		}

		if level == 0 {
			endIndex = startIndex + i + 1
			break
		}
	}
	return expr[startIndex:endIndex]
}

func parseExpression(expr string) int {
	for strings.Contains(expr, "(") {
		subexpr := findMatchingBracket(expr)
		subExprSolution := parseExpression(subexpr[1 : len(subexpr)-1])
		expr = strings.Replace(expr, subexpr, strconv.Itoa(subExprSolution), 1)
	}

	tokens := strings.Split(expr, " ")
	exprValue, _ := strconv.Atoi(tokens[0])
	for i := 1; i < len(tokens); i += 2 {
		value, _ := strconv.Atoi(tokens[i+1])
		switch tokens[i] {
		case "+":
			exprValue += value
		case "*":
			exprValue *= value
		default:
			fmt.Printf("Invalid token: %s, expected: '+' or '*'\n", tokens[i])
		}
	}
	return exprValue
}

func parseAddition(expr string) string {
	for strings.Contains(expr, "+") {
		plusIndex := strings.Index(expr, "+")
		startIndexValue1 := strings.LastIndex(expr[:plusIndex-1], " ") + 1
		value1, _ := strconv.Atoi(expr[startIndexValue1 : plusIndex-1])

		endIndexValue2 := strings.Index(expr[plusIndex+2:], " ")
		if endIndexValue2 == -1 {
			endIndexValue2 = len(expr)
		} else {
			endIndexValue2 += plusIndex + 2
		}
		value2 := 0
		value2, _ = strconv.Atoi(expr[plusIndex+2 : endIndexValue2])
		answer := value1 + value2
		expr = strings.Replace(expr, expr[startIndexValue1:endIndexValue2], strconv.Itoa(answer), 1)
	}
	return expr
}

func parseExpressionPart2(expr string) int {
	for strings.Contains(expr, "(") {
		subexpr := findMatchingBracket(expr)
		subExprSolution := parseExpressionPart2(subexpr[1 : len(subexpr)-1])
		expr = strings.Replace(expr, subexpr, strconv.Itoa(subExprSolution), 1)
	}

	expr = parseAddition(expr)

	tokens := strings.Split(expr, " ")
	exprValue, _ := strconv.Atoi(tokens[0])
	for i := 1; i < len(tokens); i += 2 {
		value, _ := strconv.Atoi(tokens[i+1])
		switch tokens[i] {
		case "*":
			exprValue *= value
		default:
			fmt.Printf("Invalid token: %s, expected: '+' or '*'\n", tokens[i])
		}
	}
	return exprValue
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

	part1 := 0
	part2 := 0
	for _, line := range data {
		part1 += parseExpression(line)
		part2 += parseExpressionPart2(line)
	}

	fmt.Printf("Day 18 part 1, answer: %d\n", part1)
	fmt.Printf("Day 18 part 2, answer: %d\n", part2)
}
