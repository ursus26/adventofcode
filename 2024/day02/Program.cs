namespace day02
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Input parsing. */
            string[] lines = File.ReadAllLines("input.txt");
            int solutionPart1 = 0;
            int solutionPart2 = 0;
            foreach(string line in lines)
            {
                Report r = new Report(line);
                if(r.IsSafe())
                {
                    solutionPart1++;
                }

                if(r.IsSafeWithProblemDampener())
                {
                    solutionPart2++;
                }
            }

            /* Part 1 */
            Console.WriteLine("Day 02 part 1, result: " + solutionPart1);

            /* Part 2 */
            Console.WriteLine("Day 02 part 2, result: " + solutionPart2);
        }

    }

    public class Report
    {
        private List<int> numbers;

        public Report(string line)
        {
            numbers = new List<int>();
                
            string[] items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach(string s in items)
            {
                int num = Int32.Parse(s);
                numbers.Add(num);
            }
        }

        public bool IsSafe()
        {
            return IsSafeAscending() || IsSafeDescending();
        }

        public bool IsSafeWithProblemDampener()
        {
            return IsSafeAscendingWithProblemDampener() || IsSafeDescendingWithProblemDampener();
        }

        bool IsSafeAscending()
        {
            for(int i = 1; i < numbers.Count; i++)
            {
                int num1 = numbers[i-1];
                int num2 = numbers[i];
                if(num1 > num2 || !IsSafeDiff(num1, num2))
                    return false;
            }
            return true;
        }

        bool IsSafeAscendingWithProblemDampener()
        {
            for(int i = 1; i < numbers.Count; i++)
            {
                int num1 = numbers[i-1];
                int num2 = numbers[i];
                if(num1 > num2 || !IsSafeDiff(num1, num2))
                {
                    numbers.RemoveAt(i-1);
                    bool result1 = IsSafeAscending();
                    numbers.Insert(i-1, num1);

                    numbers.RemoveAt(i);
                    bool result2 = IsSafeAscending();
                    numbers.Insert(i, num2);

                    return result1 || result2;
                }
            }
            return true;
        }


        bool IsSafeDescending()
        {
            for(int i = 1; i < numbers.Count; i++)
            {
                int num1 = numbers[i-1];
                int num2 = numbers[i];
                if(num1 < num2 || !IsSafeDiff(num1, num2))
                    return false;
            }
            return true;
        }

        bool IsSafeDescendingWithProblemDampener()
        {
            for(int i = 1; i < numbers.Count; i++)
            {
                int num1 = numbers[i-1];
                int num2 = numbers[i];

                if(num1 < num2 || !IsSafeDiff(num1, num2))
                {
                    numbers.RemoveAt(i-1);
                    bool result1 = IsSafeDescending();
                    numbers.Insert(i-1, num1);

                    numbers.RemoveAt(i);
                    bool result2 = IsSafeDescending();
                    numbers.Insert(i, num2);

                    return result1 || result2;
                }
            }
            return true;
        }

        bool IsSafeDiff(int num1, int num2)
        {
            int diff = Math.Abs(num1 - num2);
            if(1 <= diff && diff <= 3)
                return true;
            else
                return false;
        }
    }
}
