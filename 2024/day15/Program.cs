using System;
using System.Text.RegularExpressions;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();
            Part2();
            
        }

        static void Part1()
        {
            long solutionPart1 = 0;
            string input = File.ReadAllText("input.txt");

            string[] splits = input.Split("\n\n");
            string moves = splits[1].Replace("\n", String.Empty);
            
            List<char[]> grid = new List<char[]>();
            string[] lines = splits[0].Split("\n");
            int y = 0;
            int x = 0;
            bool foundStart = false;
            foreach(string line in lines)
            {
                char[] characters = line.ToCharArray();
                grid.Add(characters);
                if(!foundStart)
                {
                    x = line.IndexOf('@');
                    if(x != -1)
                        foundStart = true;
                    else
                        y++;
                }
            }

            foreach(char move in moves)
            {
                int dx = 0;
                int dy = 0;
                switch ((move))
                {
                    case '>':
                        dx = 1;
                        break;
                    case 'v':
                        dy = 1;
                        break;
                    case '<':
                        dx = -1;
                        break;
                    case '^':
                        dy = -1;
                        break;
                }

                if(grid[y + dy][x + dx] == '.')
                {
                    grid[y + dy][x + dx] = '@';
                    grid[y][x] = '.';
                    x += dx;
                    y += dy;
                }
                else if(grid[y + dy][x + dx] == 'O')
                {
                    int xSearch = x + dx;
                    int ySearch = y + dy;
                    do
                    {
                        xSearch += dx;
                        ySearch += dy;
                    }
                    while(grid[ySearch][xSearch] == 'O');

                    /* Only move robot if we found an empty space where we can push to boxes to. */
                    if(grid[ySearch][xSearch] == '.')
                    {
                        grid[ySearch][xSearch] = 'O';
                        grid[y + dy][x + dx] = '@';
                        grid[y][x] = '.';
                        x += dx;
                        y += dy;
                    }
                }
            }

            for(int i = 0; i < grid.Count; i++)
            {
                char[] line = grid[i];
                for(int j = 0; j < line.Length; j++)
                {
                    if(line[j] == 'O')
                    {
                        solutionPart1 += (i * 100) + j;
                    }
                }
            }

            /* Part 1 */
            Console.WriteLine("Day 15 part 1, result: " + solutionPart1);
        }


        static void Part2()
        {
            long solutionPart2 = 0;
            string input = File.ReadAllText("input.txt");

            string[] splits = input.Split("\n\n");
            string moves = splits[1].Replace("\n", String.Empty);
            
            /* Parsing ... */
            List<char[]> grid = new List<char[]>();
            string[] lines = splits[0].Split("\n");
            int y = 0;
            int x = 0;
            bool foundStart = false;
            foreach(string line in lines)
            {
                string wideLine = "";
                foreach(char c in line)
                {
                    switch (c)
                    {
                    case '#':
                        wideLine += "##";
                        break;
                    case '.':
                        wideLine += "..";
                        break;
                    case '@':
                        wideLine += "@.";
                        break;
                    case 'O':
                        wideLine += "[]";
                        break;
                    }
                }

                char[] characters = wideLine.ToCharArray();
                grid.Add(characters);
                if(!foundStart)
                {
                    x = wideLine.IndexOf('@');
                    if(x != -1)
                        foundStart = true;
                    else
                        y++;
                }
            }

            /* Moving the robot ... */
            foreach(char move in moves)
            {
                if(move == '<' || move == '>')
                {
                    int dx = move == '<' ? -1 : 1;
                    char neighbour = grid[y][x + dx];
                    if(neighbour == '.')
                    {
                        grid[y][x + dx] = '@';
                        grid[y][x] = '.';
                        x += dx;
                    }
                    else if(neighbour == '[' || neighbour == ']')
                    {
                        /* Search horizontally for anything that is not a box. If we do find a box, continue to its neighbour. */
                        int xSearch = x + dx;
                        char nextNeighbour;
                        do
                        {
                            xSearch += 2 * dx;
                            nextNeighbour = grid[y][xSearch];
                        }
                        while(nextNeighbour == '[' || nextNeighbour == ']');

                        /* Only move robot if we found an empty space where we can push to boxes to. */
                        if(nextNeighbour == '.')
                        {
                            /* Iterate backwards from the empty space to the robots position. */
                            for(int i = xSearch; i != x; i += -1 * dx)
                            {
                                grid[y][i] = grid[y][i + -1 * dx];
                            }

                            grid[y][x] = '.';
                            x += dx;
                        }
                    }
                }
                else
                {
                    int dy = move == '^' ? -1 : 1;
                    char neighbour = grid[y + dy][x];

                    if(neighbour == '.')
                    {
                        grid[y + dy][x] = '@';
                        grid[y][x] = '.';
                        y += dy;
                    }
                    else if(neighbour == '[' || neighbour == ']')
                    {
                        HashSet<Vec2> boxes = new HashSet<Vec2>(new Vec2Comparer()); // Keep a set of boxes which are moved by the robot.
                        Queue<Vec2> queue = new Queue<Vec2>(); // Queue of boxes we are going to check if they can move.
                        
                        /* Add starting box to queue. */
                        Vec2 startBox;
                        if(neighbour == '[')
                            startBox = new Vec2(x, y + dy);
                        else
                            startBox = new Vec2(x - 1, y + dy);
                        queue.Enqueue(startBox);

                        /* Check each box in the queue if they can move. If the box moves another box then add that box to the queue. */
                        bool moveBlockedByWall = false;
                        while(queue.Count != 0 && !moveBlockedByWall)
                        {
                            Vec2 boxPosition = queue.Dequeue();
                            if(boxes.Contains(boxPosition))
                                continue;
                            else
                                boxes.Add(boxPosition);

                            char left = grid[boxPosition.Y + dy][boxPosition.X];
                            char right = grid[boxPosition.Y + dy][boxPosition.X + 1];
                            if(left == '#' || right == '#')
                            {
                                moveBlockedByWall = true;
                                continue;
                            }
                            
                            if(left == '[')
                            {
                                Vec2 box = new Vec2(boxPosition.X, boxPosition.Y + dy);
                                queue.Enqueue(box);
                            }
                            
                            if(right == '[')
                            {
                                Vec2 box = new Vec2(boxPosition.X + 1, boxPosition.Y + dy);
                                queue.Enqueue(box);
                            }

                            if(left == ']')
                            {
                                Vec2 box = new Vec2(boxPosition.X - 1, boxPosition.Y + dy);
                                queue.Enqueue(box);
                            }
                        }

                        /* If all boxes can be moved the iteratively start moving those boxes.
                         * We are moving boxes by row. Starting at the row furthest away from the robot. */
                        if(!moveBlockedByWall)
                        {
                            List<List<Vec2>> moveOrders = new List<List<Vec2>>();
                            for(int i = 0; i < grid.Count; i++)
                                moveOrders.Add(new List<Vec2>());
                            foreach(Vec2 box in boxes)
                                moveOrders[box.Y].Add(box);

                            if(dy > 0)
                                moveOrders.Reverse();

                            /* Move up each block in the row. */
                            foreach(List<Vec2> row in moveOrders)
                            {
                                foreach(Vec2 box in row)
                                {
                                    grid[box.Y + dy][box.X] = '[';
                                    grid[box.Y + dy][box.X + 1] = ']';
                                    grid[box.Y][box.X] = '.';
                                    grid[box.Y][box.X + 1] = '.';
                                }
                            }

                            /* Move robot. */
                            grid[y + dy][x] = '@';
                            grid[y][x] = '.';
                            y += dy;
                        }
                    }
                }
            }

            /* Calculate score. */
            for(int i = 0; i < grid.Count; i++)
            {
                char[] line = grid[i];
                for(int j = 0; j < line.Length; j++)
                {
                    if(line[j] == '[')
                    {
                        solutionPart2 += (i * 100) + j;
                    }
                }
            }

            /* Part 2 */
            Console.WriteLine("Day 15 part 2, result: " + solutionPart2);
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

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
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
