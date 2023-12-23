#include <stdlib.h>
#include <stdbool.h>

#ifndef LINKED_LIST_H
#define LINKED_LIST_H

typedef struct ll_node ll_node;

struct ll_node
{
    void* data;
    ll_node* next;
};


ll_node* ll_new();
ll_node* ll_new_node(void* data);
void ll_insert_begin(ll_node* node, ll_node* new_node);
void ll_insert_end(ll_node* head, ll_node* new_node);
bool ll_remove_node(ll_node* head, ll_node* remove_node);


#endif /* LINKED_LIST_H */