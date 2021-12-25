using System;
using System.IO;
using System.Collections.Generic;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("input.txt");
            int solutionPart1 = 0;
            ulong solutionPart2 = 0;
            List<ulong> scores = new List<ulong>();

            foreach(string line in data)
            {
                Stack<char> stack = new Stack<char>();
                bool errorFound = false;
                foreach(char c in line)
                {
                    if(c == '(' || c == '[' || c == '{' || c == '<')
                    {
                        stack.Push(c);
                        continue;
                    }

                    char chunkOpen = stack.Pop();
                    if(!matchBrackets(chunkOpen, c))
                    {
                        solutionPart1 += getErrorScore(c);
                        errorFound = true;
                        break;
                    }
                }

                if(errorFound)
                    continue;

                ulong score = 0;
                while(stack.Count > 0)
                {
                    char chunkOpen = stack.Pop();
                    char chunkClose = findMatchingBracket(chunkOpen);
                    score = (5 * score) + getCompletingScore(chunkClose);
                }
                scores.Add(score);
            }

            scores.Sort();
            solutionPart2 = scores[(scores.Count - 1) / 2];

            Console.WriteLine("Day 10 part 1, result: " + solutionPart1);
            Console.WriteLine("Day 10 part 2, result: " + solutionPart2);
        }

        static bool matchBrackets(char open, char close)
        {
            if((open == '(' && close == ')')
            || (open == '[' && close == ']')
            || (open == '{' && close == '}')
            || (open == '<' && close == '>'))
                return true;
            else
                return false;
        }

        static char findMatchingBracket(char open)
        {
            switch(open)
            {
                case '(': return ')';
                case '[': return ']';
                case '{': return '}';
                case '<': return '>';
                default: return ' ';
            }
        }

        static int getErrorScore(char c)
        {
            switch(c)
            {
                case ')': return 3;
                case ']': return 57;
                case '}': return 1197;
                case '>': return 25137;
                default: return 0;
            }
        }

        static ulong getCompletingScore(char c)
        {
            switch(c)
            {
                case ')': return 1;
                case ']': return 2;
                case '}': return 3;
                case '>': return 4;
                default: return 0;
            }
        }
    }
}
