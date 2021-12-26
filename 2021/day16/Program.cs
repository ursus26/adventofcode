using System;
using System.IO;
using System.Collections.Generic;

namespace day16
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = File.ReadAllText("input.txt").Trim();
            string bitSequence = hexStringToBitString(data);

            Packet p = new Packet(bitSequence);
            long solutionPart1 = p.sumVersionIds();
            Console.WriteLine("Day 16 part 1, result: " + solutionPart1);

            long solutionPart2 = p.solve();
            Console.WriteLine("Day 16 part 2, result: " + solutionPart2);
        }

        static string hexStringToBitString(string hexString)
        {
            string bitString = "";
            Dictionary<char, string> lookupTable = new Dictionary<char, string>()
            {
                {'0', "0000"}, {'1', "0001"}, {'2', "0010"}, {'3', "0011"},
                {'4', "0100"}, {'5', "0101"}, {'6', "0110"}, {'7', "0111"},
                {'8', "1000"}, {'9', "1001"}, {'A', "1010"}, {'B', "1011"},
                {'C', "1100"}, {'D', "1101"}, {'E', "1110"}, {'F', "1111"}
            };

            foreach(char hex in hexString)
                bitString += lookupTable[hex];
            return bitString;
        }
    }

    class Packet
    {
        private long typeId;
        public long version { get; }
        public long size { get; set; }
        public List<Packet> subpackets;
        private long literalValue;

        public Packet(string bitSequence)
        {
            this.version = bitStringToLong(bitSequence, 3);
            this.typeId = bitStringToLong(bitSequence.Substring(3), 3);
            this.size = 6;
            this.subpackets = new List<Packet>();
            this.literalValue = 0;

            if(typeId == 4)
                parseLiteral(bitSequence.Substring(6));
            else
                parseSubPackets(bitSequence.Substring(6));
        }

        public long solve()
        {
            switch(typeId)
            {
                case 0:
                    long sum = 0;
                    foreach(Packet p in this.subpackets)
                        sum += p.solve();
                    return sum;
                case 1:
                    long product = 1;
                    foreach(Packet p in this.subpackets)
                        product *= p.solve();
                    return product;
                case 2:
                    long minimum = long.MaxValue;
                    foreach(Packet p in this.subpackets)
                    {
                        long value = p.solve();
                        minimum = (value < minimum) ? value : minimum;
                    }
                    return minimum;
                case 3:
                    long maximum = long.MinValue;
                    foreach(Packet p in this.subpackets)
                    {
                        long value = p.solve();
                        maximum = (value > maximum) ? value : maximum;
                    }
                    return maximum;
                case 4:
                    return this.literalValue;
                case 5:
                    return (this.subpackets[0].solve() > this.subpackets[1].solve()) ? 1u : 0u;
                case 6:
                    return (this.subpackets[0].solve() < this.subpackets[1].solve()) ? 1u : 0u;
                case 7:
                    return (this.subpackets[0].solve() == this.subpackets[1].solve()) ? 1u : 0u;
                default:
                    Console.WriteLine("!!!!!!!!!!!!!!!!!!!!");
                    return 0;
            }
        }

        public static long bitStringToLong(string bitSequence, int size)
        {
            long value = 0;
            for(int i = 0; i < size; i++)
            {
                if(bitSequence[i] == '1')
                    value += (1u << (size - i - 1));
            }
            return value;
        }

        private void parseLiteral(string bitSequence)
        {
            long value = 0;
            bool stopParsing = false;
            while(!stopParsing)
            {
                if(bitSequence[0] == '0')
                    stopParsing = true;

                long hexValue = bitStringToLong(bitSequence.Substring(1), 4);
                value = (value << 4) | hexValue;
                bitSequence = bitSequence.Substring(5);
                this.size += 5;
            }
            this.literalValue = value;
        }

        private void parseSubPackets(string bitSequence)
        {
            long sumSubpacketSizes = 0;
            if(bitSequence[0] == '0')
            {
                long payloadSize = bitStringToLong(bitSequence.Substring(1), 15);
                do
                {
                    Packet p = new Packet(bitSequence.Substring(16 + (int)sumSubpacketSizes));
                    subpackets.Add(p);
                    sumSubpacketSizes += p.size;
                } while(sumSubpacketSizes != payloadSize);
                this.size += 16 + sumSubpacketSizes;
            }
            else
            {
                long subpacketCount = bitStringToLong(bitSequence.Substring(1), 11);
                for(int i = 0; i < subpacketCount; i++)
                {
                    Packet p = new Packet(bitSequence.Substring(12 + (int)sumSubpacketSizes));
                    subpackets.Add(p);
                    sumSubpacketSizes += p.size;
                }
                this.size += 12 + sumSubpacketSizes;
            }
        }

        public long sumVersionIds()
        {
            long sum = this.version;
            foreach(Packet p in this.subpackets)
                sum += p.sumVersionIds();
            return sum;
        }
    }
}
