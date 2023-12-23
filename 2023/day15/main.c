// #include "list.h"
#include "linked_list.h"
#include <stdio.h>
#include <string.h>
#include <stdbool.h>


typedef struct
{
    char* label;
    int focal_length;
} Lens;


char* read_file(char* file_name)
{
    FILE* file = fopen(file_name, "r");
    if(file == NULL)
    {
        fprintf(stderr, "Error cannot open file [%s]\n", file_name);
        exit(EXIT_FAILURE);
    }

    /* Get file size. */
    if(fseek(file, 0, SEEK_END) != 0)
    {
        printf("Seeking error!\n");
    }
    long file_size = ftell(file);
    rewind(file);

    /* Get file content. */
    char* ret_val = malloc(file_size + 1);
    memset(ret_val, '\0', file_size + 1);
    for(int i = 0; i < file_size; i++)
        ret_val[i] = fgetc(file);

    fclose(file);
    return ret_val;
}


int holiday_ASCII_string_helper(char* input)
{
    int current_value = 0;
    for(int i = 0; i < strlen(input); i++)
    {
        char c = input[i];
        current_value = (17 * (current_value + c)) % 256;
    }
    return current_value;
}


ll_node* find_lens(ll_node* head, char* label)
{
    if(head == NULL || head->next == NULL)
        return NULL;

    ll_node* node = head->next;
    while(node != NULL)
    {
        Lens *l = (Lens *) node->data;
        if(strcmp(l->label, label) == 0)
            return node;
        else
            node = node->next;
    }
    return NULL;
}


int main(int argc, char *argv[])
{
    int answer_part1 = 0;
    int answer_part2 = 0;

    ll_node* buckets[256];
    for(int i = 0; i < 256; i++)
        buckets[i] = ll_new();

    char* text = read_file("input.txt");
    char* saveptr;
    for(char* str = text; ; str = NULL)
    {
        char* token = strtok_r(str, ",", &saveptr);
        if(token == NULL)
            break;

        int hash_value = holiday_ASCII_string_helper(token);
        answer_part1 += hash_value;

        int token_lenght = strlen(token);
        if(token[token_lenght - 1] == '-')
        {
            token[token_lenght - 1] = '\0';
            int idx_bucket = holiday_ASCII_string_helper(token);

            ll_node* lens_node = find_lens(buckets[idx_bucket], token);
            if(lens_node)
            {
                Lens* l = (Lens *) lens_node->data;
                free(l->label);
                free(l);
                ll_remove_node(buckets[idx_bucket], lens_node);
            }
        }
        else
        {
            token[token_lenght - 2] = '\0';
            int idx_bucket = holiday_ASCII_string_helper(token);
            int focal_length = strtol(token + token_lenght - 1, NULL, 10);

            ll_node* lens_node = find_lens(buckets[idx_bucket], token);
            if(lens_node != NULL)
            {
                /* Replace focal length. */
                Lens* l = (Lens *) lens_node->data;
                l->focal_length = focal_length;
            }
            else
            {
                /* Insert new node. */
                Lens* l = malloc(sizeof(Lens));
                l->label = malloc(token_lenght + 1);
                strcpy(l->label, token);
                l->focal_length = focal_length;

                ll_node* new_node = ll_new_node(l);
                ll_insert_end(buckets[idx_bucket], new_node);
            }
        }
    }

    /* Calculate score for part 2. */
    for(int i = 0; i < 256; i++)
    {
        ll_node* head = buckets[i];
        if(head->next == NULL)
            continue;
        
        ll_node* n = head->next;
        int idx_slot = 1;
        while(n != NULL)
        {
            Lens* l = (Lens *) n->data;
            answer_part2 += (i + 1) * idx_slot * l->focal_length;

            n = n->next;
            idx_slot++;
        }
    }

    printf("Day 15 part 1, result: %d\n", answer_part1);
    printf("Day 15 part 2, result: %d\n", answer_part2);

    /* Clean up. */
    for(int i = 0; i < 256; i++)
    {
        ll_node* head = buckets[i];
        while(head->next != NULL)
        {
            Lens* l = (Lens *) head->next->data;
            free(l->label);
            free(l);
            ll_remove_node(head, head->next);
        }

        free(head);
        buckets[i] = NULL;
    }
    free(text);
}