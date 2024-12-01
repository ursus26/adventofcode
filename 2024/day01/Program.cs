namespace day01
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Input parsing. */
            List<int> list1 = new List<int>();
            List<int> list2 = new List<int>();
            string[] lines = File.ReadAllLines("input.txt");
            foreach(string line in lines)
            {
                string[] items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                int num1 = Int32.Parse(items[0]);
                int num2 = Int32.Parse(items[1]);

                list1.Add(num1);
                list2.Add(num2);
            }

            /* Part 1 */
            int solutionPart1 = ListDistance(list1, list2);
            Console.WriteLine("Day 18 part 1, result: " + solutionPart1);

            /* Part 2 */
            int solutionPart2 = ListSimilarity(list1, list2);
            Console.WriteLine("Day 18 part 2, result: " + solutionPart2);
        }

        static int ListDistance(List<int> list1, List<int> list2)
        {
            list1.Sort();
            list2.Sort();

            int distance = 0;
            for(int i = 0; i < list1.Count; i++)
            {
                int diff = Math.Abs(list1[i] - list2[i]);
                distance += diff;
            }
            return distance;
        }

        static int ListSimilarity(List<int> list1, List<int> list2)
        {
            /* Group the same numbers in list 2 and count how many times they occure. */
            IEnumerable<IGrouping<int, int>> groups = list2.GroupBy(number => number);
            Dictionary<int, int> dict = new Dictionary<int, int>();
            foreach(IGrouping<int, int> group in groups)
            {
                dict.Add(group.Key, group.Count());
            }

            /* Calculate similarity score. */
            int similarity = 0;
            foreach(int x in list1)
            {
                if(dict.ContainsKey(x))
                {
                    similarity += x * dict[x];
                }
            }
            return similarity;
        }
    }
}
