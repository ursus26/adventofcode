using System;
using System.IO;

namespace day06
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllText("input.txt").Trim().Split(',');
            ulong[] fishCounts = new ulong[9];
            foreach(string s in data)
            {
                ulong val = UInt64.Parse(s);
                fishCounts[val]++;
            }

            fishCounts = simulate(fishCounts, 80);
            ulong solutionPart1 = countFish(fishCounts);

            fishCounts = simulate(fishCounts, 256 - 80);
            ulong solutionPart2 = countFish(fishCounts);

            Console.WriteLine("Day 6 part 1, result: {0}", solutionPart1);
            Console.WriteLine("Day 6 part 2, result: {0}", solutionPart2);
        }

        static ulong[] simulate(ulong[] currentFish, int days)
        {
            ulong[] nextFish = new ulong[9];
            for(int i = 0; i < days; i++)
            {
                for(int j = 0; j < currentFish.Length - 1; j++)
                {
                    nextFish[j] = currentFish[j+1];
                    currentFish[j+1] = 0;
                }
                nextFish[6] += currentFish[0];
                nextFish[8] += currentFish[0];
                currentFish[0] = 0;

                /* Swap buffers. */
                ulong[] tmp = nextFish;
                nextFish = currentFish;
                currentFish = tmp;
            }
            return currentFish;
        }

        static ulong countFish(ulong[] fishCounts)
        {
            ulong count = 0;
            foreach(ulong val in fishCounts)
                count += val;
            return count;
        }
    }
}
