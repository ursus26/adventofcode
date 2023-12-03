#include "list.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>


list* read_lines(char* file_name)
{
    FILE* file = fopen(file_name, "r");
    if(file == NULL)
    {
        fprintf(stderr, "Error cannot open file [%s]\n", file_name);
        exit(EXIT_FAILURE);
    }

    list* l = list_new();
    const size_t buffer_size = 256;
    char* buffer = malloc(buffer_size * sizeof(char));
    while(fgets(buffer, buffer_size, file) != NULL)
    {
        /* Remove new line character */
        char* newline_index = strchr(buffer, '\n');
        if(newline_index != NULL)
            *newline_index = '\0';

        list_append(l, buffer);
        buffer = malloc(buffer_size * sizeof(char));
    }
    free(buffer);

    fclose(file);
    return l;
}


bool is_digit(char c)
{
    if(c >= 48 && c <= 57)
        return true;
    else
        return false;
}


int main(int argc, char *argv[])
{
    int answer_part1 = 0;
    int answer_part2 = 0;

    list* l = read_lines("input.txt");

    /* Gear grid for part 2. Assume input does not contain gears with 4 adjacent numbers. */
    int** gear_grid = malloc(l->count * sizeof(int*));
    for(int i = 0; i < l->count; i++)
    {
        gear_grid[i] = malloc(strlen(l->data[0]) * sizeof(int));
        for(int j = 0; j < strlen(l->data[0]); j++)
        {
            gear_grid[i][j] = 0;
        }
    }

    for(int i = 0; i < l->count; i++)
    {
        char* cur_line = l->data[i];
        char* nptr = cur_line;
        while(*nptr != '\0')
        {
            /* Catch and ignore the optional characters for strtol. */
            if(*nptr == '+' || *nptr == '-')
            {
                nptr++;
                continue;
            }

            char* endptr;
            long value = strtol(nptr, &endptr, 10);
            if(endptr == nptr)
            {
                nptr++;
                continue;
            }

            /* Scan around number to find special characters. */
            bool adjacent_to_symbol = false;
            int line_idx = nptr - cur_line;
            int number_length = endptr - nptr;

            /* Scan above */
            if(i > 0)
            {
                char* prev_line = l->data[i-1];
                for(int j = line_idx - 1; j < line_idx + number_length + 1; j++)
                {
                    if(j < 0)
                        continue;

                    if(prev_line[j] == '\0')
                        break;

                    if(!is_digit(prev_line[j]) && prev_line[j] != '.')
                    {
                        adjacent_to_symbol = true;

                        /* Part 2 */
                        if(prev_line[j] == '*')
                        {
                            if(gear_grid[i-1][j] == 0)
                                gear_grid[i-1][j] = value;
                            else
                            {
                                answer_part2 += (value * gear_grid[i-1][j]);
                                gear_grid[i-1][j] = 0;
                            }
                        }
                    }
                }
            }

            /* Scan left and right */
            if(line_idx > 0)
            {
                int j = line_idx - 1;
                if(!is_digit(cur_line[j]) && cur_line[j] != '.')
                {
                    adjacent_to_symbol = true;

                    /* Part 2 */
                    if(cur_line[j] == '*')
                    {
                        if(gear_grid[i][j] == 0)
                            gear_grid[i][j] = value;
                        else
                        {
                            answer_part2 += (value * gear_grid[i][j]);
                            gear_grid[i][j] = 0;
                        }
                    }
                }
            }

            if(*endptr != '\0')
            {
                int j = line_idx + number_length;
                if(!is_digit(*endptr) && *endptr != '.')
                {
                    adjacent_to_symbol = true;

                    /* Part 2 */
                    if(*endptr == '*')
                    {
                        if(gear_grid[i][j] == 0)
                            gear_grid[i][j] = value;
                        else
                        {
                            answer_part2 += (value * gear_grid[i][j]);
                            gear_grid[i][j] = 0;
                        }
                    }
                }
            }

            /* Scan below */
            if(i + 1 < l->count)
            {
                char* next_line = l->data[i+1];
                for(int j = line_idx - 1; j < line_idx + number_length + 1; j++)
                {
                    if(j < 0)
                        continue;

                    if(next_line[j] == '\0')
                        break;

                    if(!is_digit(next_line[j]) && next_line[j] != '.')
                    {
                        adjacent_to_symbol = true;

                        /* Part 2 */
                        if(next_line[j] == '*')
                        {
                            if(gear_grid[i+1][j] == 0)
                                gear_grid[i+1][j] = value;
                            else
                            {
                                answer_part2 += (value * gear_grid[i+1][j]);
                                gear_grid[i+1][j] = 0;
                            }
                        }
                    }
                }
            }

            if(adjacent_to_symbol)
            {
                answer_part1 += value;
            }

            nptr = endptr;
        }
    }

    printf("Day 03 part 1, result: %d\n", answer_part1);
    printf("Day 03 part 2, result: %d\n", answer_part2);

    for(int i = 0; i < l->count; i++)
    {
        free(gear_grid[i]);
    }
    free(gear_grid);

    list_delete(l);
    l = NULL;
}