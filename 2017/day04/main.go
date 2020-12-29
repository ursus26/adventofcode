package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"sort"
	"strings"
)

func sortString(word string) string {
	characters := strings.Split(word, "")
	sort.Strings(characters)
	return strings.Join(characters, "")
}

func main() {
	var file, err = os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	/* Data parsing. */
	bytes, err := ioutil.ReadAll(file)
	passphrases := strings.Trim(string(bytes), "\n")

	validCount, validCountPart2 := 0, 0
	validPhrase, validWithAnagrams := true, true
	for _, phrase := range strings.Split(passphrases, "\n") {
		words := make(map[string]bool)
		anagrams := make(map[string]bool)
		validPhrase = true
		validWithAnagrams = true

		for _, word := range strings.Split(phrase, " ") {
			_, ok := words[word]
			if ok {
				validPhrase = false
			} else {
				words[word] = true
			}

			baseAnagram := sortString(word)
			_, ok = anagrams[baseAnagram]
			if ok {
				validWithAnagrams = false
			} else {
				anagrams[baseAnagram] = true
			}
		}

		if validPhrase {
			validCount += 1
		}

		if validWithAnagrams {
			validCountPart2 += 1
		}
	}

	fmt.Printf("Day 4 part 1, answer: %d\n", validCount)
	fmt.Printf("Day 4 part 2, answer: %d\n", validCountPart2)
}
