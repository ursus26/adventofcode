#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>


char* read_file(char* file_name)
{
    FILE* file = fopen(file_name, "r");
    if(file == NULL)
    {
        fprintf(stderr, "Error cannot open file [%s]\n", file_name);
        exit(EXIT_FAILURE);
    }

    /* Get file size. */
    fseek(file, 0, SEEK_END);
    long file_size = ftell(file);
    rewind(file);

    /* Get file content. */
    char* ret_val = malloc(file_size);
    memset(ret_val, '\0', file_size);
    for(int i = 0; i < file_size; i++)
        ret_val[i] = fgetc(file);

    fclose(file);
    return ret_val;
}


int find_text_digit(char* text)
{
    char c = text[0];

    switch (c)
    {
    case 'e':
        if(strncmp(text, "eight", 5) == 0)
            return 8;
        break;

    case 'f':
        if(strncmp(text, "four", 4) == 0)
            return 4;
        else if(strncmp(text, "five", 4) == 0)
            return 5;
        break;

    case 'n':
        if(strncmp(text, "nine", 4) == 0)
            return 9;
        break;

    case 'o':
        if(strncmp(text, "one", 3) == 0)
            return 1;
        break;

    case 's':
        if(strncmp(text, "six", 3) == 0)
            return 6;
        else if(strncmp(text, "seven", 5) == 0)
            return 7;
        break;

    case 't':
        if(strncmp(text, "two", 3) == 0)
            return 2;
        else if(strncmp(text, "three", 4) == 0)
            return 3;
        break;
    
    default:
        break;
    }

    return 0;
}


int main(int argc, char *argv[])
{
    char* puzzle_input = read_file("input.txt");
    int answer_part1 = 0;
    int answer_part2 = 0;

    size_t string_length = strlen(puzzle_input);
    int first_digit_part1 = 0;
    int last_digit_part1 = 0;
    bool first_digit_part1_set = false;
    int first_digit_part2= 0;
    int last_digit_part2 = 0;
    bool first_digit_part2_set = false;
    int digit = 0;
    for(int i = 0; i < string_length; i++)
    {
        char c = puzzle_input[i];
        switch (c)
        {
        case '\n':
            /* Running sum. */
            answer_part1 += 10 * first_digit_part1 + last_digit_part1;
            answer_part2 += 10 * first_digit_part2 + last_digit_part2;
            
            /* Reset loop variables. */
            first_digit_part1 = 0;
            last_digit_part1 = 0;
            first_digit_part1_set = false;

            first_digit_part2 = 0;
            last_digit_part2 = 0;
            first_digit_part2_set = false;
            break;

        case '0':
        case '1':
        case '2':
        case '3':
        case '4':
        case '5':
        case '6':
        case '7':
        case '8':
        case '9':
            digit = c - 48;
            if(first_digit_part1_set == false)
            {
                first_digit_part1 = digit;
                first_digit_part1_set = true;
            }
            if(first_digit_part2_set == false)
            {
                first_digit_part2 = digit;
                first_digit_part2_set = true;
            }
            last_digit_part1 = digit;
            last_digit_part2 = digit;
            break;
        
        case 'e':
        case 'f':
        case 'n':
        case 'o':
        case 's':
        case 't':
            digit = find_text_digit(puzzle_input + i);
            if(digit != 0)
            {
                if(first_digit_part2_set == false)
                {
                    first_digit_part2 = digit;
                    first_digit_part2_set = true;
                }

                last_digit_part2 = digit;
            }
            break;

        default:
            break;
        }
    }

    printf("Day 01 part 1, result: %d\n", answer_part1);
    printf("Day 01 part 2, result: %d\n", answer_part2);

    free(puzzle_input);
    puzzle_input = NULL;
}