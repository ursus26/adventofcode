package main

import (
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

type Instruction struct {
	opcode string
	param  int
}

type Program struct {
	code        []Instruction
	running     bool
	pc          int
	accumulator int
}

func (prog *Program) StepInstruction() {
	if len(prog.code) <= prog.pc {
		prog.running = false
		return
	}

	instr := prog.code[prog.pc]
	switch instr.opcode {
	case "acc":
		prog.accumulator += instr.param
	case "jmp":
		prog.pc += instr.param
		return
	case "nop":
		break
	default:
		fmt.Printf("Error, unknown instruction '%s %d'\n", instr.opcode, instr.param)
	}
	prog.pc += 1
}

/* Returns true if we encountered an infinite loop and false if the program terminates. */
func breakOnInfiniteLoop(prog *Program) bool {
	executedCode := make(map[int]bool)
	prog.pc = 0
	prog.accumulator = 0
	prog.running = true

	for prog.running {
		_, ok := executedCode[prog.pc]
		if !ok {
			executedCode[prog.pc] = true
			prog.StepInstruction()
		} else {
			return true
		}
	}
	return false
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
	bootProgram := Program{[]Instruction{}, true, 0, 0}

	for _, line := range data {
		tokens := strings.Split(line, " ")
		op := tokens[0]
		param, _ := strconv.Atoi(tokens[1])

		instr := Instruction{op, param}
		bootProgram.code = append(bootProgram.code, instr)
	}

	/* Part 1 */
	halted := breakOnInfiniteLoop(&bootProgram)
	part1 := 0
	if halted {
		part1 = bootProgram.accumulator
	}

	/* Part 2 */
	part2 := 0
	for i, instr := range bootProgram.code {
		/* Replace the instruction. */
		switch instr.opcode {
		case "nop":
			bootProgram.code[i] = Instruction{"jmp", instr.param}
		case "jmp":
			bootProgram.code[i] = Instruction{"nop", instr.param}
		default:
			continue
		}

		halted = breakOnInfiniteLoop(&bootProgram)
		if halted {
			bootProgram.code[i] = instr
		} else {
			part2 = bootProgram.accumulator
			break
		}
	}

	fmt.Printf("Day 8 part 1, answer: %d\n", part1)
	fmt.Printf("Day 8 part 2, answer: %d\n", part2)
}
