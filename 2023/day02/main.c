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
    const int max_red = 12;
    const int max_green = 13;
    const int max_blue = 14;

    list* l = read_lines("input.txt");

    for(int i = 0; i < l->count; i++)
    {
        bool valid_game = true;
        long cube_count = 0;
        long red_count = 0;
        long green_count = 0;
        long blue_count = 0;

        /* strtok variables */
        char* starting_point = strchr((char *) (l->data[i]), ':') + 2;
        char* saveptr;

        for(char* str = starting_point; ; str = NULL)
        {
            char* token = strtok_r(str, " ", &saveptr);
            if(token == NULL)
                break;

            /* parse number */
            if(is_digit(token[0]))
            {
                cube_count = strtol(token, NULL, 10);
                continue;
            }

            if(strstr(token, "red") != NULL)
            {
                if(cube_count > max_red)
                    valid_game = false;

                if(cube_count > red_count)
                    red_count = cube_count;
            }

            if(strstr(token, "green") != NULL)
            {
                if(cube_count > max_green)
                    valid_game = false;

                if(cube_count > green_count)
                    green_count = cube_count;
            }

            if(strstr(token, "blue") != NULL)
            {
                if(cube_count > max_blue)
                    valid_game = false;

                if(cube_count > blue_count)
                    blue_count = cube_count;
            }

            cube_count = 0;
        }

        if(valid_game)
            answer_part1 += i + 1;

        answer_part2 += (red_count * green_count * blue_count);
    }

    printf("Day 02 part 1, result: %d\n", answer_part1);
    printf("Day 02 part 2, result: %d\n", answer_part2);

    list_delete(l);
    l = NULL;
}