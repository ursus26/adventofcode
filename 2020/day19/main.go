package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

const (
	ITEM_RULE      = iota
	ITEM_CHARACTER = iota
)

var rules []Rule

type Requirement struct {
	requirementType int
	value           int
}

type Expression struct {
	items []Requirement
}

type Rule struct {
	id          int
	expressions []Expression
}

func NewRule(id int) Rule {
	rule := Rule{
		id:          id,
		expressions: []Expression{},
	}
	return rule
}

func parseRule(line string) Rule {
	words := strings.Split(line, ": ")

	ruleID, _ := strconv.Atoi(words[0])
	newRule := NewRule(ruleID)

	requirements := strings.Split(words[1], " | ")
	for _, requirementList := range requirements {
		expr := Expression{}
		for _, requirement := range strings.Split(requirementList, " ") {
			if requirement[0] == '"' {
				character := -1
				if requirement[1] == 'a' {
					character = 0
				} else if requirement[1] == 'b' {
					character = 1
				}
				item := Requirement{
					requirementType: ITEM_CHARACTER,
					value:           character,
				}
				expr.items = append(expr.items, item)
				continue
			} else {
				value, _ := strconv.Atoi(requirement)
				item := Requirement{
					requirementType: ITEM_RULE,
					value:           value,
				}
				expr.items = append(expr.items, item)
			}
		}
		newRule.expressions = append(newRule.expressions, expr)
	}
	return newRule
}

func matchRule(ruleID int, word []int) (int, bool) {
	rule := rules[ruleID]
	ruleMatched := false
	matchedCharacters := 0
	for _, expression := range rule.expressions {
		count, ok := matchExpression(expression, word)
		if ok {
			matchedCharacters = count
			ruleMatched = true
			break
		}
	}

	return matchedCharacters, ruleMatched
}

func matchExpression(expr Expression, word []int) (int, bool) {
	matchedCharacters := 0
	for _, item := range expr.items {
		characterCount, ok := matchRequirement(item, word[matchedCharacters:])
		if !ok {
			return 0, false
		}
		matchedCharacters += characterCount
	}
	return matchedCharacters, true
}

func matchRequirement(requirement Requirement, word []int) (int, bool) {
	switch requirement.requirementType {
	case ITEM_CHARACTER:
		if len(word) == 0 {
			return 0, false
		} else if word[0] != requirement.value {
			return 0, false
		} else {
			return 1, true
		}
	case ITEM_RULE:
		return matchRule(requirement.value, word)
	default:
		return 0, false
	}
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
	unparsedRules := strings.Split(data[0], "\n")

	rules = make([]Rule, 1024)
	for _, unparsedRule := range unparsedRules {
		newRule := parseRule(unparsedRule)
		rules[newRule.id] = newRule
	}

	/* Part 1 */
	part1 := 0
	lines := strings.Split(data[1], "\n")
	for _, line := range lines {
		word := make([]int, len(line))
		for i, character := range line {
			switch character {
			case 'a':
				word[i] = 0
			case 'b':
				word[i] = 1
			default:
				word[i] = -1
			}
		}
		matchedCharacters, valid := matchRule(0, word)
		if matchedCharacters != len(word) {
			valid = false
		}

		if valid {
			part1 += 1
		}
	}

	/* Part 2 */
	rule8 := parseRule("8: 42 | 42 8")
	rule11 := parseRule("11: 42 31 | 42 11 31")
	rules[8] = rule8
	rules[11] = rule11

	part2 := 0
	for _, line := range lines {
		word := make([]int, len(line))
		for i, character := range line {
			switch character {
			case 'a':
				word[i] = 0
			case 'b':
				word[i] = 1
			default:
				word[i] = -1
			}
		}
		matchedCharacters, valid := matchRule(0, word)
		if matchedCharacters != len(word) {
			valid = false
		}

		if valid {
			// fmt.Printf("PART2 | Valid word: %s\n", line)
			part2 += 1
		}
	}

	fmt.Printf("Day 19 part 1, answer: %d\n", part1)
	fmt.Printf("Part 2 is not finished\nDay 19 part 2, answer: %d\n", part2)
}
