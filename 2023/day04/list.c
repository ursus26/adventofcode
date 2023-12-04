#include "list.h"
#include <assert.h>
#include <string.h>
#include <stdio.h>


const size_t DEFAULT_CAPACITY = 64;


void _double_capacity(list* l);


list* list_new()
{
    list* l = malloc(sizeof(list));
    l->data = malloc(sizeof(void*) * DEFAULT_CAPACITY);
    l->capacity = DEFAULT_CAPACITY;
    l->count = 0;
    return l;
}


void list_delete(list* l)
{
    assert(l != NULL);

    list_clear(l);
    free(l->data);
    free(l);
}


void list_clear(list* l)
{
    assert(l != NULL);

    for(int i = 0; i < l->count; i++)
    {
        free(l->data[i]);
    }

    l->count = 0;
}


void list_append(list* l, void* new_elem)
{
    assert(l != NULL);

    if(l->count == l->capacity)
        _double_capacity(l);

    l->data[l->count] = new_elem;
    l->count++;
}


/**
 * Remove and return the last item of the list.
*/
void* list_pop(list* l)
{
    assert(l != NULL);
    assert(l->count > 0);

    l->count--;
    return l->data[l->count];
}


/**
 * Doubles the capacity of a list by creating a new larger buffer and copying over items to the new list.
*/
void _double_capacity(list* l)
{
    void* new_buffer = malloc(sizeof(void*) * 2 * l->capacity);
    memcpy(new_buffer, l->data, sizeof(void*) * l->count);
    free(l->data);
    l->data = new_buffer;
    l->capacity = 2 * l->capacity;
}