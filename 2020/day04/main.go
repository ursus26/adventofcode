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

func inRange(value int, start int, stop int) bool {
	if value >= start && value <= stop {
		return true
	} else {
		return false
	}
}

func validateHeight(value string) bool {
	val, _ := strconv.Atoi(value[:len(value)-2])
	if strings.Contains(value, "cm") {
		return inRange(val, 150, 193)
	} else {
		return inRange(val, 59, 76)
	}
}

func validateField(key string, value string) bool {
	switch key {
	case "byr":
		v, _ := strconv.Atoi(value)
		return inRange(v, 1920, 2002)
	case "iyr":
		v, _ := strconv.Atoi(value)
		return inRange(v, 2010, 2020)
	case "eyr":
		v, _ := strconv.Atoi(value)
		return inRange(v, 2020, 2030)
	case "hgt":
		return validateHeight(value)
	case "hcl":
		matched, _ := regexp.Match(`^#[0-9|a-f]{6}$`, []byte(value))
		return matched
	case "ecl":
		matched, _ := regexp.Match(`^(amb)|(blu)|(brn)|(gry)|(grn)|(hzl)|(oth)$`, []byte(value))
		return matched
	case "pid":
		matched, _ := regexp.Match(`^\d{9}$`, []byte(value))
		return matched
	case "cid":
		return true
	default:
		fmt.Printf("Error, key: \"%s\" is not a valid input field and is ignored\n", key)
		return true
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
	requiredFields := [7]string{"byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"}
	validPart1 := 0
	validPart2 := 0

	for _, line := range data {
		/* Parse the input fields. */
		fields := strings.Split(strings.Replace(line, "\n", " ", -1), " ")
		passportFields := map[string]string{}
		for _, field := range fields {
			keyvalue := strings.Split(field, ":")
			key := keyvalue[0]
			value := keyvalue[1]
			passportFields[key] = value
		}

		/* Check if required field is present and if true then check if value is valid. */
		requiredFieldCount := 0
		validFieldCount := 0
		for _, requiredFieldName := range requiredFields {
			_, ok := passportFields[requiredFieldName]
			if ok {
				requiredFieldCount++
				if validateField(requiredFieldName, passportFields[requiredFieldName]) {
					validFieldCount++
				}
			}
		}

		if requiredFieldCount == len(requiredFields) {
			validPart1++
		}
		if validFieldCount == len(requiredFields) {
			validPart2++
		}
	}

	fmt.Printf("Day 4 part 1, valid passwords: %d\n", validPart1)
	fmt.Printf("Day 4 part 2, valid passwords: %d\n", validPart2)
}
