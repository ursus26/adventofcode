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


void tilt_north(list* grid)
{
    char** data = (char **) grid->data;
    int column_count = strlen(data[0]);

    for(int i = 0; i < column_count; i++)
    {
        for(int j = 0; j < grid->count; j++)
        {
            char c = data[j][i];
            if(c == 'O')
            {
                for(int k = j - 1; k >= 0; k--)
                {
                    char next_position = data[k][i];
                    if(next_position == '.')
                    {
                        data[k][i] = 'O';
                        data[k+1][i] = '.';
                    }
                    else
                        break;
                }
            }            
        }
    }
}


void tilt_south(list* grid)
{
    char** data = (char **) grid->data;
    int column_count = strlen(data[0]);

    for(int i = 0; i < column_count; i++)
    {
        for(int j = grid->count - 1; j >= 0; j--)
        {
            char c = data[j][i];
            if(c == 'O')
            {
                for(int k = j + 1; k < grid->count; k++)
                {
                    char next_position = data[k][i];
                    if(next_position == '.')
                    {
                        data[k][i] = 'O';
                        data[k-1][i] = '.';
                    }
                    else
                        break;
                }
            }            
        }
    }
}


void tilt_west(list* grid)
{
    char** data = (char **) grid->data;
    int column_count = strlen(data[0]);

    for(int i = 0; i < grid->count; i++)
    {
        for(int j = 0; j < column_count; j++)
        {
            char c = data[i][j];
            if(c == 'O')
            {
                for(int k = j - 1; k >= 0; k--)
                {
                    char next_position = data[i][k];
                    if(next_position == '.')
                    {
                        data[i][k] = 'O';
                        data[i][k+1] = '.';
                    }
                    else
                        break;
                }
            }            
        }
    }
}


void tilt_east(list* grid)
{
    char** data = (char **) grid->data;
    int column_count = strlen(data[0]);

    for(int i = 0; i < grid->count; i++)
    {
        for(int j = column_count - 1; j >= 0; j--)
        {
            char c = data[i][j];
            if(c == 'O')
            {
                for(int k = j + 1; k < grid->count; k++)
                {
                    char next_position = data[i][k];
                    if(next_position == '.')
                    {
                        data[i][k] = 'O';
                        data[i][k-1] = '.';
                    }
                    else
                        break;
                }
            }            
        }
    }
}


int calculate_load(list* grid)
{
    char** data = (char **) grid->data;
    int load = 0;
    for(int i = 0; i < grid->count; i++)
    {
        char* line = data[i];
        for(int j = 0; j < strlen(line); j++)
        {
            if(line[j] == 'O')
                load += grid->count - i;
        }
    }
    return load;
}

char* create_unique_string(list* grid)
{
    char** data = (char **) grid->data;
    size_t line_length = strlen(data[0]);
    char* out = malloc(grid->count * line_length + 1);
    memset(out, '\0', grid->count * line_length + 1);
    for(int i = 0; i < grid->count; i++)
    {
        char* line = data[i];
        strcat(out, line);
    }
    return out;
}


int main(int argc, char *argv[])
{
    int answer_part1 = 0;
    int answer_part2 = 0;

    list* l = read_lines("input.txt");
    list* known_grids = list_new();

    /* Save initial state. */
    char* grid_hash = create_unique_string(l);
    list_append(known_grids, grid_hash);

    /* Part 1 .*/
    tilt_north(l);
    answer_part1 = calculate_load(l);

    /* Finish 1st cycle. */
    tilt_west(l);
    tilt_south(l);
    tilt_east(l);

    grid_hash = create_unique_string(l);
    list_append(known_grids, grid_hash);

    /* Part 2. */
    bool cycle_found = false;
    int cycle_size = 0;
    int target_cycles = 1000000000;
    for(int i = 2; i <= target_cycles; i++)
    {
        tilt_north(l);
        tilt_west(l);
        tilt_south(l);
        tilt_east(l);

        if(!cycle_found)
        {
            grid_hash = create_unique_string(l);
            list_append(known_grids, grid_hash);
            char** hashes = (char **) known_grids->data;
            for(int j = 0; j < known_grids->count - 1; j++)
            {
                if(strcmp(hashes[j], grid_hash) == 0)
                {
                    cycle_found = true;
                    cycle_size = i - j;
                    break;
                }
            }

            if(cycle_found)
            {
                while(i + cycle_size < target_cycles)
                    i += cycle_size;
            }
        }
    }

    answer_part2 = calculate_load(l);

    printf("Day 14 part 1, result: %d\n", answer_part1);
    printf("Day 14 part 2, result: %d\n", answer_part2);

    list_delete(known_grids);
    list_delete(l);
    l = NULL;
}