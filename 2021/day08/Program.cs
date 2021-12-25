using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day08
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("input.txt");
            int solutionPart1 = 0;
            int solutionPart2 = 0;

            foreach(string line in data)
            {
                string[] words = line.Split(" ");
                List<HashSet<char>> group5 = new List<HashSet<char>>();
                List<HashSet<char>> group6 = new List<HashSet<char>>();
                HashSet<char>[] digits = new HashSet<char>[10];

                /* Find digits 1, 4, 7, and 8. */
                for(int i = 0; i < 10; i++)
                {
                    HashSet<char> newDigit = new HashSet<char>();
                    foreach(char c in words[i])
                        newDigit.Add(c);

                    switch(words[i].Length)
                    {
                        case 2:
                            digits[1] = newDigit;
                            break;
                        case 3:
                            digits[7] = newDigit;
                            break;
                        case 4:
                            digits[4] = newDigit;
                            break;
                        case 5:
                            group5.Add(newDigit);
                            break;
                        case 6:
                            group6.Add(newDigit);
                            break;
                        case 7:
                            digits[8] = newDigit;
                            break;
                    }
                }

                /* Solve all digits with 6 lines. */
                HashSet<char> digit9 = deduceDigit9(digits[4], group6);
                group6.Remove(digit9);
                digits[9] = digit9;

                HashSet<char> digit0 = deduceDigit0(digits[1], group6);
                group6.Remove(digit0);
                digits[0] = digit0;

                digits[6] = group6[0];

                /* Solve all digits with 5 lines. */
                HashSet<char> digit3 = deduceDigit3(digits[1], group5);
                group5.Remove(digit3);
                digits[3] = digit3;

                HashSet<char> digit5 = deduceDigit5(digits[6], group5);
                group5.Remove(digit5);
                digits[5] = digit5;

                digits[2] = group5[0];

                /* Solve the output number. */
                int output = 0;
                for(int i = 11; i < words.Length; i++)
                {
                    HashSet<char> outputDigit = new HashSet<char>();
                    foreach(char c in words[i])
                        outputDigit.Add(c);

                    int length = outputDigit.Count;
                    if(length == 2 || length == 3 || length == 4 || length == 7)
                        solutionPart1++;

                    for(int j = 0; j < 10; j++)
                    {
                        if(digits[j].SetEquals(outputDigit))
                        {
                            output += j * (int)Math.Pow(10, (4 - (i - 10)));
                            break;
                        }
                    }

                }
                solutionPart2 += output;
            }

            Console.WriteLine("Day 8 part 1, result: " + solutionPart1);
            Console.WriteLine("Day 8 part 2, result: " + solutionPart2);
        }

        static HashSet<char> deduceDigit9(HashSet<char> digit4, List<HashSet<char>> group6)
        {
            foreach(HashSet<char> elem in group6)
            {
                if(digit4.IsSubsetOf(elem))
                    return elem;
            }
            return null;
        }

        static HashSet<char> deduceDigit0(HashSet<char> digit1, List<HashSet<char>> group6)
        {
            foreach(HashSet<char> elem in group6)
            {
                if(digit1.IsSubsetOf(elem))
                    return elem;
            }
            return null;
        }

        static HashSet<char> deduceDigit3(HashSet<char> digit1, List<HashSet<char>> group5)
        {
            foreach(HashSet<char> elem in group5)
            {
                if(digit1.IsSubsetOf(elem))
                    return elem;
            }
            return null;
        }

        static HashSet<char> deduceDigit5(HashSet<char> digit6, List<HashSet<char>> group5)
        {
            foreach(HashSet<char> elem in group5)
            {
                HashSet<char> digit6Copy = new HashSet<char>(digit6);
                digit6Copy.ExceptWith(elem);
                if(digit6Copy.Count == 1)
                    return elem;
            }
            return null;
        }

    }
}

/*

1,4,7 and 8 have string lengths 2, 4, 3 and 7 respectively. They are most easiliy deduced.
If you know 4 you can identify 9 because of all the digits with 6 lines, the number 9 contains all lines of 4. 0 and 6 do not.
Then 1 can identify 0 because the lines of 0 contain the lines of 1.
This leaves 6 as the only digits with 6 lines.
At this point you should know 0, 1, 4, 6, 7, 8, 9. Which leaves 2, 3, and 5 unknown with all three length 5.
1 can identify 3 because neither 2 or 5 contain all lines of 1. But 3 contains all lines also present in 1.
6 can identify the last 2 digits: 2 and 5. The difference between 2 and 6 is 2 lines. The difference between 5 and 6 is 1 line.

1, 4, 7, 8

Group of 6 lines
4->9
1->6
0 is left over as the only digits with 6 lines.

Group of 5 lines
1->3
6->2,5

*/
