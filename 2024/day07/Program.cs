using System;
using System.Text.RegularExpressions;

namespace day07
{
    class Program
    {
        static void Main(string[] args)
        {
            long solutionPart1 = 0;
            long solutionPart2 = 0;
            string input = File.ReadAllText("input.txt");

            string pattern = @"(?<testValue>\d+):(?<numbers>\ \d+)+";
            Regex reg = new Regex(pattern);
            MatchCollection matches = reg.Matches(input);

            foreach(Match match in matches)
            {
                /* Parsing. */
                long testValue = Convert.ToInt64(match.Groups["testValue"].Value);
                List<long> numbers = match.Groups["numbers"].Captures.Select(capture => Convert.ToInt64(capture.Value)).ToList();

                /* Part 1 & Part 2 equation checking. */
                if(TestEquation(testValue, numbers))
                {
                    solutionPart1 += testValue;
                    solutionPart2 += testValue;
                }
                else if(TestEquation2(testValue, numbers))
                {
                    solutionPart2 += testValue;
                }
            }

            /* Part 1 */
            Console.WriteLine("Day 07 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 07 part 2, result: " + solutionPart2);
        }

        static bool TestEquation(long testValue, List<long> numbers)
        {
            int permuationSize = (int) Math.Pow(2, numbers.Count - 1);
            for(int i = 0; i < permuationSize; i++)
            {
                long result = numbers[0];
                for(int j = 1; j < numbers.Count; j++)
                {
                    long num = numbers[j];
                    int bitSelector = 1 << (j-1);
                    if((i & bitSelector) == 0)
                        result += num;
                    else
                        result *= num;
                }

                if(result == testValue)
                    return true;
            }

            return false;
        }

        static bool TestEquation2(long testValue, List<long> numbers)
        {
            return RecursiveTestEquation(testValue, numbers, 1, numbers[0]);
        }

        static bool RecursiveTestEquation(long testValue, List<long> numbers, int depth, long partialAnswer)
        {
            if(depth >= numbers.Count)
                return testValue == partialAnswer;

            if(partialAnswer > testValue)
                return false;

            long num = numbers[depth];
            bool result1 = RecursiveTestEquation(testValue, numbers, depth + 1, partialAnswer + num);
            bool result2 = RecursiveTestEquation(testValue, numbers, depth + 1, partialAnswer * num);

            long nextPartial = partialAnswer * (long) Math.Pow(10, Convert.ToString(num).Length) + num;
            bool result3 = RecursiveTestEquation(testValue, numbers, depth + 1, nextPartial);
            return result1 || result2 || result3;
        }
    }
}
