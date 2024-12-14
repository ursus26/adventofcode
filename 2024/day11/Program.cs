namespace day11
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");
            Dictionary<string, long> numbers = new Dictionary<string, long>();
            foreach(string s in input.Split(' '))
            {
                if(!numbers.ContainsKey(s))
                    numbers[s] = 0;
                numbers[s] += 1;
            }

            /* Part 1 */
            int maxIterationsPart1 = 25;
            for(int i = 0; i < maxIterationsPart1; i++)
                numbers = Blink(numbers);
            long solutionPart1 = numbers.Select(x => x.Value).Sum();
            Console.WriteLine("Day 11 part 1, result: " + solutionPart1);

            /* Part 2 */
            int maxIterationsPart2 = 75;
            for(int i = maxIterationsPart1; i < maxIterationsPart2; i++)
                numbers = Blink(numbers);
            long solutionPart2 = numbers.Select(x => x.Value).Sum();
            Console.WriteLine("Day 11 part 2, result: " + solutionPart2);
        }

        static Dictionary<string, long> Blink(Dictionary<string, long> numbers)
        {
            Dictionary<string, long> nextNumbers = new Dictionary<string, long>();
            foreach(KeyValuePair<string, long> kvp in numbers)
            {
                string number = kvp.Key;
                long count = kvp.Value;

                if(number == "0")
                {
                    if(!nextNumbers.ContainsKey("1"))
                        nextNumbers["1"] = 0;
                    nextNumbers["1"] += count;
                } 
                else if(number.Length % 2 == 0)
                {
                    int length = number.Length / 2;
                    string s1 = number.Substring(0, length).TrimStart('0');
                    string s2 = number.Substring(0 + length, length).TrimStart('0');
                    if(s2 == "")
                        s2 = "0";
                    
                    if(!nextNumbers.ContainsKey(s1))
                        nextNumbers[s1] = 0;
                    nextNumbers[s1] += count;

                    if(!nextNumbers.ContainsKey(s2))
                        nextNumbers[s2] = 0;
                    nextNumbers[s2] += count;
                }
                else
                {
                    long tmp = Convert.ToInt64(number);
                    tmp *= 2024;
                    string newNumber = Convert.ToString(tmp);

                    if(!nextNumbers.ContainsKey(newNumber))
                        nextNumbers[newNumber] = 0;
                    nextNumbers[newNumber] += count;
                }
            }

            return nextNumbers;
        }
    }
}
