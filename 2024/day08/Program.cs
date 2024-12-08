namespace day08
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("input.txt");
            int gridHeight = input.Length;
            int gridWidth = input[0].Length;

            Dictionary<char, List<(int, int)>> antennas = new Dictionary<char, List<(int, int)>>();
            for(int i = 0; i < gridHeight; i++)
            {
                string line = input[i];
                for(int j = 0; j < gridWidth; j++)
                {
                    char c = line[j];
                    if(c == '.')
                        continue;

                    if(!antennas.ContainsKey(c))
                        antennas.Add(c, new List<(int, int)>());

                    List<(int, int)> positions = antennas[c];
                    positions.Add((j, i));
                }
            }

            /* Evaluate every type of antenna. */
            HashSet<(int, int)> antiNodesPart1 = new HashSet<(int, int)>();
            HashSet<(int, int)> antiNodesPart2 = new HashSet<(int, int)>();
            foreach(KeyValuePair<char, List<(int, int)>> kvp in antennas)
            {
                List<(int, int)> antennaPositions = kvp.Value;

                /* Evaluate every pair of antenna of the same type. */
                for(int i = 0; i < antennaPositions.Count; i++)
                {
                    (int x1, int y1) = antennaPositions[i];
                    for(int j = i + 1; j < antennaPositions.Count; j++)
                    {
                        (int x2, int y2) = antennaPositions[j];
                        int dx = x1 - x2;
                        int dy = y1 - y2;

                        antiNodesPart2.Add((x1, y1));
                        int xAntiNode1 = x1 + dx;
                        int yAntiNode1 = y1 + dy;
                        if(xAntiNode1 >= 0 && xAntiNode1 < gridWidth && yAntiNode1 >= 0 && yAntiNode1 < gridHeight)
                        {
                            antiNodesPart1.Add((xAntiNode1, yAntiNode1));

                            while(xAntiNode1 >= 0 && xAntiNode1 < gridWidth && yAntiNode1 >= 0 && yAntiNode1 < gridHeight)
                            {
                                antiNodesPart2.Add((xAntiNode1, yAntiNode1));
                                xAntiNode1 += dx;
                                yAntiNode1 += dy;
                            }
                        }
                        
                        antiNodesPart2.Add((x2, y2));
                        int xAntiNode2 = x2 - dx;
                        int yAntiNode2 = y2 - dy;
                        if(xAntiNode2 >= 0 && xAntiNode2 < gridWidth && yAntiNode2 >= 0 && yAntiNode2 < gridHeight)
                        {
                            antiNodesPart1.Add((xAntiNode2, yAntiNode2));

                            while(xAntiNode2 >= 0 && xAntiNode2 < gridWidth && yAntiNode2 >= 0 && yAntiNode2 < gridHeight)
                            {
                                antiNodesPart2.Add((xAntiNode2, yAntiNode2));
                                xAntiNode2 -= dx;
                                yAntiNode2 -= dy;
                            }
                        }
                    }
                }
            }

            /* Part 1 */
            long solutionPart1 = antiNodesPart1.Count;
            Console.WriteLine("Day 08 part 1, result: " + solutionPart1);

            /* Part 2 */
            long solutionPart2 = antiNodesPart2.Count;
            Console.WriteLine("Day 08 part 2, result: " + solutionPart2);
        }
    }
}
