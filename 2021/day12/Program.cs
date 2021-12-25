using System;
using System.IO;
using System.Collections.Generic;

namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("input.txt");
            Dictionary<string, Node> graph =  new Dictionary<string, Node>();
            foreach(string line in data)
            {
                string[] names = line.Split('-');
                if(!graph.ContainsKey(names[0]))
                    graph[names[0]] = new Node(names[0]);
                if(!graph.ContainsKey(names[1]))
                    graph[names[1]] = new Node(names[1]);

                Node n1 = graph[names[0]];
                Node n2 = graph[names[1]];

                n1.addNeighbour(n2);
                n2.addNeighbour(n1);
            }

            int solutionPart1 = part1(graph);
            Console.WriteLine("Day 12 part 1, result: " + solutionPart1);

            int solutionPart2 = part2(graph);
            Console.WriteLine("Day 12 part 2, result: " + solutionPart2);

        }

        static int part1(Dictionary<string, Node> graph)
        {
            Stack<(Node, List<Node>)> stack = new Stack<(Node, List<Node>)>();
            stack.Push((graph["start"], new List<Node>()));
            int solutionPart1 = 0;
            while(true)
            {
                if(stack.Count == 0)
                    break;
                (Node current, List<Node> path) elem = stack.Pop();
                Node current = elem.current;
                List<Node> path = elem.path;
                path.Add(current);

                if(current.name == "end")
                {
                    solutionPart1++;
                    continue;
                }

                foreach(Node neighbour in current.neighbours)
                {
                    if(neighbour.largeCave || (!path.Contains(neighbour) && !neighbour.largeCave))
                        stack.Push((neighbour, new List<Node>(path)));
                }
            }
            return solutionPart1;
        }

        static int part2(Dictionary<string, Node> graph)
        {
            Stack<(Node, List<Node>, bool)> stack = new Stack<(Node, List<Node>, bool)>();
            stack.Push((graph["start"], new List<Node>(), false));
            int solutionPart2 = 0;
            while(true)
            {
                if(stack.Count == 0)
                    break;
                (Node current, List<Node> path, bool visitedTwice) elem = stack.Pop();
                Node current = elem.current;
                List<Node> path = elem.path;
                bool visitedTwice = elem.visitedTwice;
                path.Add(current);

                if(current.name == "end")
                {
                    solutionPart2++;
                    continue;
                }

                foreach(Node neighbour in current.neighbours)
                {
                    if(neighbour.name == "start")
                        continue;

                    if(neighbour.largeCave || (!path.Contains(neighbour) && !neighbour.largeCave))
                        stack.Push((neighbour, new List<Node>(path), visitedTwice));
                    else if((path.Contains(neighbour) && !neighbour.largeCave && !visitedTwice))
                        stack.Push((neighbour, new List<Node>(path), true));
                }
            }
            return solutionPart2;
        }
    }

    class Node
    {
        public string name { get; }
        public List<Node> neighbours { get; }
        public bool visited { get; set; }
        public bool largeCave { get; set; }

        public Node(string nodeName)
        {
            this.name = nodeName;
            this.neighbours = new List<Node>();
            this.visited = false;
            this.largeCave = Char.IsUpper(nodeName[0]) ? true : false;
        }

        public void addNeighbour(Node n)
        {
            this.neighbours.Add(n);
        }
    }
}
