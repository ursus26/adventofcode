namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");
            int gridHeight = input.Length + 2;
            int gridWidth = input[0].Length + 2;

            /* Create a grid of heights with a border for safety checks. */
            List<List<int>> grid = new List<List<int>>();
            List<int> padding = new List<int>();
            for(int i = 0; i < gridWidth; i++)
                padding.Add(-1);
            grid.Add(padding);
            foreach(string line in input)
            {
                List<int> heights = new List<int>();
                heights.Add(-1);
                foreach(char c in line)
                {
                    int height = (int)Char.GetNumericValue(c);
                    heights.Add(height);
                }
                heights.Add(-1);
                grid.Add(heights);
            }
            padding = new List<int>();
            for(int i = 0; i < gridWidth; i++)
                padding.Add(-1);
            grid.Add(padding);

            /* Find all posible trail head starts. */
            List<Vec2> trailHeads = new List<Vec2>();
            for(int y = 0; y < gridHeight; y++)
            {
                for(int x = 0; x < gridWidth; x++)
                {
                    if(grid[y][x] == 0)
                        trailHeads.Add(new Vec2(x, y));
                }
            }

            /* Map out the trails. */
            long solutionPart1 = 0;
            long solutionPart2 = 0;
            foreach(Vec2 trailStart in trailHeads)
            {
                List<Vec2> trailEndpoints = GetTrailsEndpoints(grid, trailStart);
                HashSet<Vec2> visitedPeaks = new HashSet<Vec2>(trailEndpoints, new Vec2Comparer());

                solutionPart1 += visitedPeaks.Count;
                solutionPart2 += trailEndpoints.Count;
            }

            /* Part 1 */
            Console.WriteLine("Day 10 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 10 part 2, result: " + solutionPart2);
        }

        static List<Vec2> GetTrailsEndpoints(List<List<int>> grid, Vec2 start)
        {
            // HashSet<Vec2> visitedPeaks = new HashSet<Vec2>(new Vec2Comparer());
            List<Vec2> visitedPeaks = new List<Vec2>();
            Queue<Vec2> queue = new Queue<Vec2>();
            queue.Enqueue(start);

            while(queue.Count != 0)
            {
                Vec2 position = queue.Dequeue();
                int height = grid[position.Y][position.X];
                if(height == 9)
                {
                    visitedPeaks.Add(position);
                    continue;
                }

                Vec2 up = new Vec2(position.X, position.Y - 1);
                Vec2 down = new Vec2(position.X, position.Y + 1);
                Vec2 left = new Vec2(position.X - 1, position.Y);
                Vec2 right = new Vec2(position.X + 1, position.Y);

                if(grid[up.Y][up.X] == height + 1)
                    queue.Enqueue(up);

                if(grid[down.Y][down.X] == height + 1)
                    queue.Enqueue(down);

                if(grid[left.Y][left.X] == height + 1)
                    queue.Enqueue(left);

                if(grid[right.Y][right.X] == height + 1)
                    queue.Enqueue(right);
            }

            return visitedPeaks;
        }
    }


    public class Vec2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vec2(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class Vec2Comparer: IEqualityComparer<Vec2>
    {
        bool IEqualityComparer<Vec2>.Equals(Vec2? v1, Vec2? v2)
        {
            if(v1 == null && v2 == null)
                return true;
            else if(v1 == null || v2 == null)
                return false;
            else
                return v1.X == v2.X && v1.Y == v2.Y;
        }

        int IEqualityComparer<Vec2>.GetHashCode(Vec2 obj)
        {
            return obj.X << 16 | obj.Y;
        }
    }
}
