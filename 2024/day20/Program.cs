using System;
using System.Text.RegularExpressions;

namespace day20
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            Dictionary<(int, int), char> grid = new Dictionary<(int, int), char>();
            (int, int) start = (0, 0);
            (int, int) end = (0, 0);
            for(int y = 0; y < input.Length; y++)
            {
                string line = input[y];
                for(int x = 0; x < line.Length; x++)
                {
                     char c = line[x];
                    if(c == 'S')
                    {
                        c = '.';
                        start = (x, y);
                    }
                    else if(c == 'E')
                    {
                        c = '.';
                        end = (x, y);
                    }
                    grid[(x, y)] = c;
                }
            }

            (Dictionary<(int, int), int> dist, Dictionary<(int, int), (int, int)> prev) result = ShortestPath(grid, start, end);
            Dictionary<(int, int), int> distance = result.dist;
            Dictionary<(int, int), (int, int)> prev = result.prev;

            /* Walk backwards from the path and attempt to cheat along the way. */
            long solutionPart1 = 0;
            long solutionPart2 = 0;
            (int x, int y) current = end;
            do
            {
                current = prev[current];

                /* Create a list of neighbouring nodes. */
                List<(int, int)> neighbours = new List<(int, int)>();
                for(int i = current.x - 20; i <= current.x + 20; i++)
                {
                    for(int j = current.y - 20; j <= current.y + 20; j++)
                    {
                        (int, int) cheatNode = (i, j);
                        if(!grid.ContainsKey(cheatNode))
                            continue;
                        
                        if(grid[cheatNode] == '.')
                            neighbours.Add(cheatNode);
                    }
                }

                /* Check if we can cheat towards a neighbouring node and if it saves enough time. */
                foreach((int x, int y) cheatStart in neighbours)
                {
                    int manhattanDistance = ManhattanDistance(current, cheatStart);
                    if(manhattanDistance > 20)
                        continue;

                    int newCost = distance[current] + manhattanDistance + distance[end] - distance[cheatStart];
                    if(distance[end] - newCost >= 100)
                    {
                        if(manhattanDistance == 2)
                            solutionPart1++;
                        solutionPart2++;
                    }
                }

            } while(prev.ContainsKey(current));

            Console.WriteLine("Day 20 part 1, result: " + solutionPart1);
            Console.WriteLine("Day 20 part 2, result: " + solutionPart2);
        }

        static (Dictionary<(int, int), int>, Dictionary<(int, int), (int, int)>) ShortestPath(Dictionary<(int, int), char> grid, (int, int) start, (int, int) end)
        {
            PriorityQueue<(int, int), int> queue = new PriorityQueue<(int, int), int>();
            Dictionary<(int, int), int> distance = new Dictionary<(int, int), int>();
            Dictionary<(int, int), (int, int)> prev = new Dictionary<(int, int), (int, int)>();

            queue.Enqueue(start, 0);
            distance[start] = 0;

            while(queue.Count != 0)
            {
                (int x, int y) node = queue.Dequeue();
                int cost = distance[node];

                if(node == end)
                    break;

                List<(int, int)> neighbours = new List<(int, int)> {(node.x - 1, node.y), (node.x + 1, node.y), (node.x, node.y - 1), (node.x, node.y + 1)};
                foreach((int x, int y) neighbour in neighbours)
                {
                    if(!grid.ContainsKey(neighbour))
                        grid.Add(neighbour, '.');

                    char space = grid[neighbour];
                    if(space == '#')
                        continue;

                    if(!distance.ContainsKey(neighbour))
                        distance.Add(neighbour, Int32.MaxValue);
                    
                    if(cost + 1 < distance[neighbour])
                    {
                        distance[neighbour] = cost + 1;
                        queue.Enqueue(neighbour, cost + 1);
                        prev[neighbour] = node;
                    }
                }
            }

            return (distance, prev);
        }

        static int ManhattanDistance((int x, int y) n, (int x, int y) m)
        {
            return Math.Abs(n.x - m.x) + Math.Abs(n.y - m.y);
        }
    }
}
