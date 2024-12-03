using System;
using System.Text.RegularExpressions;

namespace day03
{
    class Program
    {
        static void Main(string[] args)
        {
            int solutionPart1 = 0;
            int solutionPart2 = 0;
            string input = File.ReadAllText("input.txt");

            string pattern = @"mul\((?<num1>\d+),(?<num2>\d+)\)|do\(\)|don't\(\)";
            Regex reg = new Regex(pattern);
            MatchCollection matches = reg.Matches(input);

            bool enableInstruction = true;
            foreach(Match match in matches)
            {
                if(match.Value == "do()")
                    enableInstruction = true;
                else if (match.Value == "don't()")
                    enableInstruction = false;
                else
                {
                    int num1 = Int32.Parse(match.Groups["num1"].Value);
                    int num2 = Int32.Parse(match.Groups["num2"].Value);
                    solutionPart1 += num1 * num2;

                    if(enableInstruction)
                        solutionPart2 += num1 * num2;
                }
            }

            /* Part 1 */
            Console.WriteLine("Day 03 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 03 part 2, result: " + solutionPart2);
        }
    }
}
