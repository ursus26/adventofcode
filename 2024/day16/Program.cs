namespace day16
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> input = File.ReadAllLines("input.txt").ToList();

            Dictionary<(int, int), char> grid = new Dictionary<(int, int), char>();
            (int x, int y) start = (0, 0);
            (int x, int y) end = (0, 0);

            for(int y = 0; y < input.Count; y++)
            {
                for(int x = 0; x < input[0].Length; x++)
                {
                    if(input[y][x] == 'S')
                    {
                        start = (x, y);
                        grid[(x, y)] = '.';
                    }
                    else if(input[y][x] == 'E')
                    {
                        end = (x, y);
                        grid[(x, y)] = '.';
                    }
                    else
                    {
                        grid[(x, y)] = input[y][x];
                    }
                }
            }

            (int part1, int part2) solution = ShortestPath(grid, start, end);

            /* Part 1 */
            Console.WriteLine("Day 16 part 1, result: " + solution.part1);

            /* Part 2 */
            Console.WriteLine("Day 16 part 2, result: " + solution.part2);
        }


        static (int, int) ShortestPath(Dictionary<(int, int), char> grid, (int x, int y) start, (int x, int y) end)
        {
            /**
             * Direction 0: east
             * Direction 1: north
             * Direction 2: west
             * Direction 3: south
             */
            List<(int dx, int dy)> dxdy = new List<(int dx, int dy)> { (1, 0), (0, -1), (-1, 0), (0, 1) };

            PriorityQueue<(int, int, int), int> queue = new PriorityQueue<(int, int, int), int>();
            Dictionary<(int, int, int), int> distance = new Dictionary<(int, int, int), int>();
            Dictionary<(int, int, int), List<(int, int, int)>> prev = new Dictionary<(int, int, int), List<(int, int, int)>>();

            queue.Enqueue((start.x, start.y, 0), 0);
            distance[(start.x, start.y, 0)] = 0;
            prev[(start.x, start.y, 0)] = new List<(int, int, int)>();

            int outputCost = Int32.MaxValue;
            (int x, int y, int dir) outputNode = (-1, -1, -1);

            while(queue.Count != 0)
            {
                (int x, int y, int dir) node = queue.Dequeue();
                int cost = distance[node];

                if(node.x == end.x && node.y == end.y)
                {
                    outputCost = cost;
                    outputNode = node;
                    break;
                }

                List<(int, int, int, int)> neighbours = new List<(int, int, int, int)> {
                    (node.x + dxdy[node.dir].dx, node.y + dxdy[node.dir].dy, node.dir, cost + 1),
                    (node.x, node.y, (node.dir + 1) % 4, cost + 1000),
                    (node.x, node.y, (node.dir + 3) % 4, cost + 1000)
                };
                foreach((int x, int y, int dir, int cost) neighbour in neighbours)
                {
                    char space = grid[(neighbour.x, neighbour.y)];
                    if(space == '#')
                        continue;

                    (int, int, int) coord = (neighbour.x, neighbour.y, neighbour.dir);
                    if(!distance.ContainsKey(coord))
                        distance.Add(coord, Int32.MaxValue);
                    
                    if(neighbour.cost < distance[coord])
                    {
                        distance[coord] = neighbour.cost;
                        queue.Enqueue(coord, neighbour.cost);
                        prev[coord] = new List<(int, int, int)>{ node };
                    }
                    else if(neighbour.cost == distance[coord])
                    {
                        prev[coord].Add(node);
                    }
                }
            }

            return (outputCost, CountTilesOnBestPaths(prev, outputNode));
        }

        static int CountTilesOnBestPaths(Dictionary<(int, int, int), List<(int, int, int)>> prev, (int, int, int) end)
        {
            HashSet<(int, int)> tiles = new HashSet<(int, int)>();
            Stack<(int, int, int)> stack = new Stack<(int, int, int)>();
            stack.Push(end);
            while(stack.Count != 0)
            {
                (int x, int y, int dir) node = stack.Pop();
                tiles.Add((node.x, node.y));
                foreach((int x, int y, int dir) prevNode in prev[node])
                {
                    stack.Push(prevNode);
                }
            }

            return tiles.Count;
        }
    }
}
