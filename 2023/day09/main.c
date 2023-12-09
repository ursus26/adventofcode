#include "list.h"
#include <stdio.h>
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


int main(int argc, char *argv[])
{
    list* l = read_lines("input.txt");

    /* Part 1 */
    int answer_part1 = 0;
    int answer_part2 = 0;
 
    for(int i = 0; i < l->count; i++)
    {
        char* nptr = (char *) l->data[i];
        int number_count = 1;
        for(int j = 0; j < strlen(nptr); j++)
            if(nptr[j] == ' ')
                number_count++;

        int grid[number_count][number_count];
        memset(&grid, 0, sizeof(grid));

        /* Parse numbers. */
        int idx = 0;
        while(*nptr != '\0')
        {
            char *endptr;
            int num = (int) strtol(nptr, &endptr, 10);
            if(nptr == endptr)
            {
                nptr++;
                continue;;
            }

            grid[0][idx] = num;
            idx++;
            nptr = endptr;
        }

        /* Calc difference. */
        int zero_row = 0;
        for(int current_row = 1; current_row < number_count; current_row++)
        {
            bool all_diff_zero = true;
            for(int j = 0; j < number_count - current_row; j++)
            {
                int diff = grid[current_row-1][j+1] - grid[current_row-1][j];
                grid[current_row][j] = diff;

                if(diff != 0)
                    all_diff_zero = false;
            }

            if(all_diff_zero)
            {
                zero_row = current_row;
                break;
            }
        }

        /* Calculate the previous and next number in the sequence. */
        int next_num = 0;
        int previous_num = 0;
        for(int j = zero_row; j > 0; j--)
        {
            next_num = grid[j-1][number_count - j] + next_num;
            previous_num = grid[j-1][0] - previous_num;
        }
        answer_part1 += next_num;
        answer_part2 += previous_num;
    }
 
    printf("Day 09 part 1, result: %d\n", answer_part1);
    printf("Day 09 part 2, result: %d\n", answer_part2);

    list_delete(l);
    l = NULL;
}