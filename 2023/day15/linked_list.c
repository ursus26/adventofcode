#include "linked_list.h"


ll_node* ll_new()
{
    ll_node* out = malloc(sizeof(ll_node));
    if(out == NULL)
        return NULL;

    out->data = NULL;
    out->next = NULL;
    return out;
}


ll_node* ll_new_node(void* data)
{
    ll_node* out = ll_new();
    out->data = data;
    out->next = NULL;
    return out;
}


void ll_insert_begin(ll_node* head, ll_node* new_node)
{
    new_node->next = head->next;
    head->next = new_node;
}


void ll_insert_end(ll_node* head, ll_node* new_node)
{
    if(head == NULL)
        return;

    if(head->next == NULL)
    {
        head->next = new_node;
        return;
    }

    ll_node* node = head->next;
    while(node->next != NULL)
        node = node->next;

    node->next = new_node;
}


bool ll_remove_node(ll_node* head, ll_node* remove_node)
{
    if(head == NULL || head->next == NULL)
        return false;

    ll_node* node = head->next;
    if(node == remove_node)
    {
        head->next = node->next;
        free(remove_node);
        return true;
    }

    ll_node* prev = node;
    node = node->next;
    while(node != NULL)
    {
        if(node == remove_node)
        {
            prev->next = node->next;
            free(node);
            return true;
        }
        else
        {
            prev = node;
            node = node->next;
        }
    }

    return false;
}