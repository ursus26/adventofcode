package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"sort"
	"strings"
)

func main() {
	var file, err = os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	/* Data parsing. */
	bytes, err := ioutil.ReadAll(file)
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n")

	set := make(map[string]map[string]bool)
	for _, line := range data {
		breakIndex := strings.Index(line, "(")
		allergenSlice := line[breakIndex+10 : len(line)-1]
		ingredientSlice := line[:breakIndex-1]

		for _, allergen := range strings.Split(allergenSlice, ", ") {
			_, ok := set[allergen]
			if !ok {
				set[allergen] = make(map[string]bool)
				for _, ingredient := range strings.Split(ingredientSlice, " ") {
					set[allergen][ingredient] = true
				}
				continue
			}

			reducedSet := make(map[string]bool)
			for _, ingredient := range strings.Split(ingredientSlice, " ") {
				_, ok := set[allergen][ingredient]
				if ok {
					reducedSet[ingredient] = true
				}
			}
			set[allergen] = reducedSet
		}
	}

	// Reduce the options.
	dangerousIngredients := make(map[string]string)
	knownAllergen := make(map[string]string)
	solutionFound := true
	for solutionFound {
		solutionFound = false
		for k, v := range set {
			if len(v) == 1 {
				keyAllergen := ""
				for allergen, _ := range v {
					keyAllergen = allergen
					break
				}
				dangerousIngredients[keyAllergen] = k
				knownAllergen[k] = keyAllergen

				for _, v2 := range set {
					delete(v2, keyAllergen)
				}
				delete(set, k)
				solutionFound = true
				break
			}
		}
	}

	ingredientCount := 0
	for _, line := range data {
		breakIndex := strings.Index(line, "(")
		ingredientSlice := line[:breakIndex-1]

		for _, ingredient := range strings.Split(ingredientSlice, " ") {
			_, ok := dangerousIngredients[ingredient]
			if !ok {
				ingredientCount += 1
			}
		}
	}
	fmt.Printf("Day 21 part 1, answer: %d\n", ingredientCount)

	// PART 2
	sortedKeys := make([]string, 0, len(knownAllergen))
	for allergen, _ := range knownAllergen {
		sortedKeys = append(sortedKeys, allergen)
	}
	sort.Strings(sortedKeys)

	part2 := knownAllergen[sortedKeys[0]]
	for _, allergen := range sortedKeys[1:] {
		part2 += "," + knownAllergen[allergen]
	}
	fmt.Printf("Day 21 part 2, answer: %s\n", part2)
}
