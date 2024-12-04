namespace day04
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");

            int width = lines[0].Length;
            string padding = new string('.', width + 6);

            /* Create a grid with a padded border to make border checks safer. */
            List<string> grid = new List<string>();
            grid.Add(padding);
            grid.Add(padding);
            grid.Add(padding);
            foreach(string line in lines)
                grid.Add("..." + line + "...");
            grid.Add(padding);
            grid.Add(padding);
            grid.Add(padding);

            int solutionPart1 = 0;
            int solutionPart2 = 0;
            for(int y = 3; y < grid.Count - 3; y++)
            {
                for(int x = 3; x < grid[y].Length - 3; x++)
                {
                    if(grid[y][x] == 'X')
                    {
                        if(grid[y][x+1] == 'M' && grid[y][x+2] == 'A' && grid[y][x+3] == 'S')
                        solutionPart1++;

                        if(grid[y+1][x+1] == 'M' && grid[y+2][x+2] == 'A' && grid[y+3][x+3] == 'S')
                            solutionPart1++;

                        if(grid[y+1][x] == 'M' && grid[y+2][x] == 'A' && grid[y+3][x] == 'S')
                            solutionPart1++;

                        if(grid[y+1][x-1] == 'M' && grid[y+2][x-2] == 'A' && grid[y+3][x-3] == 'S')
                            solutionPart1++;

                        if(grid[y][x-1] == 'M' && grid[y][x-2] == 'A' && grid[y][x-3] == 'S')
                            solutionPart1++;

                        if(grid[y-1][x-1] == 'M' && grid[y-2][x-2] == 'A' && grid[y-3][x-3] == 'S')
                            solutionPart1++;

                        if(grid[y-1][x] == 'M' && grid[y-2][x] == 'A' && grid[y-3][x] == 'S')
                            solutionPart1++;

                        if(grid[y-1][x+1] == 'M' && grid[y-2][x+2] == 'A' && grid[y-3][x+3] == 'S')
                            solutionPart1++;
                    }

                    if(grid[y][x] == 'A')
                    {
                        bool diagonal1 = (grid[y-1][x-1] == 'M' && grid[y+1][x+1] == 'S') || (grid[y-1][x-1] == 'S' && grid[y+1][x+1] == 'M');
                        bool diagonal2 = (grid[y-1][x+1] == 'M' && grid[y+1][x-1] == 'S') || (grid[y-1][x+1] == 'S' && grid[y+1][x-1] == 'M');
                        if(diagonal1 && diagonal2)
                            solutionPart2++;
                    }
                }
            }

            /* Part 1 */
            Console.WriteLine("Day 04 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 04 part 2, result: " + solutionPart2);
        }
    }
}
