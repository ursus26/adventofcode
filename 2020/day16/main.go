package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

type Range struct {
	lowerBound int
	upperBound int
}

type Rule struct {
	name  string
	lower Range
	upper Range
}

func (r Range) InRange(number int) bool {
	if number >= r.lowerBound && number <= r.upperBound {
		return true
	} else {
		return false
	}
}

func (r Rule) CheckRule(number int) bool {
	if r.lower.InRange(number) || r.upper.InRange(number) {
		return true
	} else {
		return false
	}
}

func parseRules(data string) []Rule {
	lines := strings.Split(data, "\n")
	out := make([]Rule, len(lines))

	for i, line := range lines {
		line = strings.Replace(line, " or ", "-", -1)
		line = strings.Replace(line, ": ", "-", -1)
		words := strings.Split(line, "-")

		range1Lower, _ := strconv.Atoi(words[1])
		range1Upper, _ := strconv.Atoi(words[2])
		range2Lower, _ := strconv.Atoi(words[3])
		range2Upper, _ := strconv.Atoi(words[4])
		range1 := Range{range1Lower, range1Upper}
		range2 := Range{range2Lower, range2Upper}
		rule := Rule{words[0], range1, range2}
		out[i] = rule
	}
	return out
}

func parseTicket(line string) []int {
	words := strings.Split(line, ",")
	out := make([]int, len(words))
	for i, word := range words {
		value, _ := strconv.Atoi(word)
		out[i] = value
	}
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
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n\n")
	rules := parseRules(data[0])
	myTicket := parseTicket(strings.Split(data[1], "\n")[1])
	ruleConstraints := make([][]bool, len(rules))
	for i := 0; i < len(rules); i++ {
		ruleConstraints[i] = make([]bool, len(myTicket))
		for j := 0; j < len(myTicket); j++ {
			ruleConstraints[i][j] = true
		}
	}

	ticketScanningErrorRate := 0
	tickets := strings.Split(data[2], "\n")[1:]
	for _, ticket := range tickets {
		fields := parseTicket(ticket)
		validTicket := true
		for _, field := range fields {
			/* Assume the field does not follow the rules. */
			validField := false
			for _, rule := range rules {
				/* Check if field follows the rule. If that is the case than revert the assumed error. */
				if rule.CheckRule(field) {
					validField = true
					break
				}
			}

			if !validField {
				validTicket = false
				ticketScanningErrorRate += field
			}
		}

		if validTicket {
			for i, rule := range rules {
				for j, field := range fields {
					if ruleConstraints[i][j] == false {
						continue
					}
					ruleConstraints[i][j] = rule.CheckRule(field)
				}
			}
		}
	}
	fmt.Printf("Day 16 part 1, answer: %d\n", ticketScanningErrorRate)

	ruleMapping := make(map[string]int)
	running := true
	for running {
		running = false

		for i, ruleConstraint := range ruleConstraints {
			optionCount := 0
			lastValidColumn := 0
			for j, possible := range ruleConstraint {
				if possible {
					optionCount++
					lastValidColumn = j
				}
			}

			if optionCount == 1 {
				ruleMapping[rules[i].name] = lastValidColumn
				for j := 0; j < len(ruleConstraints); j++ {
					ruleConstraints[j][lastValidColumn] = false
				}

				running = true
				break
			}
		}
	}

	part2 := 1
	for k, v := range ruleMapping {
		if strings.Contains(k, "departure") {
			part2 *= myTicket[v]
		}
	}
	fmt.Printf("Day 16 part 2, answer: %d\n", part2)
}
