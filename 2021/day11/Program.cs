using System;
using System.IO;
using System.Collections.Generic;

namespace day11
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("input.txt");
            int gridHeight = data.Length;
            int gridWidth = data[0].Length;
            int[,] grid = new int[gridHeight + 2, gridWidth + 2];

            for(int i = 0; i < gridHeight; i++)
            {
                for(int j = 0; j < gridWidth; j++)
                {
                    grid[i+1, j+1] = Int32.Parse(data[i][j].ToString());
                }
            }


            int solutionPart1 = 0;
            for(int step = 0; step < 100; step++)
                solutionPart1 += simStep(grid);
            Console.WriteLine("Day 11 part 1, result: " + solutionPart1);

            int flashCount = 0;
            int solutionPart2 = 100;
            while(flashCount != gridHeight * gridWidth)
            {
                flashCount = simStep(grid);
                solutionPart2++;
            }

            Console.WriteLine("Day 11 part 2, result: " + solutionPart2);
        }

        static int simStep(int [,] grid)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>();
            int gridHeight = grid.GetLength(0) - 2;
            int gridWidth = grid.GetLength(1) - 2;
            int flashCount = 0;
            for(int i = 1; i < gridHeight + 1; i++)
            {
                for(int j = 1; j < gridWidth + 1; j++)
                {
                    if(incrementEnergy(grid, (i,j)))
                    {
                        flashCount++;
                        stack.Push((i-1,j-1));
                        stack.Push((i-1,j));
                        stack.Push((i-1,j+1));
                        stack.Push((i,j-1));
                        stack.Push((i,j+1));
                        stack.Push((i+1,j-1));
                        stack.Push((i+1,j));
                        stack.Push((i+1,j+1));
                    }
                }
            }

            /* Handle flashes */
            while(stack.Count > 0)
            {
                (int y, int x) coord = stack.Pop();
                if(coord.y > 0 && coord.y <= gridHeight && coord.x > 0 && coord.x <= gridWidth
                && grid[coord.y, coord.x] > 0)
                {
                    if(incrementEnergy(grid, coord))
                    {
                        flashCount++;
                        stack.Push((coord.y-1,coord.x-1));
                        stack.Push((coord.y-1,coord.x));
                        stack.Push((coord.y-1,coord.x+1));
                        stack.Push((coord.y,coord.x-1));
                        stack.Push((coord.y,coord.x+1));
                        stack.Push((coord.y+1,coord.x-1));
                        stack.Push((coord.y+1,coord.x));
                        stack.Push((coord.y+1,coord.x+1));
                    }
                }
            }

            return flashCount;
        }

        static bool incrementEnergy(int[,] grid, (int y, int x) coord)
        {
            grid[coord.y, coord.x] = (grid[coord.y, coord.x] + 1) % 10;
            if(grid[coord.y, coord.x] == 0)
                return true;
            else
                return false;
        }

        static void printGrid(int[,] grid)
        {
            for(int i = 1 ; i < grid.GetLength(0) - 1; i++)
            {
                for(int j = 1; j < grid.GetLength(1) - 1; j++)
                {
                    Console.Write("{0}", grid[i, j]);
                }
                Console.WriteLine("");
            }
        }
    }
}
