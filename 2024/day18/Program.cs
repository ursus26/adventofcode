using System;

namespace day18
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            List<(int, int)> coordinates = new List<(int, int)>();
            foreach(string line in input)
            {
                string[] split = line.Split(",");
                int x = Convert.ToInt32(split[0]);
                int y = Convert.ToInt32(split[1]);
                coordinates.Add((x, y));
            }

            const int width = 71;
            const int height = 71;
            Dictionary<(int, int), char> grid = new Dictionary<(int, int), char>();
            for(int i = -1; i < width + 1; i++)
            {
                grid[(i, -1)] = '#';
                grid[(i, height)] = '#';
            }
            for(int i = -1; i < height + 1; i++)
            {
                grid[(-1, i)] = '#';
                grid[(width, i)] = '#';
            }

            /* Part 1 */
            for(int i = 0; i < 1024; i++)
            {
                (int, int) coord = coordinates[i];
                grid[coord] = '#';
            }
            long solutionPart1 = ShortestPath(grid, (0, 0), (width-1, height-1));
            Console.WriteLine("Day 18 part 1, result: " + solutionPart1);

            /* Part 2 */
            string solutionPart2 = String.Empty;
            foreach((int, int) coordinate in coordinates.Skip(1024))
            {
                grid[coordinate] = '#';
                int cost = ShortestPath(grid, (0, 0), (width-1, height-1));
                if(cost == Int32.MaxValue)
                {
                    solutionPart2 = coordinate.ToString();
                    break;
                }
            }
            Console.WriteLine("Day 18 part 2, result: " + solutionPart2);
        }

        static int ShortestPath(Dictionary<(int, int), char> grid, (int, int) start, (int, int) end)
        {
            PriorityQueue<(int, int), int> queue = new PriorityQueue<(int, int), int>();
            Dictionary<(int, int), int> distance = new Dictionary<(int, int), int>();

            queue.Enqueue(start, 0);
            distance[start] = 0;

            while(queue.Count != 0)
            {
                (int x, int y) node = queue.Dequeue();
                int cost = distance[node];

                if(node == end)
                    return cost;

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
                    }
                }
            }

            return Int32.MaxValue;
        }
    }
}