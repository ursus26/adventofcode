using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day17
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = File.ReadAllText("input.txt").Trim();
            string pattern = @"target\sarea:\sx=(-?\d+)..(-?\d+),\sy=(-?\d+)..(-?\d+)";
            Regex regex = new Regex(pattern);

            Match m = regex.Match(data);
            int xMin = Int32.Parse(m.Groups[1].Value);
            int xMax = Int32.Parse(m.Groups[2].Value);
            int yMin = Int32.Parse(m.Groups[3].Value);
            int yMax = Int32.Parse(m.Groups[4].Value);

            int solutionPart1 = Int32.MinValue;
            int solutionPart2 = 0;
            for(int x = 0; x <= xMax; x++)
            {
                for(int y = yMin; y < 1000; y++)
                {
                    int maximum = simulate(x, y, xMin, xMax, yMin, yMax);
                    if(maximum > solutionPart1)
                        solutionPart1 = maximum;

                    if(maximum != Int32.MinValue)
                        solutionPart2++;
                }
            }

            Console.WriteLine("Day 17 part 1, result: " + solutionPart1);
            Console.WriteLine("Day 17 part 2, result: " + solutionPart2);
        }

        static int simulate(int vx, int vy, int xMin, int xMax, int yMin, int yMax)
        {
            int x = 0;
            int y = 0;
            int dx = vx;
            int dy = vy;
            int maximum = 0;

            do
            {
                /* Update according to the rules. */
                x += dx;
                y += dy;
                if(dx != 0)
                    dx--;
                dy--;

                /* Bounds check and maximum altitude check. */
                maximum = Math.Max(y, maximum);
                if(x >= xMin && x <= xMax && y >= yMin && y <= yMax)
                    return maximum;

            } while(x <= xMax && y >= yMin);
            return Int32.MinValue;
        }
    }
}
