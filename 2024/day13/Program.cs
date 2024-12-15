using System;
using System.Text.RegularExpressions;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            long solutionPart1 = 0;
            long solutionPart2 = 0;
            string input = File.ReadAllText("input.txt");

            string pattern = @"Button A: X\+(\d+), Y\+(\d+)\nButton\ B: X\+(\d+), Y\+(\d+)\nPrize: X=(\d+), Y=(\d+)";
            Regex reg = new Regex(pattern);
            MatchCollection matches = reg.Matches(input);

            foreach(Match match in matches)
            {
                long numerator1 = Convert.ToInt64(match.Groups[1].Value);
                long numerator2 = Convert.ToInt64(match.Groups[2].Value);
                long denominator1 = Convert.ToInt64(match.Groups[3].Value);
                long denominator2 = Convert.ToInt64(match.Groups[4].Value);
                long prizeX = Convert.ToInt64(match.Groups[5].Value);
                long prizeY = Convert.ToInt64(match.Groups[6].Value);

                long num3 = numerator1 * denominator1 * denominator2 * denominator2 * -1;
                long num4 = numerator2 * denominator2 * denominator1 * denominator1;
                long denominator3 = num3 + num4;

                long num5 = prizeX * denominator1 * denominator2 * denominator2;
                long num6 = prizeY * denominator2 * denominator1 * denominator1;
                long numerator3 = num6 - num5;

                long aCount = numerator3 / denominator3;
                long bCount = (prizeX - numerator1 * aCount) / denominator1;

                if(0 <= aCount && aCount <= 100 && 0 <= bCount && bCount <= 100)
                {
                    if(numerator1 * aCount + denominator1 * bCount == prizeX && numerator2 * aCount + denominator2 * bCount == prizeY)
                    {
                        solutionPart1 += 3 * aCount + bCount;
                    }
                }

                /* Part 2. */
                long prizeXPart2 = prizeX + 10000000000000;
                long prizeYPart2 = prizeY + 10000000000000;

                long num5Part2 = prizeXPart2 * denominator1 * denominator2 * denominator2;
                long num6Part2 = prizeYPart2 * denominator2 * denominator1 * denominator1;
                long numerator3Part2 = num6Part2 - num5Part2;

                long aCountPart2 = numerator3Part2 / denominator3;
                long bCountPart2 = (prizeXPart2 - numerator1 * aCountPart2) / denominator1;
                if(numerator1 * aCountPart2 + denominator1 * bCountPart2 == prizeXPart2 && numerator2 * aCountPart2 + denominator2 * bCountPart2 == prizeYPart2)
                {
                    solutionPart2 += 3 * aCountPart2 + bCountPart2;
                }
            }

            /* Part 1 */
            Console.WriteLine("Day 13 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 13 part 2, result: " + solutionPart2);
        }
    }
}
