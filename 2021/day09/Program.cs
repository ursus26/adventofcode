using System;
using System.IO;
using System.Collections.Generic;

namespace day09
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] heightMap = parseRawData("input.txt");
            List<Basin> basins = new List<Basin>();
            int solutionPart1 = 0;

            int mapHeight = heightMap.GetLength(0);
            int mapWidth = heightMap.GetLength(1);
            for(int i = 0; i < mapHeight; i++)
            {
                for(int j = 0; j < mapWidth; j++)
                {
                    int height = heightMap[i, j];
                    if(i-1 >= 0)
                        if(height >= heightMap[i-1, j])
                            continue;

                    if(i+1 < mapHeight)
                        if(height >= heightMap[i+1, j])
                            continue;

                    if(j-1 >= 0)
                        if(height >= heightMap[i, j-1])
                            continue;

                    if(j+1 < mapWidth)
                        if(height >= heightMap[i, j+1])
                            continue;

                    basins.Add(new Basin(i, j));
                    solutionPart1 += height + 1;
                }
            }

            int solutionPart2 = part2(basins, heightMap);
            Console.WriteLine("Day 9 part 1, result: " + solutionPart1);
            Console.WriteLine("Day 9 part 2, result: " + solutionPart2);

        }

        static int[,] parseRawData(string fileName)
        {
            string[] rawRows = File.ReadAllLines(fileName);
            int[,] heightMap = new int[rawRows.Length, rawRows[0].Length];
            for(int i = 0; i < rawRows.Length; i++)
            {
                for(int j = 0; j < rawRows[0].Length; j++)
                {
                    heightMap[i, j] = Int32.Parse(rawRows[i][j].ToString());
                }
            }
            return heightMap;
        }

        static int part2(List<Basin> basins, int[,] heightMap)
        {
            /* Initialize the basins and cache. */
            Dictionary<(int, int), Basin> cache = new Dictionary<(int, int), Basin>();
            foreach(Basin b in basins)
                cache.Add((b.y, b.x), b);

            Stack<(int, int)> stack = new Stack<(int, int)>();
            for(int i = 0; i < heightMap.GetLength(0); i++)
            {
                for(int j = 0; j < heightMap.GetLength(1); j++)
                {
                    if(heightMap[i, j] == 9)
                        continue;

                    (int y, int x) pos = (i, j);
                    Basin basin;
                    while(true)
                    {
                        if(cache.ContainsKey((pos.y, pos.x)))
                        {
                            basin = cache[(pos.y, pos.x)];
                            break;
                        }

                        /* Store the current position and find a lower neighbour. */
                        stack.Push(pos);
                        pos = findLowerNeighbour(pos, heightMap);
                    }

                    /* Backtrace the stack to our starting point. */
                    while(stack.Count > 0)
                    {
                        pos = stack.Pop();
                        cache.Add(pos, basin);
                        basin.IncrementSize();
                    }
                }
            }

            basins.Sort(delegate(Basin b1, Basin b2) { return b1.size.CompareTo(b2.size); });
            int idx = basins.Count - 1;
            return basins[idx].size * basins[idx-1].size * basins[idx-2].size;
        }

        static (int, int) findLowerNeighbour((int y, int x) pos, int[,] heightMap)
        {
            int mapHeight = heightMap.GetLength(0);
            int mapWidth = heightMap.GetLength(1);
            int minHeight = heightMap[pos.y, pos.x];
            (int, int) returnPosition = pos;

            if(pos.y-1 >= 0)
            {
                if(minHeight > heightMap[pos.y-1, pos.x])
                {
                    minHeight = heightMap[pos.y-1, pos.x];
                    returnPosition = (pos.y-1, pos.x);
                }
            }
            if(pos.y+1 < mapHeight)
            {
                if(minHeight > heightMap[pos.y+1, pos.x])
                {
                    minHeight = heightMap[pos.y+1, pos.x];
                    returnPosition = (pos.y+1, pos.x);
                }
            }
            if(pos.x-1 >= 0)
            {
                if(minHeight > heightMap[pos.y, pos.x-1])
                {
                    minHeight = heightMap[pos.y, pos.x-1];
                    returnPosition = (pos.y, pos.x-1);
                }
            }
            if(pos.x+1 < mapWidth)
            {
                if(minHeight > heightMap[pos.y, pos.x+1])
                {
                    minHeight = heightMap[pos.y, pos.x+1];
                    returnPosition = (pos.y, pos.x+1);
                }
            }
            return returnPosition;
        }
    }

    class Basin
    {
        public int size { get; set; }
        public int x { get; }
        public int y { get; }

        public Basin(int y, int x)
        {
            this.x = x;
            this.y = y;
            this.size = 1;
        }

        public void IncrementSize()
        {
            this.size++;
        }
    }
}
