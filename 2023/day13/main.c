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


list* transpose_grid(list* grid)
{
    list* new_grid = list_new();
    char** lines = (char **) grid->data;
    int length = strlen(lines[0]);

    for(int i = 0; i < length; i++)
    {
        char* column = (char *) malloc(grid->count + 1);
        for(int j = 0; j < grid->count; j++)
        {
            column[j] = lines[j][i];
        }
        column[grid->count] = '\0';
        list_append(new_grid, column);
    }

    return new_grid;
}

/**
 * Count the number of characters that are different between string s1 and string s2.
*/
int char_diff(char *s1, char *s2)
{
    int result = 0;
    for(int i = 0; i < strlen(s1) && i < strlen(s2); i++)
    {
        if(s1[i] != s2[i])
            result++;
    }

    int length_diff = strlen(s1) - strlen(s2);
    if(length_diff < 0)
        length_diff *= -1;

    return result + length_diff;
}


int horizontal_reflection(list* grid, bool enable_smudge_fixing)
{
    int result = 0;
    for(int i = 0; i < grid->count - 1; i++)
    {
        bool found_reflection = false;
        bool applied_smudge_fix = false;
        for(int j = 0; j <= i && i-j >= 0 && i + j + 1 < grid->count; j++)
        {
            char *line1 = (char *) grid->data[i - j];
            char *line2 = (char *) grid->data[i + j + 1];

            if(strcmp(line1, line2) == 0)
                found_reflection = true;
            else if(enable_smudge_fixing && char_diff(line1, line2) == 1 && !applied_smudge_fix)
            {
                applied_smudge_fix = true;
                found_reflection = true;
            }            
            else
            {
                found_reflection = false;
                break;
            }
        }

        if(found_reflection && (!enable_smudge_fixing || (enable_smudge_fixing && applied_smudge_fix)))
        {
            result = i + 1;
            break;
        }
    }

    return result;
}


int main(int argc, char *argv[])
{
    list* l = read_lines("input.txt");

    /* Part 1 */
    int answer_part1 = 0;
    int answer_part2 = 0;
 
    char** data = (char **) l->data;
    list* grid = list_new();
    list* grid_rotated;
    for(int i = 0; i < l->count; i++)
    {
        char *line = data[i];

        if(strlen(line) == 0)
        {
            grid_rotated = transpose_grid(grid);

            answer_part1 += 100 * horizontal_reflection(grid, false);
            answer_part1 += horizontal_reflection(grid_rotated, false);

            answer_part2 += 100 * horizontal_reflection(grid, true);
            answer_part2 += horizontal_reflection(grid_rotated, true);

            list_delete(grid_rotated);
            list_clear(grid);
            continue;
        }

        list_append(grid, strdup(line));
    }

    /* Evaluate final grid. */
    grid_rotated = transpose_grid(grid);

    answer_part1 += 100 * horizontal_reflection(grid, false);
    answer_part1 += horizontal_reflection(grid_rotated, false);

    answer_part2 += 100 * horizontal_reflection(grid, true);
    answer_part2 += horizontal_reflection(grid_rotated, true);

    printf("Day 13 part 1, result: %d\n", answer_part1);
    printf("Day 13 part 2, result: %d\n", answer_part2);

    list_delete(grid_rotated);
    list_delete(grid);
    list_delete(l);
    l = NULL;
}