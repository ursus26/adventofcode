using System;
using System.IO;
using System.Collections.Generic;

namespace day07
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] rawData = File.ReadAllText("input.txt").Trim().Split(',');
            List<int> data = new List<int>();
            foreach(string s in rawData)
            {
                data.Add(Int32.Parse(s));
            }
            data.Sort();
            int median = Median(data);

            /* Part 1 */
            int solutionPart1 = 0;
            foreach(int crabPosition in data)
            {
                solutionPart1 += Math.Abs(crabPosition - median);
            }
            Console.WriteLine("Day 7 part 1, result: {0}", solutionPart1);

            /* Part 2, brute force. */
            int minFuel = Int32.MaxValue;
            for(int i = 0; i < data[data.Count - 1]; i++)
            {
                int sumFuel = 0;
                foreach(int crabPosition in data)
                {
                    int n = Math.Abs(crabPosition - i);
                    sumFuel += ((n * (n+1)) / 2);
                }
                if(sumFuel < minFuel)
                    minFuel = sumFuel;
            }
            int solutionPart2 = minFuel;
            Console.WriteLine("Day 7 part 2, result: {0}", solutionPart2);
        }


        static int Median(List<int> data)
        {
            if(data.Count % 2 == 0)
            {
                return (data[data.Count / 2 - 1] + data[data.Count / 2]) / 2;
            }
            else
            {
                return data[(data.Count + 1) / 2];
            }
        }
    }
}
