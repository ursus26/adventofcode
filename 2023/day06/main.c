#include "list.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include <math.h>


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
    int answer_part1 = 1;
    char* nptr_time = (char *) l->data[0] + 5;
    char* nptr_distance = (char *) l->data[1] + 9;

    while(*nptr_time != '\0' || *nptr_distance != '\0')
    {
        /* Parsing numbers. */
        char* endptr_time;
        int time = (int) strtol(nptr_time, &endptr_time, 10);
        if(endptr_time == nptr_time)
        {
            fprintf(stderr, "Error cannot parse time value.\n");
            break;
        }
        nptr_time = endptr_time;

        char* endptr_distance;
        int distance_record = (int) strtol(nptr_distance, &endptr_distance, 10);
        if(endptr_distance == nptr_distance)
        {
            fprintf(stderr, "Error cannot parse distance value.\n");
            break;
        }
        nptr_distance = endptr_distance;

        int record_breaking_count = 0;
        for(int i = 0; i < time; i++)
        {
            int distance = i * (time - i);

            if(distance > distance_record)
                record_breaking_count++;
        }
        answer_part1 *= record_breaking_count;       
    }
    printf("Day 06 part 1, result: %d\n", answer_part1);

    /* Part 2 */
    int answer_part2 = 0;
    long int time_part2 = 52947594;
    long int distance_part2 = 426137412791216;

    for(long int i = 0; i < time_part2; i++)
    {
        long int distance = i * (time_part2 - i);
        /**
         * distance(i) = -i^2 + time_part2 * i
         * 
         * record = distance(i)
         * record = -i^2 + time_part2 * i
         * -i^2 + time_part2 * i - record = 0
         * i^2 - time_part2 * i + record = 0
         **/

        if(distance > distance_part2)
            answer_part2++;
    }
    printf("Day 06 part 2, result: %d\n", answer_part2);

    list_delete(l);
    l = NULL;
}