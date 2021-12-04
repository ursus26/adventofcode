using System;
using System.IO;
using System.Collections.Generic;

namespace day03
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> data = new List<string>(File.ReadAllLines("input.txt"));

            int solutionPart1 = part1(data);
            Console.WriteLine("Day 3 part 1, result: " + solutionPart1);

            int solutionPart2 = part2(data);
            Console.WriteLine("Day 3 part 2, result: " + solutionPart2);
        }


        static int part1(List<string> data)
        {
            int[] count = new int[data[0].Length];
            foreach(string line in data)
                for(int i = 0; i < line.Length; i++)
                    count[i] += line[i] == '1' ? 1 : -1;

            int gamma = 0;
            int epsilon = 0;
            for(int i = 0; i < count.Length; i++)
            {
                if(count[i] > 0)
                    gamma |= (1 << (count.Length - i - 1));
            }

            int bitmask = 0;
            for(int i = 0; i < data[0].Length; i++)
                bitmask |= 1 << i;
            epsilon = ~gamma & bitmask;
            return gamma * epsilon;
        }


        static int part2(List<string> data)
        {
            int count = 0;
            int lineLength = data[0].Length;
            List<string> list1 = new List<string>(data);
            List<string> list2 = new List<string>(data);

            /* Most common */
            for(int i = 0; (i < lineLength && list1.Count > 1); i++)
            {
                count = 0;
                foreach(string line in list1)
                    count += line[i] == '1' ? 1 : -1;

                char removeChar = count >= 0 ? '0' : '1';
                list1.RemoveAll(elem => elem[i] == removeChar);
            }

            /* Least common */
            for(int i = 0; (i < lineLength && list2.Count > 1); i++)
            {
                count = 0;
                foreach(string line in list2)
                    count += line[i] == '1' ? 1 : -1;

                char removeChar = count >= 0 ? '1' : '0';
                list2.RemoveAll(elem => elem[i] == removeChar);
            }

            int oxygen = bitStringToInt(list1[0]);
            int co2 = bitStringToInt(list2[0]);
            return oxygen * co2;
        }


        static int bitStringToInt(string s)
        {
            int output = 0;
            for(int i = 0; i < s.Length; i++)
            {
                if(s[i] == '1')
                    output |= (1 << (s.Length - i - 1));
            }
            return output;
        }
    }
}
