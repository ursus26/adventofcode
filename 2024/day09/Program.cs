namespace day09
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("input.txt");

            List<int> memory = new List<int>();
            for(int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                int num = (int)Char.GetNumericValue(c);

                for(int j = 0; j < num; j++)
                {
                    if(i % 2 == 0)
                        memory.Add(i / 2);
                    else
                        memory.Add(-1);
                }
            }

            int idxStart = 0;
            int idxEnd = memory.Count - 1;
            while(idxStart <= idxEnd)
            {
                for(; idxStart < idxEnd; idxStart++)
                {
                    if(memory[idxStart] == -1)
                    {
                        break;
                    }
                }

                for(; idxEnd > idxStart; idxEnd--)
                {
                    if(memory[idxEnd] != -1)
                    {
                        break;
                    }
                }

                if(idxStart == idxEnd)
                    break;

                memory[idxStart] = memory[idxEnd];
                memory[idxEnd] = -1;
                idxStart++;
                idxEnd--;
            }
            memory.RemoveAll(x => x == -1);

            ulong solutionPart1 = 0;
            for(int i = 0; i < memory.Count; i++)
            {
                ulong fileId = (ulong)memory[i];
                solutionPart1 += (ulong)i * fileId;
            }

            /* Part 1 */
            Console.WriteLine("Day 09 part 1, result: " + solutionPart1);

            Part2();
        }

        static void Part2()
        {
            LinkedList<Memory> memory = GenerateMemorySpan();
            LinkedListNode<Memory> current = memory.First;
            LinkedListNode<Memory> searchTail = memory.Last;
            LinkedListNode<Memory> searchHead = memory.First;
            while(!searchHead.Value.Used && searchHead != null)
                searchHead = searchHead.Next;

            while(searchHead != searchTail)
            {
                /* Find a file. */
                while(searchHead != searchTail)
                {
                    Memory tail = searchTail.Value;
                    if(tail.Used)
                        break;
                    searchTail = searchTail.Previous;
                }

                if(searchHead == searchTail)
                    break;

                Memory file = searchTail.Value;

                /* Search for open space. */
                LinkedListNode<Memory> searchNode = searchHead;
                while(searchNode != searchTail && (searchNode.Value.Used || (!searchNode.Value.Used && searchNode.Value.Size < file.Size)))
                    searchNode = searchNode.Next;
                    

                /* Error check if we actually found space. */
                if(searchNode == searchTail)
                {
                    searchTail = searchTail.Previous;
                    continue;
                }

                /* Move file */
                LinkedListNode<Memory> fileNode = searchTail;
                searchTail = searchTail.Previous;

                Memory empty = searchNode.Value;
                if(empty.Size == file.Size)
                {
                    /* Replace entire empty space with file. */

                    empty.Used = true;
                    empty.Id = file.Id;
                    empty.Size = file.Size;

                    file.Id = -1;
                    file.Used = false;

                    /* Find next open space. */
                    while(!searchHead.Value.Used && searchHead != searchTail)
                        searchHead = searchHead.Next;
                }
                else
                {
                    /* Create a new file node before the empty space and shrink the empty space to fit the file in memory.
                     * Lastly mark the old memory space as empty. */

                    Memory newFile = new Memory(true, file.Id, empty.Position, file.Size);
                    memory.AddBefore(searchNode, newFile);

                    empty.Position += file.Size;
                    empty.Size -= file.Size;

                    file.Id = -1;
                    file.Used = false;
                }
            }

            Console.WriteLine("Day 09 part 2, result: " + FileSystemChecksum(memory));
        }

        static LinkedList<Memory> GenerateMemorySpan()
        {
            string input = File.ReadAllText("input.txt");

            LinkedList<Memory> memory = new LinkedList<Memory>();
            int position = 0;
            for(int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                int size = (int)Char.GetNumericValue(c);
                int id;
                bool inUse;

                if(i % 2 == 0)
                {
                    id = i / 2;
                    inUse = true;
                }
                else
                {
                    id = -1;
                    inUse = false;
                }

                Memory m = new Memory(inUse, id, position, size);
                memory.AddLast(m);

                position += size;
            }

            return memory;
        }

        static ulong FileSystemChecksum(LinkedList<Memory> memory)
        {
            ulong result = 0;
            for(LinkedListNode<Memory> current = memory.First; current != null; current = current.Next)
            {
                Memory m = current.Value;
                if(!m.Used)
                    continue;

                for(int i = m.Position; i < m.Position + m.Size; i++)
                    result += (ulong)(i * m.Id);
            }
            return result;
        }
    }


    public class Memory
    {
        public bool Used { get; set; }
        public int Id { get; set; }
        public int Position { get; set; }
        public int Size { get; set; }

        public Memory(bool inUse, int id, int position, int size)
        {
            Used = inUse;
            Id = id;
            Position = position;
            Size = size;
        }
    }
}
