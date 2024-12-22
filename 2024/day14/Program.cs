using System;
using System.Text.RegularExpressions;

namespace day14
{
    class Program
    {
        static int GridWidth = 101;
        static int gridHeight = 103;

        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");

            string pattern = @"p=(?<px>-?\d+),(?<py>-?\d+)\ v=(?<vx>-?\d+),(?<vy>-?\d+)";
            Regex reg = new Regex(pattern);
            MatchCollection matches = reg.Matches(input);

            /* Parse all robots. */
            List<Robot> robots = new List<Robot>();
            foreach(Match match in matches)
            {
                int px = Convert.ToInt32(match.Groups["px"].Value);
                int py = Convert.ToInt32(match.Groups["py"].Value);
                int vx = Convert.ToInt32(match.Groups["vx"].Value);
                int vy = Convert.ToInt32(match.Groups["vy"].Value);

                Vec2 position = new Vec2(px, py);
                Vec2 speed = new Vec2(vx, vy);

                Robot r = new Robot(position, speed, GridWidth, gridHeight);
                robots.Add(r);
            }

            /* Iterate through the first 100 seconds for part 1. */
            for(int t = 1; t <= 100; t++)
            {
                foreach(Robot r in robots)
                    r.Update();

                Console.WriteLine("\nAfter {0} seconds:", t);
                PrintGrid(robots);
            }
            int solutionPart1 = SafetyScore(robots);

            /* Continue searching for the tree in part 2. */
            for(int t = 101; t <= 10000; t++)
            {
                foreach(Robot r in robots)
                    r.Update();

                Console.WriteLine("\nAfter {0} seconds:", t);
                PrintGrid(robots);
            }

            /* Part 1 */
            Console.WriteLine("Day 14 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 14 part 2, look through output noise to find the tree");
        }


        static int SafetyScore(List<Robot> robots)
        {
            int q1 = 0;
            int q2 = 0;
            int q3 = 0;
            int q4 = 0;

            int xMiddle = (GridWidth - 1) / 2;
            int yMiddle = (gridHeight - 1) / 2;

            foreach(Robot r in robots)
            {
                if(r.Position.X == xMiddle || r.Position.Y == yMiddle)
                    continue;

                if(r.Position.X < xMiddle)
                {
                    if(r.Position.Y < yMiddle)
                        q1++;
                    else
                        q3++;
                }
                else
                {
                    if(r.Position.Y < yMiddle)
                        q2++;
                    else
                        q4++;
                }
            }

            return q1 * q2 * q3 * q4;
        }

        static void PrintGrid(List<Robot> robots)
        {
            List<List<string>> grid = new List<List<string>>();
            for(int i = 0; i < gridHeight; i++)
            {
                grid.Add(new List<string>());
                for(int j = 0; j < GridWidth; j++)
                {
                    grid[i].Add(" ");
                }
            }

            foreach(Robot r in robots)
            {
                grid[r.Position.Y][r.Position.X] = "#";
            }

            foreach(List<string> characters in grid)
            {
                string line = string.Join("", characters);
                Console.WriteLine(line);
            }
        }
    }


    public class Robot
    {
        public Vec2 Speed { get; }
        public Vec2 Position { get; set; }

        private int GridWidth;
        private int gridHeight;

        public Robot(Vec2 position, Vec2 speed, int gridWidth, int gridHeight)
        {
            this.Position = position;
            this.Speed = speed;
            this.GridWidth = gridWidth;
            this.gridHeight = gridHeight;
        }

        public void Update()
        {
            Position.X = (Position.X + Speed.X + GridWidth) % GridWidth;
            Position.Y = (Position.Y + Speed.Y + gridHeight) % gridHeight;
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
