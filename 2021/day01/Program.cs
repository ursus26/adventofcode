using System;
using System.IO;
using System.Collections.Generic;

namespace day01
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> data = readIntegers("./input.txt");
            int solutionPart1 = part1(data);
            int solutionPart2 = part2(data);
            Console.WriteLine("Day 1 part 1, result: " + solutionPart1);
            Console.WriteLine("Day 1 part 2, result: " + solutionPart2);
        }


        static int part1(List<int> input)
        {
            int incrementCounter = 0;
            int previous = input[0];
            foreach(int current in input)
            {
                if(current > previous)
                    incrementCounter++;

                previous = current;
            }
            return incrementCounter;
        }


        static int part2(List<int> input)
        {
            int incrementCounter = 0;
            int previous = input[0] + input[1] + input[2];
            int current = input[0] + input[1];
            for(int i = 2; i < input.Count; i++)
            {
                current += input[i];
                if(current > previous)
                    incrementCounter++;

                previous = current;
                current -= input[i - 2];
            }
            return incrementCounter;
        }


        static List<int> readIntegers(string fileName)
        {
            List<int> data = new List<int>();
            foreach(string line in File.ReadLines("./input.txt"))
            {
                data.Add(Int32.Parse(line));
            }
            return data;
        }
    }
}
