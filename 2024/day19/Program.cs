using System;
using System.Text.RegularExpressions;

namespace day19
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");
            string[] splits = input.Split("\n\n");
            List<string> patterns = splits[0].Split(", ").ToList();
            List<string> lines = splits[1].Split("\n").ToList();

            long solutionPart1 = 0;
            long solutionPart2 = 0;
            foreach(string line in lines)
            {
                Dictionary<string, long> history = new Dictionary<string, long>();
                for(int i = line.Length - 1; i >= 0; i--)
                {
                    string substring = line.Substring(i);
                    history[substring] = 0;

                    foreach(string pattern in patterns)
                    {
                        if(substring.StartsWith(pattern))
                        {
                            if(pattern.Length == substring.Length)
                            {
                                history[substring]++;
                            }
                            else
                            {
                                string subsubstring = substring.Substring(pattern.Length);
                                history[substring] += history[subsubstring];
                            }
                        }
                    }
                }

                long solutions = history[line];
                solutionPart1 += solutions > 0 ? 1 : 0;
                solutionPart2 += solutions;
            }

            /* Part 1 */
            Console.WriteLine("Day 19 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 19 part 2, result: " + solutionPart2);
        }
    }
}
