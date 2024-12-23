using System;

namespace day22
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");

            long solutionPart1 = 0;
            Dictionary<(long, long, long, long), long> profit = new Dictionary<(long, long, long, long), long>();
            foreach(string line in input)
            {
                long initialNumber = Convert.ToInt64(line);
                long currentSecret = initialNumber;
                long nextSecret = 0;

                List<long> changes = new List<long>();
                List<long> prices = new List<long>();
                for(int i = 0; i < 2000; i++)
                {
                    long currentPrice = currentSecret % 10;
                    prices.Add(currentPrice);

                    nextSecret = NextSecret(currentSecret);
                    long nextPrice = nextSecret % 10;
                    long change = nextPrice - currentPrice;
                    changes.Add(change);

                    currentSecret = nextSecret;
                }
                prices.Add(currentSecret % 10);

                HashSet<(long, long, long, long)> processedSequences = new HashSet<(long, long, long, long)>();
                for(int i = 0; i < changes.Count - 3; i++)
                {
                    (long, long, long, long) sequence = (changes[i], changes[i+1], changes[i+2], changes[i+3]);
                    long price = prices[i+4];

                    if(!processedSequences.Contains(sequence))
                    {
                        processedSequences.Add(sequence);
                        
                        if(!profit.ContainsKey(sequence))
                            profit[sequence] = 0;    
                        profit[sequence] += price;
                    }
                }

                solutionPart1 += currentSecret;
            }
            
            /* Part 1 */
            Console.WriteLine("Day 22 part 1, result: " + solutionPart1);

            /* Part 2 */
            long solutionPart2 = profit.Select(x => x.Value).Max();
            Console.WriteLine("Day 22 part 2, result: " + solutionPart2);
        }

        static long NextSecret(long num)
        {
            long secret = MixAndPrune(num, 64 * num);
            secret = MixAndPrune(secret, secret / 32);
            secret = MixAndPrune(secret, 2048 * secret);
            return secret;
        }

        static long MixAndPrune(long secret, long num)
        {
            return (secret ^ num) % 16777216;
        }
    }
}