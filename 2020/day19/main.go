package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"regexp"
	"strconv"
	"strings"
)

func parseRuleToRegexString(ruleID int, parsedRules map[int]string, unparsedRules map[int]string, part2 bool) string {
	// Check if we have already parsed the rule.
	result, ok := parsedRules[ruleID]
	if ok {
		return result
	} else {
		result = ""
	}

	expression := unparsedRules[ruleID]
	orExpression := false
	for _, requirement := range strings.Split(expression, " ") {
		if requirement[0] == 'a' || requirement[0] == 'b' {
			result += requirement
		} else if requirement[0] == '|' {
			orExpression = true
			result += "|"
		} else {
			value, _ := strconv.Atoi(requirement)

			if part2 && value == 8 {
				result += parseRuleToRegexString(42, parsedRules, unparsedRules, part2) + "+"
				continue
			} else if part2 && value == 11 {
				rule42 := parseRuleToRegexString(42, parsedRules, unparsedRules, part2)
				rule31 := parseRuleToRegexString(31, parsedRules, unparsedRules, part2)
				maxLoops := 5
				result += "("
				for i := 0; i < maxLoops-1; i++ {
					result += strings.Repeat(rule42, i+1) + strings.Repeat(rule31, i+1) + "|"
				}
				result += strings.Repeat(rule42, maxLoops) + strings.Repeat(rule31, maxLoops) + ")"
				continue
			}

			regexExpression, ok := parsedRules[value]
			if ok {
				result += regexExpression
			} else {
				result += parseRuleToRegexString(value, parsedRules, unparsedRules, part2)
			}
		}
	}

	if orExpression {
		result = "(" + result + ")"
	}
	parsedRules[ruleID] = result
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
	stringData := string(bytes)
	data := strings.Split(strings.Trim(strings.ReplaceAll(stringData, "\"", ""), "\n"), "\n\n")
	ruleLines := strings.Split(data[0], "\n")

	unparsedRules := make(map[int]string)
	for _, line := range ruleLines {
		words := strings.Split(line, ": ")
		ruleID, _ := strconv.Atoi(words[0])
		unparsedRules[ruleID] = words[1]
	}

	parsedRules := make(map[int]string)
	regexString := "^" + parseRuleToRegexString(0, parsedRules, unparsedRules, false) + "$"
	regexPart1 := regexp.MustCompile(regexString)

	/* Part 1 */
	part1 := 0
	lines := strings.Split(data[1], "\n")
	for _, line := range lines {
		if regexPart1.MatchString(line) {
			part1 += 1
		}
	}

	delete(parsedRules, 0)
	delete(parsedRules, 8)
	delete(parsedRules, 11)
	regexString2 := "^" + parseRuleToRegexString(0, parsedRules, unparsedRules, true) + "$"
	regexPart2 := regexp.MustCompile(regexString2)

	/* Part 2 */
	part2 := 0
	for _, line := range lines {
		if regexPart2.MatchString(line) {
			part2 += 1
		}
	}

	fmt.Printf("Day 19 part 1, answer: %d\n", part1)
	fmt.Printf("Day 19 part 2, answer: %d\n", part2)
}
