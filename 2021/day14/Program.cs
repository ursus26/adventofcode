using System;
using System.IO;
using System.Collections.Generic;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllText("input.txt").Trim().Split("\n\n");
            Dictionary<string, ulong> polymerPairs = new Dictionary<string, ulong>();
            Dictionary<string, char> rules = new Dictionary<string, char>();
            foreach(string line in data[1].Split('\n'))
            {
                string[] splittedRule = line.Split(" -> ");
                rules[splittedRule[0]] = splittedRule[1][0];
                // Console.WriteLine("{0}: {1}", splittedRule[1], splittedRule[1].Length);
            }

            string polymerPattern = data[0];
            for(int i = 1; i < polymerPattern.Length; i++)
            {
                string key = "" + polymerPattern[i-1] + polymerPattern[i];
                if(!polymerPairs.ContainsKey(key))
                    polymerPairs[key] = 0;

                polymerPairs[key] += 1;
            }

            for(int i = 0; i < 10; i++)
                polymerPairs = step(polymerPairs, rules);

            ulong solutionPart1 = countSolution(polymerPairs, polymerPattern);
            Console.WriteLine("Day 14 part 1, result: " + solutionPart1);

            for(int i = 10; i < 40; i++)
                polymerPairs = step(polymerPairs, rules);

            ulong solutionPart2 = countSolution(polymerPairs, polymerPattern);
            Console.WriteLine("Day 14 part 2, result: " + solutionPart2);
        }

        static Dictionary<string, ulong> step(Dictionary<string, ulong> polymerPairs, Dictionary<string, char> rules)
        {
            Dictionary<string, ulong> nextStep = new Dictionary<string, ulong>();
            foreach(string key in rules.Keys)
                nextStep[key] = 0;

            foreach(var item in polymerPairs)
            {
                char c = rules[item.Key];
                nextStep["" + item.Key[0] + c] += item.Value;
                nextStep["" + c + item.Key[1]] += item.Value;
            }

            return nextStep;
        }

        static ulong countSolution(Dictionary<string, ulong> polymerPairs, string startPattern)
        {
            Dictionary<char, ulong> counter = new Dictionary<char, ulong>();
            foreach(var item in polymerPairs)
            {
                char c1 = item.Key[0];
                if(!counter.ContainsKey(c1))
                    counter[c1] = 0;
                counter[c1] += item.Value;

                char c2 = item.Key[1];
                if(!counter.ContainsKey(c2))
                    counter[c2] = 0;
                counter[c2] += item.Value;
            }
            char firstChar = startPattern[0];
            counter[firstChar] += 1;
            char lastChar = startPattern[startPattern.Length - 1];
            counter[lastChar] += 1;
            foreach(char k in counter.Keys)
                counter[k] = (counter[k] / 2ul);

            /* Find min and max count. */
            ulong minCount = UInt64.MaxValue;
            ulong maxCount = UInt64.MinValue;
            foreach(ulong count in counter.Values)
            {
                minCount = (count < minCount) ? count : minCount;
                maxCount = (count > maxCount) ? count : maxCount;
            }
            return maxCount - minCount;
        }
    }
}
