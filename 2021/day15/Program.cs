using System;
using System.IO;
using System.Collections.Generic;

namespace day15
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("input.txt");
            int[,] grid = new int[data.Length, data[0].Length];

            for(int i = 0; i < data.Length; i++)
            {
                string line = data[i];
                for(int j = 0; j < line.Length; j++)
                {
                    grid[i,j] = int.Parse(line[j].ToString());
                }
            }

            int gridWidth = grid.GetLength(1);
            int gridHeight = grid.GetLength(0);

            Vec2 start = new Vec2(0, 0);
            Vec2 end = new Vec2(gridWidth - 1, gridHeight - 1);

            int solutionPart1 = dijkstra(grid, start, end);
            Console.WriteLine("Day 15 part 1, result: " + solutionPart1);

            /* Create the larger grid. */
            int[,] grid2 = new int[5 * gridHeight, 5 * gridWidth];
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    for(int k = 0; k < gridHeight; k++)
                    {
                        for(int l = 0; l < gridWidth; l++)
                        {
                            int cost = (grid[k, l] + i + j);
                            if(cost > 9)
                                cost -= 9;
                            grid2[i*gridHeight+k, j*gridWidth+l] = cost;
                        }
                    }
                }
            }

            Vec2 endPart2 = new Vec2(grid2.GetLength(1) - 1, grid2.GetLength(0) - 1);
            int solutionPart2 = dijkstra(grid2, start, endPart2);
            Console.WriteLine("Day 15 part 2, result: " + solutionPart2);
        }

        static int dijkstra(int[,] grid, Vec2 source, Vec2 destination)
        {
            int gridWidth = grid.GetLength(1);
            int gridHeight = grid.GetLength(0);
            MyPriorityQueue queue = new MyPriorityQueue();
            Dictionary<Vec2, int> dist = new Dictionary<Vec2, int>();
            dist[source] = 0;
            for(int y = 0; y < gridHeight; y++)
            {
                for(int x = 0; x < gridWidth; x++)
                {
                    Vec2 vertex = new Vec2(x, y);
                    if(vertex != source)
                    {
                        dist[vertex] = Int32.MaxValue;
                    }
                    queue.enqueue(vertex, dist[vertex]);
                }
            }

            Vec2 rotation = new Vec2(0, -1);
            int tmp;
            while(!queue.isEmpty())
            {
                Vec2 vertex = queue.dequeue();

                for(int i = 0; i < 4; i++)
                {
                    /* Calculate the neighbour position. */
                    Vec2 neighbour = new Vec2(vertex);
                    tmp = rotation.x;
                    rotation.x = -rotation.y;
                    rotation.y = tmp;
                    neighbour.add(rotation);

                    /* Check if neighbour is on the grid. */
                    if(neighbour.x < 0 || neighbour.x >= gridWidth || neighbour.y < 0 || neighbour.y >= gridHeight)
                        continue;

                    if(dist[vertex] == Int32.MaxValue)
                        continue;

                    /* Distance calculation. */
                    int distanceToNeighbour = dist[vertex] + grid[neighbour.y, neighbour.x];
                    if(distanceToNeighbour < dist[neighbour])
                    {
                        dist[neighbour] = distanceToNeighbour;
                        queue.updatePriority(neighbour, distanceToNeighbour);
                    }
                }
            }
            return dist[destination];
        }
    }

    class Vec2 : IEquatable<Vec2>
    {
        public int x { get; set; }
        public int y { get; set; }
        public Vec2(int x, int y) { this.x = x; this.y = y; }
        public Vec2(Vec2 v) { this.x = v.x; this.y = v.y; }
        public void add(Vec2 v) { this.x += v.x; this.y += v.y; }

        public bool Equals(Vec2 other)
        {
            if(other == null)
                return false;

            return (this.x == other.x && this.y == other.y) ? true : false;
        }

        public override bool Equals(Object obj)
        {
            if(obj == null)
                return false;

            Vec2 vec2Obj = obj as Vec2;
            if(vec2Obj == null)
                return false;
            else
                return Equals(vec2Obj);
        }

        public override int GetHashCode()
        {
            return (x << 16) | y;
        }

        public static bool operator == (Vec2 v1, Vec2 v2)
        {
            if(((object)v1) == null || ((object)v2) == null)
                return Object.Equals(v1, v2);

            return v1.Equals(v2);
        }

        public static bool operator != (Vec2 v1, Vec2 v2)
        {
            if(((object)v1) == null || ((object)v2) == null)
                return ! Object.Equals(v1, v2);

            return !(v1.Equals(v2));
        }
    }

    class MyPriorityQueue
    {

        public List<(Vec2, int)> minheap;
        public int count { get; set; }

        public MyPriorityQueue()
        {
            this.minheap = new List<(Vec2, int)>();
        }

        public void enqueue(Vec2 node, int priority)
        {
            this.minheap.Add((node, priority));
            sortDownwards(this.minheap.Count - 1);
        }

        public Vec2 dequeue()
        {
            if(this.minheap.Count == 0)
                return null;

            (Vec2 node, int priority) n = this.minheap[0];
            this.minheap[0] = this.minheap[this.minheap.Count - 1];
            this.minheap.RemoveAt(this.minheap.Count - 1);
            sortUpwards();
            return n.node;
        }

        public void updatePriority(Vec2 node, int newPriority)
        {
            /* Breath first search. */
            int index = 0;
            for(int i = 0; i < this.minheap.Count; i++)
            {
                (Vec2 node, int priority) element = this.minheap[i];
                if(element.node == node)
                {
                    index = i;
                    break;
                }
            }
            this.minheap[index] = (node, newPriority);
            this.sortDownwards(index);
        }

        public Vec2 peek()
        {
            if(this.minheap.Count == 0)
                return null;
            else
                return this.minheap[0].Item1;
        }

        public bool isEmpty()
        {
            if(this.minheap.Count == 0)
                return true;
            else
                return false;
        }

        private void sortDownwards(int startingIdx)
        {
            (Vec2 node, int priority) n = this.minheap[startingIdx];
            int priority = n.priority;

            while(startingIdx != 0 && priority < this.minheap[(startingIdx - 1) >> 1].Item2)
            {
                var tmp = this.minheap[startingIdx];
                this.minheap[startingIdx] = this.minheap[(startingIdx - 1) >> 1];
                this.minheap[(startingIdx - 1) >> 1] = tmp;
                startingIdx = (startingIdx - 1) >> 1;
            }
        }

        private void sortUpwards()
        {
            int i = 0;
            int queueSize = this.minheap.Count;
            int minChildPriority;
            (Vec2 node, int priority) parent;
            (Vec2 node, int priority) leftChild;
            (Vec2 node, int priority) rightChild;
            while(2*i + 1 < queueSize)
            {
                parent = this.minheap[i];
                leftChild = this.minheap[2*i + 1];
                rightChild = (2*i + 2 < queueSize) ? this.minheap[2*i + 2] : (null, Int32.MaxValue);

                minChildPriority = Math.Min(leftChild.priority, rightChild.priority);
                if(parent.priority <= minChildPriority)
                    break;

                if(minChildPriority == leftChild.priority)
                {
                    this.minheap[i] = leftChild;
                    this.minheap[2*i + 1] = parent;
                    i = 2*i + 1;
                }
                else
                {
                    this.minheap[i] = rightChild;
                    this.minheap[2*i + 2] = parent;
                    i = 2*i + 2;
                }
            }
        }
    }
}
