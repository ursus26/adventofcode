using System;
using System.IO;
using System.Collections.Generic;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllText("input.txt").Trim().Split("\n\n");
            HashSet<(int, int)> dots = new HashSet<(int, int)>();
            foreach(string dot in data[0].Split('\n'))
            {
                string[] dotCoord = dot.Split(',');
                int x = int.Parse(dotCoord[0]);
                int y = int.Parse(dotCoord[1]);
                dots.Add((x, y));
            }

            int solutionPart1 = 0;
            foreach(string rawFoldCommand in data[1].Split('\n'))
            {
                char foldDirection = rawFoldCommand[11];
                int fold = int.Parse(rawFoldCommand.Substring(13));

                if(foldDirection == 'x')
                    dots = foldX(dots, fold);
                else
                    dots = foldY(dots, fold);

                if(solutionPart1 == 0)
                    solutionPart1 = dots.Count;
            }

            Console.WriteLine("Day 13 part 1, result: " + solutionPart1);
            Console.WriteLine("Day 13 part 2, result:");
            int maxX = 0;
            int maxY = 0;
            foreach((int x, int y) elem in dots)
            {
                if(elem.x > maxX)
                    maxX = elem.x;
                if(elem.y > maxY)
                    maxY = elem.y;
            }
            for(int i = 0; i < maxY+1; i++)
            {
                for(int j = 0; j < maxX+1; j++)
                {
                    if(dots.Contains((j, i)))
                        Console.Write('#');
                    else
                        Console.Write(' ');
                }
                Console.WriteLine("");
            }
        }

        static HashSet<(int, int)> foldY(HashSet<(int, int)> dots, int foldY)
        {
            HashSet<(int, int)> outputDots = new HashSet<(int, int)>();
            foreach((int x, int y) elem in dots)
            {
                if(elem.y < foldY && !outputDots.Contains(elem))
                    outputDots.Add(elem);
                else if(elem.y > foldY && !outputDots.Contains((elem.x, foldY - (elem.y - foldY))))
                    outputDots.Add((elem.x, foldY - (elem.y - foldY)));
            }
            return outputDots;
        }

        static HashSet<(int, int)> foldX(HashSet<(int, int)> dots, int foldX)
        {
            HashSet<(int, int)> outputDots = new HashSet<(int, int)>();
            foreach((int x, int y) elem in dots)
            {
                if(elem.x < foldX && !outputDots.Contains(elem))
                    outputDots.Add(elem);
                else if(elem.x > foldX && !outputDots.Contains((foldX - (elem.x - foldX), elem.y)))
                    outputDots.Add((foldX - (elem.x - foldX), elem.y));
            }
            return outputDots;
        }
    }
}
