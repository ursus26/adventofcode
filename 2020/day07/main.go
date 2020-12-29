package main

import (
	"container/list"
	"fmt"
	"io/ioutil"
	"log"
	"os"
	"strconv"
	"strings"
)

/* Graph node with directed edges. inEdges go into this node and outEdges go out of this node. */
type GraphNode struct {
	name     string
	inEdges  []DirectedEdge
	outEdges []DirectedEdge
}

/* Directed edge from in to out and its associated cost. */
type DirectedEdge struct {
	in   *GraphNode
	out  *GraphNode
	cost int
}

func NewGraphNode(name string) *GraphNode {
	node := new(GraphNode)
	node.name = name
	node.inEdges = []DirectedEdge{}
	node.outEdges = []DirectedEdge{}
	return node
}

/* DFS on out going edges of a bag. */
func (node *GraphNode) BagCount() int {
	count := 0
	for _, edge := range node.outEdges {
		count += edge.cost + edge.cost*edge.out.BagCount()
	}
	return count
}

func main() {
	var file, err = os.Open("input.txt")
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	/* Data parsing. */
	bytes, err := ioutil.ReadAll(file)
	data := strings.Split(strings.Trim(string(bytes), "\n"), "\n")
	graph := make(map[string]*GraphNode)

	for _, line := range data {
		/* Parsing */
		split := strings.Split(line, " bags contain ")
		outerBag := split[0]
		innerBags := strings.Split(strings.Trim(string(split[1]), "."), ", ")

		outerNode, ok := graph[outerBag]
		if !ok {
			outerNode = NewGraphNode(outerBag)
			graph[outerBag] = outerNode
		}

		for _, elem := range innerBags {
			if elem == "no other bags" {
				continue
			}

			/* Parsing */
			tmp := strings.Split(elem, " bag")[0]
			tmp2 := strings.SplitN(tmp, " ", 2)
			count, _ := strconv.Atoi(tmp2[0])
			bagType := tmp2[1]

			/* Fetch or create the inner node. */
			innerNode, ok := graph[bagType]
			if !ok {
				innerNode = NewGraphNode(bagType)
				graph[bagType] = innerNode
			}

			/* Create the edge. */
			edge := DirectedEdge{outerNode, innerNode, count}
			outerNode.outEdges = append(outerNode.outEdges, edge)
			innerNode.inEdges = append(innerNode.inEdges, edge)
		}
	}

	/* Part 1 */
	queue := list.New()
	queue.PushBack(graph["shiny gold"])
	bagCountPart1 := 0
	countedBags := make(map[string]bool)
	for e := queue.Front(); e != nil; e = e.Next() {
		node := e.Value.(*GraphNode)

		for _, edge := range node.inEdges {
			_, ok := countedBags[edge.in.name]
			if !ok {
				countedBags[edge.in.name] = true
				bagCountPart1 += 1
				queue.PushBack(edge.in)
			}
		}
	}

	/* Part 2 */
	bagCountPart2 := graph["shiny gold"].BagCount()

	fmt.Printf("Day 7 part 1, answer: %d\n", bagCountPart1)
	fmt.Printf("Day 7 part 2, answer: %d\n", bagCountPart2)
}
