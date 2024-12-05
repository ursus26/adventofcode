namespace day05
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");

            /* Input parsing. */
            bool parsingRules = true;
            Dictionary<int, Rule> rules = new Dictionary<int, Rule>();
            List<int[]> updates = new List<int[]>();
            foreach(string line in lines)
            {
                if(line == "")
                {
                    parsingRules = false;
                    continue;
                }

                if(parsingRules)
                {
                    string[] split = line.Split('|');
                    int num1 = Int32.Parse(split[0]);
                    int num2 = Int32.Parse(split[1]);

                    Rule r1;
                    Rule r2;

                    if(!rules.TryGetValue(num1, out r1))
                    {
                        r1 = new Rule(num1);
                        rules.Add(num1, r1);
                    }

                    if(!rules.TryGetValue(num2, out r2))
                    {
                        r2 = new Rule(num2);
                        rules.Add(num2, r2);
                    }

                    r1.AddAfter(num2);
                    r2.AddBefore(num1);
                }
                else
                {
                    int[] pageNumbers = line.Split(',').Select(s => Int32.Parse(s)).ToArray();
                    updates.Add(pageNumbers);
                }
            }

            /* Validating the updates. */
            int solutionPart1 = 0;
            int solutionPart2 = 0;
            foreach(int[] update in updates)
            {
                bool validUpdate = true;
                for(int i = 0; i < update.Length; i++)
                {
                    if(!rules.ContainsKey(update[i]))
                        continue;

                    Rule r = rules[update[i]];
                    if(!r.Validate(update))
                    {
                        validUpdate = false;
                        break;
                    }
                }

                if(validUpdate)
                {
                    solutionPart1 += update[((update.Length - 1) / 2)];
                }
                else
                {
                    int[] fixedUpdate = BubbleSort(update, rules);
                    solutionPart2 += fixedUpdate[((fixedUpdate.Length - 1) / 2)];
                }
            }

            /* Part 1 */
            Console.WriteLine("Day 05 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 05 part 2, result: " + solutionPart2);
        }

        static public int[] BubbleSort(int[] numbers, Dictionary<int, Rule> rules)
        {
            bool sorted = false;
            while(!sorted)
            {
                sorted = true;
                for(int i = 0; i < numbers.Length -1 ; i++)
                {
                    int num1 = numbers[i];
                    int num2 = numbers[i + 1];
                    if(!rules.ContainsKey(num1) || !rules.ContainsKey(num2))
                        continue;

                    Rule r = rules[num1];
                    int compare = r.Compare(num2);
                    if(compare < 0)
                    {
                        numbers[i] = num2;
                        numbers[i + 1] = num1;
                        sorted = false;
                    }
                }
            }
            return numbers;
        }
    }


    public class Rule
    {
        int MyNumber = 0;
        private HashSet<int> NumbersBefore = new HashSet<int>();
        private HashSet<int> NumbersAfter = new HashSet<int>();

        public Rule(int number)
        {
            MyNumber = number;
        }

        public void AddBefore(int number)
        {
            NumbersBefore.Add(number);
        }

        public void AddAfter(int number)
        {
            NumbersAfter.Add(number);
        }

        public bool Validate(int[] numbers)
        {
            int myIdx = Array.IndexOf(numbers, MyNumber);

            for(int i = 0; i < myIdx; i++)
            {
                if(NumbersAfter.Contains(numbers[i]))
                    return false;
            }

            for(int j = myIdx + 1; j < numbers.Length; j++)
            {
                if(NumbersBefore.Contains(numbers[j]))
                    return false;
            }

            return true;
        }

        /* Return similar output as the strcmp funciton in C. */
        public int Compare(int number)
        {
            if(NumbersBefore.Contains(number))
                return -1;
            else if(NumbersAfter.Contains(number))
                return 1;
            else
                return 0;
        }
    }
}
