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


int partition(int** array, int idx_low, int idx_high)
{
    int pivot = *array[idx_high];
    int i = idx_low - 1;

    for(int j = idx_low; j < idx_high; j++)
    {
        if(*array[j] <= pivot)
        {
            i++;
            int tmp = *array[i];
            *array[i] = *array[j];
            *array[j] = tmp;
        }
    }

    i++;
    int tmp = *array[i];
    *array[i] = *array[idx_high];
    *array[idx_high] = tmp;

    return i;
}


void quick_sort(int** array, int idx_low, int idx_high)
{
    if(idx_low >= idx_high || idx_low < 0)
        return;

    int idx_pivot = partition(array, idx_low, idx_high);

    quick_sort(array, idx_low, idx_pivot - 1);
    quick_sort(array, idx_pivot + 1, idx_high);
}


void sort_list(list* l)
{
    int** data = (int **) l->data;
    quick_sort(data, 0, l->count - 1);
}


int main(int argc, char *argv[])
{
    int answer_part1 = 0;
    int answer_part2 = 0;

    list* l = read_lines("input.txt");
    int* card_count = malloc(sizeof(int) * l->count);
    for(int i = 0; i < l->count; i++)
        card_count[i] = 1;

    for(int i = 0; i < l->count; i++)
    {
        /**
         * Parsing winning numbers.
         */
        char* start_winning_numbers = strchr((char *) l->data[i], ':');
        if(start_winning_numbers == NULL)
        {
            fprintf(stderr, "Error on input line %d, cannot find start of winning numbers.\n", i+1);
            exit(EXIT_FAILURE);
        }

        list* winning_numbers = list_new();        
        char* nptr = start_winning_numbers + 1;
        while(*nptr != '\0' || *nptr != '|')
        {
            char* endptr;
            int value = (int) strtol(nptr, &endptr, 10);
            if(endptr == nptr)
                break;

            int* v = malloc(sizeof(int));
            *v = value;
            list_append(winning_numbers, v);

            nptr = endptr;
        }

        /**
         * Parsing numbers we have.
         */
        char* start_my_numbers = strchr((char *) l->data[i], '|');
        if(start_my_numbers == NULL)
        {
            fprintf(stderr, "Error on input line %d, cannot find start of winning numbers.\n", i+1);
            exit(EXIT_FAILURE);
        }

        list* my_numbers = list_new();        
        nptr = start_my_numbers + 1;
        while(*nptr != '\0' || *nptr != '|')
        {
            char* endptr;
            long value = strtol(nptr, &endptr, 10);
            if(endptr == nptr)
                break;

            long* v = malloc(sizeof(long));
            *v = value;
            list_append(my_numbers, v);

            nptr = endptr;
        }

        /**
         * Compare winning numbers and my numbers. Both list are sorted.
         */
        sort_list(winning_numbers);
        sort_list(my_numbers);
        int k = 0;
        int count = 0;
        for(int j = 0; j < my_numbers->count;)
        {
            if(k >= winning_numbers->count)
                break;

            int my_number = *(int *) my_numbers->data[j];
            int winning_number = *(int *) winning_numbers->data[k];

            if(my_number == winning_number)
            {
                count++;
                j++;
                k++;
            }
            else if(my_number < winning_number)
            {
                j++;
            }
            else
            {
                k++;
            }
        }

        list_delete(winning_numbers);
        list_delete(my_numbers);

        /* Part 1 - power of 2. */
        answer_part1 += ((1 << count) >> 1);

        /* Part 2 */
        for(int j = i+1; j < i + 1 + count; j++)
            card_count[j] += card_count[i];        
    }

    /* Sum card count for part 2 answer. */
    for(int i = 0; i < l->count; i++)
        answer_part2 += card_count[i];

    printf("Day 04 part 1, result: %d\n", answer_part1);
    printf("Day 04 part 2, result: %d\n", answer_part2);

    list_delete(l);
    l = NULL;

    free(card_count);
}