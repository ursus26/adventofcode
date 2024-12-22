using System;

namespace day17
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");

            string[] splits = input.Split("\n\n");
            string[] registersInput = splits[0].Split("\n");
            long regA = Convert.ToInt64(registersInput[0].Substring(12));
            long regB = Convert.ToInt64(registersInput[1].Substring(12));
            long regC = Convert.ToInt64(registersInput[2].Substring(12));
            List<int> digits = splits[1].Where(c => Char.IsDigit(c)).Select(c => (int)Char.GetNumericValue(c)).ToList();

            /* Part 1 */
            Computer c = new Computer(digits, regA, regB, regC);
            string solutionPart1 = c.Run();
            Console.WriteLine("Day 17 part 1, result: " + solutionPart1);

            /* Part 2 */
            long solutionPart2 = BackwardsSearch(digits);
            Console.WriteLine("Day 17 part 2, result: " + solutionPart2);

            // string expectedOutput = splits[1].Substring(9);
            // do
            // {
            //     if(solutionPart2 % 1_000_000_000 == 0)
            //         Console.WriteLine("{0}: {1}", solutionPart2, new Computer(digits, solutionPart2, regB, regC).Run());

            //     solutionPart2++;

            //     long RegA = solutionPart2;
            //     long RegB = 0;
            //     long RegC = 0;
            //     int outputIter = 0;

            //     do
            //     {
            //         RegB = RegA % 8;
            //         RegB ^= 0b011;
            //         RegC = RegA >> (int)RegB;
            //         RegB ^= 0b101;

            //         long value = (RegB ^ RegC) & 0b111;
            //         if(value != digits[outputIter])
            //             break;
            //         outputIter++;

            //         RegA >>= 3;
            //     } while(RegA != 0);

            //     if(outputIter == digits.Count)
            //         break;

            // } while(true);
        }

        static long BackwardsSearch(List<int> expectedResult)
        {
            expectedResult.Reverse();
            List<long> solutions = new List<long>();
            Stack<(long, int)> stack = new Stack<(long, int)>();
            stack.Push((0, 0));

            while(stack.Count > 0)
            {
                (long prefix, int depth) = stack.Pop();

                if(depth == expectedResult.Count)
                {
                    solutions.Add(prefix >> 3);
                    continue;
                }

                long target = (long)expectedResult[depth];
                for(long i = 0L; i < 1L << 3; i++)
                {
                    long RegA = prefix | i;
                    bool result = ((RegA % 8) ^ 0b011 ^ 0b101 ^ (RegA >> (int)((RegA % 8) ^ 0b011)) % 8) == target;
                    if(result)
                        stack.Push((RegA << 3, depth + 1));
                }
            }

            expectedResult.Reverse();
            return solutions.Min();
        }
    }

    public class Computer
    {
        private long RegA;
        private long RegB;
        private long RegC;
        private List<int> Program;
        private int IP;
        private string Stdout = String.Empty;

        public Computer(List<int> program, long regA, long regB, long regC)
        {
            Program = program;
            RegA = regA;
            RegB = regB;
            RegC = regC;
            IP = 0;
        }

        public string Run()
        {
            while(IP >= 0 && IP < Program.Count)
            {
                long opcode = Program[IP];
                long operand = Program[IP + 1];

                switch (opcode)
                {
                    case 0:
                        adv(operand);
                        break;
                    case 1:
                        bxl(operand);
                        break;
                    case 2:
                        bst(operand);
                        break;
                    case 3:
                        jnz(operand);
                        break;
                    case 4:
                        bxc(operand);
                        break;
                    case 5:
                        output(operand);
                        break;
                    case 6:
                        bdv(operand);
                        break;
                    case 7:
                        cdv(operand);
                        break;
                }

                IP += 2;
            }

            return Stdout;
        }

        public bool RunAlgorithm(long regA, long regB, long regC)
        {
            // START:
            //     bst: RegB = RegA mod 8          // Get last 3 bits
            //     bxl: RegB = RegB ^ 0b011        // Flip the lower 2 bits of RegA mod 8
            //     cdv: RegC = RegA / (2**RegB)    // RegB can only be 0-7. Therefore 2**RegB is only 1, 2, 4, 8, 16, 32, 64, 128
            //     bxl: RegB = RegB ^ 0b101        // Flip bits 0b110 of RegA mod 8
            //     bxc: RegB = RegB ^ RegC
            //     out: RegB mod 8
            //     adv: RegA = RegA / 0x8          // Shift RegA by 3
            //     jnz: if RegA != 0 Then jump to START

            Stdout = String.Empty;
            RegA = regA;
            RegB = regB;
            RegC = regC;
            IP = 0;
            int outputIter = 0;

            do
            {
                RegB = RegA % 8;
                RegB ^= 0b011;
                RegC = RegA >> (int)RegB;
                RegB ^= 0b101;

                long value = (RegB ^ RegC) & 0b111;
                if(value != Program[outputIter])
                    break;
                outputIter++;

                RegA >>= 3;
            } while(RegA != 0);

            return outputIter == Program.Count;
        }

        private long GetComboOperatondValue(long operand)
        {
            if(operand < 4)
                return operand;
            else if(operand == 4)
                return RegA;
            else if(operand == 5)
                return RegB;
            else if(operand == 6)
                return RegC;
            else
                throw new ArgumentOutOfRangeException();
        }

        private void adv(long operand)
        {
            long numerator = RegA;
            long denominator = (long)Math.Pow(2, GetComboOperatondValue(operand));
            RegA = numerator / denominator;
        }

        private void bxl(long operand)
        {
            RegB ^= operand;
        }

        private void bst(long operand)
        {
            long comboOperand = GetComboOperatondValue(operand);
            RegB = comboOperand % 8;
        }

        private void jnz(long operand)
        {
            if(RegA != 0)
                IP = (int)operand - 2;
        }

        private void bxc(long operand)
        {
            RegB ^= RegC;
        }

        private void output(long operand)
        {
            long value = GetComboOperatondValue(operand) % 8;
            string output = value.ToString();
            if(Stdout != String.Empty)
                Stdout = String.Concat(Stdout, ",", output);
            else
                Stdout = output;
        }

        private void bdv(long operand)
        {
            long numerator = RegA;
            long denominator = (long)Math.Pow(2, GetComboOperatondValue(operand));
            RegB = numerator / denominator;
        }

        private void cdv(long operand)
        {
            long numerator = RegA;
            long denominator = (long)Math.Pow(2, GetComboOperatondValue(operand));
            RegC = numerator / denominator;
        }
    }
}
