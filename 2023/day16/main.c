#include "list.h"
#include <stdio.h>
#include <string.h>
#include <stdbool.h>


typedef struct Beam
{
    int x;
    int y;
    int dir_x;
    int dir_y;
} Beam;


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


int beam_trace(list* l, Beam* initial_beam)
{
    /* Get grid data. */
    char** grid = (char **) l->data;
    int grid_height = l->count;
    int grid_width = strlen(grid[0]);

    /* Initialize visited nodes. */
    int** visited = (int **) malloc(grid_height * sizeof(int *));
    for(int i = 0; i < l->count; i++)
    {
        visited[i] = (int *) malloc(grid_width * sizeof(int));
        memset(visited[i], 0, grid_width * sizeof(int));
    }

    /* Setup initial conditions.*/
    list* beams = list_new();
    list_append(beams, initial_beam);

    while(beams->count > 0)
    {
        Beam *b = (Beam *) list_pop(beams);

        /* Handle beam position. */
        int x_next = b->x + b->dir_x;
        int y_next = b->y + b->dir_y;
        if(x_next < 0 || x_next >= grid_width || y_next < 0 || y_next >= grid_height)
        {
            free(b);
            continue;
        }
        b->x = x_next;
        b->y = y_next;

        /* Handle beam direction. */
        char c = grid[y_next][x_next];
        switch (c)
        {
        case '|':
            if(b->dir_x != 0)
            {
                if(visited[y_next][x_next])
                {
                    free(b);
                    break;
                }

                Beam* duplicate = (Beam *) malloc(sizeof(Beam));
                memcpy(duplicate, b, sizeof(Beam));
                duplicate->dir_x = 0;
                duplicate->dir_y = 1;
                list_append(beams, duplicate);

                b->dir_x = 0;
                b->dir_y = -1;
                list_append(beams, b);
            }
            else
            {
                list_append(beams, b);
            }
            break;
        
        case '-':
            if(b->dir_y != 0)
            {
                if(visited[y_next][x_next])
                {
                    free(b);
                    break;
                }

                Beam* duplicate = (Beam *) malloc(sizeof(Beam));
                memcpy(duplicate, b, sizeof(Beam));
                duplicate->dir_x = 1;
                duplicate->dir_y = 0;
                list_append(beams, duplicate);

                b->dir_x = -1;
                b->dir_y = 0;
                list_append(beams, b);
            }
            else
            {
                list_append(beams, b);
            }
            break;

        case '/':
            if(b->dir_x != 0)
            {
                b->dir_y = -1 * b->dir_x;
                b->dir_x = 0;
            }
            else
            {
                b->dir_x = -1 * b->dir_y;
                b->dir_y = 0;
            }
            list_append(beams, b);
            break;

        case '\\':
            if(b->dir_x != 0)
            {
                b->dir_y = b->dir_x;
                b->dir_x = 0;
            }
            else
            {
                b->dir_x = b->dir_y;
                b->dir_y = 0;
            }
            list_append(beams, b);
            break;

        default:
            list_append(beams, b);
            break;
        }

        /* Update visited set. */
        visited[y_next][x_next] = 1;
    }

    int out = 0;
    for(int i = 0; i < grid_height; i++)
        for(int j = 0; j < grid_width; j++)
            out += visited[i][j];

    
    for(int i = 0; i < grid_height; i++)
        free(visited[i]);
    free(visited);
    list_delete(beams);

    return out;
}

int main(int argc, char *argv[])
{
    int answer_part1 = 0;
    int answer_part2 = 0;

    list* l = read_lines("input.txt");
    int grid_width = strlen(l->data[0]);

    /* Part 1. */
    Beam* initial_beam = (Beam *) malloc(sizeof(Beam));
    initial_beam->x = -1;
    initial_beam->y = 0;
    initial_beam->dir_x = 1;
    initial_beam->dir_y = 0;
    answer_part1 = beam_trace(l, initial_beam);

    /* Part 2. */
    for(int i = 0; i < l->count; i++)
    {
        Beam* beam_part2 = (Beam *) malloc(sizeof(Beam));
        beam_part2->x = -1;
        beam_part2->y = i;
        beam_part2->dir_x = 1;
        beam_part2->dir_y = 0;
        int result = beam_trace(l, beam_part2);
        if(result > answer_part2)
            answer_part2 = result;

        beam_part2 = (Beam *) malloc(sizeof(Beam));
        beam_part2->x = grid_width;
        beam_part2->y = i;
        beam_part2->dir_x = -1;
        beam_part2->dir_y = 0;
        result = beam_trace(l, beam_part2);
        if(result > answer_part2)
            answer_part2 = result;
    }

    for(int i = 0; i < grid_width; i++)
    {
        Beam* beam_part2 = (Beam *) malloc(sizeof(Beam));
        beam_part2->x = i;
        beam_part2->y = -1;
        beam_part2->dir_x = 0;
        beam_part2->dir_y = 1;
        int result = beam_trace(l, beam_part2);
        if(result > answer_part2)
            answer_part2 = result;

        beam_part2 = (Beam *) malloc(sizeof(Beam));
        beam_part2->x = i;
        beam_part2->y = l->count;
        beam_part2->dir_x = 0;
        beam_part2->dir_y = -1;
        result = beam_trace(l, beam_part2);
        if(result > answer_part2)
            answer_part2 = result;
    }

    printf("Day 16 part 1, result: %d\n", answer_part1);
    printf("Day 16 part 2, result: %d\n", answer_part2);

    list_delete(l);
    l = NULL;
}