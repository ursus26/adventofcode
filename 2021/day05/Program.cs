using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day05
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("input.txt");
            Dictionary<(int, int), int> boardPart1 = new Dictionary<(int, int), int>();
            Dictionary<(int, int), int> boardPart2 = new Dictionary<(int, int), int>();

            string pattern = @"(\d+),(\d+)\s->\s(\d+),(\d+)";
            Regex regex = new Regex(pattern);
            foreach(string line in data)
            {
                Match match = regex.Match(line);
                int x1 = Int32.Parse(match.Groups[1].Value);
                int y1 = Int32.Parse(match.Groups[2].Value);
                int x2 = Int32.Parse(match.Groups[3].Value);
                int y2 = Int32.Parse(match.Groups[4].Value);

                if(x1 == x2) /* Horizontal */
                {
                    for(int i = Math.Min(y1, y2); i <= Math.Max(y1, y2); i++)
                    {
                        boardPart1[(x1, i)] = !boardPart1.ContainsKey((x1, i)) ? 1 : boardPart1[(x1, i)] + 1;
                        boardPart2[(x1, i)] = !boardPart2.ContainsKey((x1, i)) ? 1 : boardPart2[(x1, i)] + 1;
                    }
                }
                else if(y1 == y2) /* Vertical */
                {
                    for(int i = Math.Min(x1, x2); i <= Math.Max(x1, x2); i++)
                    {
                        boardPart1[(i, y1)] = !boardPart1.ContainsKey((i, y1)) ? 1 : boardPart1[(i, y1)] + 1;
                        boardPart2[(i, y1)] = !boardPart2.ContainsKey((i, y1)) ? 1 : boardPart2[(i, y1)] + 1;
                    }
                }
                else /* Diagonal */
                {
                    int dx = x1 < x2 ? 1 : -1;
                    int dy = y1 < y2 ? 1 : -1;
                    for(int i = 0; i <= Math.Abs(x1 - x2); i++)
                    {
                        (int, int) coord = (x1 + dx * i, y1 + dy * i);
                        boardPart2[coord] = !boardPart2.ContainsKey(coord) ? 1 : boardPart2[coord] + 1;
                    }
                }
            }

            int solutionPart1 = 0;
            foreach(int count in boardPart1.Values)
            {
                if(count > 1)
                    solutionPart1++;
            }

            int solutionPart2 = 0;
            foreach(int count in boardPart2.Values)
            {
                if(count > 1)
                    solutionPart2++;
            }
            Console.WriteLine("Day 5 part 1, result: {0}", solutionPart1);
            Console.WriteLine("Day 5 part 2, result: {0}", solutionPart2);
        }
    }
}
