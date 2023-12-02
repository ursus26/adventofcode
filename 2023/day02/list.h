#include <stdlib.h>

#ifndef LIST_H
#define LIST_H

typedef struct
{
    size_t count;
    size_t capacity;
    void** data;
} list;


list* list_new();
void list_delete(list* l);
void list_clear(list* l);
void list_append(list* l, void* new_elem);
void* list_pop(list* l);


#endif /* LIST_H */