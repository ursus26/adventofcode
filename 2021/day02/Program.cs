using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day02
{
    class Program
    {
        static void Main(string[] args)
        {
            List<(string, int)> data = readInput("./input.txt");
            int solutionPart1 = part1(data);
            int solutionPart2 = part2(data);
            Console.WriteLine("Day 2 part 1, result: " + solutionPart1);
            Console.WriteLine("Day 2 part 2, result: " + solutionPart2);
        }


        static int part1(List<(string, int)> input)
        {
            int x = 0;
            int y = 0;
            foreach((string, int) command in input) {
                switch(command.Item1) {
                    case "forward":
                        x += command.Item2;
                        break;

                    case "up":
                        y -= command.Item2;
                        break;

                    case "down":
                        y += command.Item2;
                        break;

                    default:
                        break;
                }
            }
            return x * y;
        }


        static int part2(List<(string, int)> input)
        {
            int x = 0;
            int y = 0;
            int aim = 0;
            foreach((string, int) command in input) {
                switch(command.Item1) {
                    case "forward":
                        x += command.Item2;
                        y += aim * command.Item2;
                        break;

                    case "up":
                        aim -= command.Item2;
                        break;

                    case "down":
                        aim += command.Item2;
                        break;

                    default:
                        break;
                }
            }
            return x * y;
        }


        static List<(string, int)> readInput(string fileName)
        {
            List<(string, int)> data = new List<(string, int)>();
            string pattern = @"(forward|up|down)\s(\d+)";
            Regex regex = new Regex(pattern);
            string text = File.ReadAllText(fileName);

            foreach(Match match in regex.Matches(text))
            {
                Group group1 = match.Groups[1];
                Group group2 = match.Groups[2];
                data.Add((group1.Value, Int32.Parse(group2.Value)));
            }
            return data;
        }
    }
}
