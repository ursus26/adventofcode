#include "list.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include <math.h>


char cards[] = "23456789TJQKA";

enum card_types {
    CARD_TWO = 2,
    CARD_THREE,
    CARD_FOUR,
    CARD_FIVE,
    CARD_SIX,
    CARD_SEVEN,
    CARD_EIGHT,
    CARD_NINE,
    CARD_T,
    CARD_J,
    CARD_Q,
    CARD_K,
    CARD_A
};

int char_to_card_type(char c)
{
    switch (c)
    {
    case '2':
        return CARD_TWO;
    case '3':
        return CARD_THREE;
    case '4':
        return CARD_FOUR;
    case '5':
        return CARD_FIVE;
    case '6':
        return CARD_SIX;
    case '7':
        return CARD_SEVEN;
    case '8':
        return CARD_EIGHT;
    case '9':
        return CARD_NINE;
    case 'T':
        return CARD_T;
    case 'J':
        return CARD_J;
    case 'Q':
        return CARD_Q;
    case 'K':
        return CARD_K;
    case 'A':
        return CARD_A;
    
    default:
        return 0;
    }
}

struct hand
{
    int cards[5];
    int bid;
    enum Hand_Type {HIGH_CARD, ONE_PAIR , TWO_PAIR, THREE_OF_A_KIND, \
                    FULL_HOUSE, FOUR_OF_A_KIND, FIVE_OF_A_KIND} type;
};


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


void set_hand_type(struct hand *h)
{
    int card_count[16] = { 0 };
    for(int i = 0; i < 5; i++)
        card_count[h->cards[i]]++;

    int pair = 0;
    bool three_of_a_kind = false;
    bool four_of_a_kind = false;
    bool five_of_a_kind = false;
    for(int i = 0; i < sizeof(cards); i++)
    {
        int card = char_to_card_type(cards[i]);
        int count = card_count[card];

        if(count == 2)
            pair++;
        else if(count == 3)
            three_of_a_kind = true;
        else if(count == 4)
            four_of_a_kind = true;
        else if(count == 5)
            five_of_a_kind = true;
    }

    if(five_of_a_kind)
        h->type = FIVE_OF_A_KIND;
    else if(four_of_a_kind)
        h->type = FOUR_OF_A_KIND;
    else if(three_of_a_kind && pair == 1)
        h->type = FULL_HOUSE;
    else if(three_of_a_kind)
        h->type = THREE_OF_A_KIND;
    else if(pair == 2)
        h->type = TWO_PAIR;
    else if(pair == 1)
        h->type = ONE_PAIR;
    else
        h->type = HIGH_CARD;
}


void replace_joker_cards(struct hand *h)
{
    /* Number of jokers to replace. */
    int joker_count = 0;
    for(int i = 0; i < 5; i++)
        if(h->cards[i] == CARD_J)
            joker_count++;
    
    if(joker_count == 0)
        return;

    if(joker_count == 4 || joker_count == 5)
    {
        h->type = FIVE_OF_A_KIND;

        for(int i = 0; i < 5; i++)
            if(h->cards[i] == CARD_J)
                h->cards[i] = CARD_TWO - 1;
        return;
    }

    /* Create template new hand for evaluating permuations. */
    struct hand new_hand;
    for(int i = 0; i < 5; i++)
        new_hand.cards[i] = h->cards[i];
    new_hand.bid = new_hand.bid;
    new_hand.type = new_hand.type;

    int replacement_cards[] = {CARD_TWO, CARD_THREE, CARD_FOUR, CARD_FIVE, CARD_SIX, CARD_SEVEN,
                                CARD_EIGHT, CARD_NINE, CARD_T, CARD_Q, CARD_K, CARD_A};
    const int replacement_count = sizeof(replacement_cards) / sizeof(replacement_cards[0]);
    int permutation_count = replacement_count;
    for(int j = 1; j < joker_count; j++)
        permutation_count *= permutation_count;

    int permutation[5] = { 0, 0, 0, 0, 0 };

    for(int j = 0; j < permutation_count; j++)
    {
        /* Apply permutation on new hand. */
        int jokers_replaced = 0;
        for(int k = 0; k < 5; k++)
        {
            if(h->cards[k] == CARD_J)
            {
                new_hand.cards[k] = replacement_cards[permutation[jokers_replaced]];
                jokers_replaced++;
            }
        }

        /* Evaluate permuation hand and check if is better than original. */
        set_hand_type(&new_hand);
        if(new_hand.type > h->type)
            h->type = new_hand.type;

        /* Update permuation list. */
        for(int k = 0; k < joker_count; k++)
        {
            permutation[k] = (permutation[k] + 1) % replacement_count;
            if(permutation[k] != 0)
                break;
        }
    }

    for(int j = 0; j < 5; j++)
    {
        if(h->cards[j] == CARD_J)
            h->cards[j] = CARD_TWO - 1;
    }
}


void insertion_sort_cammel_cards(struct hand *h, int size)
{
    for(int i = 1; i < size; i++)
    {
        struct hand selected_hand = h[i];

        for(int j = i - 1; j >= 0; j--)
        {
            struct hand previous_hand = h[j];
            if(selected_hand.type > previous_hand.type)
                break;
            else if(selected_hand.type < previous_hand.type)
            {
                h[j+1] = previous_hand;
                h[j] = selected_hand;
                continue;
            }
            else
            {
                bool swapped = false;
                for(int k = 0; k < 5; k++)
                {
                    if(selected_hand.cards[k] == previous_hand.cards[k])
                        continue;
                    else if(selected_hand.cards[k] < previous_hand.cards[k])
                    {
                        h[j+1] = previous_hand;
                        h[j] = selected_hand;
                        swapped = true;
                        break;
                    }
                    else
                        break;
                }

                if(!swapped)
                    break;
            }
        }
    }
}


int main(int argc, char *argv[])
{
    long long int answer_part1 = 0;
    int answer_part2 = 0;

    list* l = read_lines("input.txt");
    int hand_count = l->count;
    struct hand *h = (struct hand *) malloc(hand_count * sizeof(struct hand));

    /*Setup hands. */
    for(int i = 0; i < hand_count; i++)
    {
        char *hand_data = (char *) l->data[i];
        for(int j = 0; j < 5; j++)
            h[i].cards[j] = char_to_card_type(hand_data[j]);
            
        h[i].bid = strtol(hand_data + 5, NULL, 10);
        set_hand_type(&(h[i]));
    }

    /* Part 1. */
    insertion_sort_cammel_cards(h, hand_count);

    for(int i = 0; i < hand_count; i++)
    {
        struct hand selected_hand = h[i];
        answer_part1 += (selected_hand.bid * (i + 1));
    }

    /* Part 2. */
    for(int i = 0; i < hand_count; i++)
        replace_joker_cards(&(h[i]));
    insertion_sort_cammel_cards(h, hand_count);

    for(int i = 0; i < hand_count; i++)
    {
        struct hand selected_hand = h[i];
        answer_part2 += (selected_hand.bid * (i + 1));
    }

    printf("Day 07 part 1, result: %lld\n", answer_part1);
    printf("Day 07 part 2, result: %d\n", answer_part2);

    list_delete(l);
    l = NULL;
    free(h);
}