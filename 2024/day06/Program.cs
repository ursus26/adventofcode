namespace day06
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            int gridHeight = lines.Length;
            int gridWidth = lines[0].Length;

            /* Find start position. */
            int xStart = 0;
            int yStart = 0;
            foreach(string line in lines)
            {
                xStart = line.IndexOf('^', 0);
                if(xStart != -1)
                    break;
                yStart++;
            }

            /* Part 1 */
            int x = xStart;
            int y = yStart;
            int dx = 0;
            int dy = -1;
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            while(x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
            {
                char c = lines[y][x];
                if(c == '#')
                {
                    /* Revert to previous step. */
                    x -= dx;
                    y -= dy;

                    /* Turn 90 degrees clockwise. */
                    int tmp = dx;
                    dx = -dy;
                    dy = tmp;
                }
                else
                {
                    visited.Add((x, y));
                }

                /* Update position. */
                x += dx;
                y += dy;
            }

            int solutionPart1 = visited.Count;
            Console.WriteLine("Day 06 part 1, result: " + solutionPart1);

            /* Part 2
             * Only add obstacles on visited nodes from part 1. */
            int solutionPart2 = 0;
            foreach((int xVisited, int yVisited) in visited)
            {
                if(lines[yVisited][xVisited] == '^')
                    continue;

                char[] arr = lines[yVisited].ToCharArray();
                arr[xVisited] = '#';

                string oldLine = lines[yVisited];
                string newLine = new string(arr);

                lines[yVisited] = newLine;
                if(CheckForCycle(lines, xStart, yStart, 0, -1))
                    solutionPart2++;
                lines[yVisited] = oldLine;
            }

            Console.WriteLine("Day 06 part 2, result: " + solutionPart2);
        }

        static bool CheckForCycle(string[] grid, int x, int y, int dx, int dy)
        {
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            HashSet<(int, int, int, int)> cycleDetection = new HashSet<(int, int, int, int)>();
            cycleDetection.Add((x, y, dx, dy));

            int gridHeight = grid.Length;
            int gridWidth = grid[0].Length;
            while(x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
            {
                char c = grid[y][x];
                if(c == '#')
                {
                    x -= dx;
                    y -= dy;

                    int tmp = dx;
                    dx = -dy;
                    dy = tmp;
                }
                else
                {
                    visited.Add((x, y));
                    cycleDetection.Add((x, y, dx, dy));
                }

                x += dx;
                y += dy;

                if(cycleDetection.Contains((x, y, dx, dy)))
                    return true;
            }

            return false;
        }
    }
}
