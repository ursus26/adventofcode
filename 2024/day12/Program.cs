namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> garden = ParseInput();
            HashSet<Vec2> assignedPlots = new HashSet<Vec2>(new Vec2Comparer());
            long solutionPart1 = 0;
            long solutionPart2 = 0;
            for(int y = 1; y < garden.Count - 1; y++)
            {
                string line = garden[y];
                for(int x = 1; x < line.Length - 1; x++)
                {
                    Vec2 pos = new Vec2(x, y);
                    if(!assignedPlots.Contains(pos))
                    {
                        HashSet<Vec2> region = FloodFillPlots(garden, pos);
                        assignedPlots.UnionWith(region);

                        int area = region.Count;

                        int perimeter = CalculatePerimiter(garden, region);
                        solutionPart1 += area * perimeter;

                        int corners = CountCorners(garden, region);
                        solutionPart2 += area * corners;
                    }
                }
            }

            /* Part 1 */
            Console.WriteLine("Day 12 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 12 part 2, result: " + solutionPart2);
        }

        static List<string> ParseInput()
        {
            string[] lines = File.ReadAllLines("input.txt");
            int width = lines[0].Length + 2;
            string padding = new string('.', width);

            List<string> output = new List<string>();
            output.Add(padding);
            foreach(string line in lines)
                output.Add("." + line + ".");
            output.Add(padding);
            return output;
        }

        static HashSet<Vec2> FloodFillPlots(List<string> garden, Vec2 start)
        {
            char c = garden[start.Y][start.X];
            Queue<Vec2> queue = new Queue<Vec2>();
            queue.Enqueue(start);
            HashSet<Vec2> region = new HashSet<Vec2>(new Vec2Comparer());
            region.Add(start);
            while(queue.Count != 0)
            {
                Vec2 position = queue.Dequeue();

                Vec2 up = new Vec2(position.X, position.Y - 1);
                Vec2 down = new Vec2(position.X, position.Y + 1);
                Vec2 left = new Vec2(position.X - 1, position.Y);
                Vec2 right = new Vec2(position.X + 1, position.Y);

                if(!region.Contains(up) && garden[up.Y][up.X] == c)
                {
                    region.Add(up);
                    queue.Enqueue(up);
                }

                if(!region.Contains(down) && garden[down.Y][down.X] == c)
                {
                    region.Add(down);
                    queue.Enqueue(down);
                }
                    

                if(!region.Contains(left) && garden[left.Y][left.X] == c)
                {
                    region.Add(left);
                    queue.Enqueue(left);
                }

                if(!region.Contains(right) && garden[right.Y][right.X] == c)
                {
                    region.Add(right);
                    queue.Enqueue(right);
                }
            }

            return region;
        }

        static int CalculatePerimiter(List<string> garden, HashSet<Vec2> region)
        {
            int perimeter = 0;
            foreach(Vec2 position in region)
            {
                char c = garden[position.Y][position.X];

                Vec2 up = new Vec2(position.X, position.Y - 1);
                Vec2 down = new Vec2(position.X, position.Y + 1);
                Vec2 left = new Vec2(position.X - 1, position.Y);
                Vec2 right = new Vec2(position.X + 1, position.Y);

                if(garden[up.Y][up.X] != c)
                    perimeter++;

                if(garden[down.Y][down.X] != c)
                    perimeter++;

                if(garden[left.Y][left.X] != c)
                    perimeter++;

                if(garden[right.Y][right.X] != c)
                    perimeter++;
            }

            return perimeter;
        }

        static int CountCorners(List<string> garden, HashSet<Vec2> region)
        {
            int corners = 0;

            foreach(Vec2 position in region)
            {
                char c = garden[position.Y][position.X];

                char topLeft    = garden[position.Y - 1][position.X - 1];
                char top        = garden[position.Y - 1][position.X];
                char topRight   = garden[position.Y - 1][position.X + 1];
                char left       = garden[position.Y][position.X - 1];
                char right      = garden[position.Y][position.X + 1];
                char downLeft   = garden[position.Y + 1][position.X - 1];
                char down       = garden[position.Y + 1][position.X];
                char downRight  = garden[position.Y + 1][position.X + 1];

                if(top != c && left != c)
                    corners++;

                if(top != c && right != c)
                    corners++;

                if(left != c && down != c)
                    corners++;

                if(right != c && down != c)
                    corners++;

                if(topLeft != c && top == c && left == c)
                    corners++;

                if(topRight != c && top == c && right == c)
                    corners++;

                if(left == c && downLeft != c && down == c)
                    corners++;

                if(right == c && downRight != c && down == c)
                    corners++;
            }

            return corners;
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
