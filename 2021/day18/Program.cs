using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day18
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            int snailfishNumberCount = lines.Length;

            /* Part 1 */
            Tree t = new Tree(lines[0]);
            for(int i = 1; i < lines.Length; i++)
                t.add(new Tree(lines[i]));
            int solutionPart1 = t.magnitude();
            Console.WriteLine("Day 18 part 1, result: " + solutionPart1);

            /* Part 2 */
            int solutionPart2 = 0;
            int magnitude = 0;
            for(int i = 0; i < snailfishNumberCount - 1; i++)
            {
                for(int j = i+1; j < snailfishNumberCount; j++)
                {
                    Tree t1 = new Tree(lines[i]);
                    t1.add(new Tree(lines[j]));
                    magnitude = t1.magnitude();
                    if(magnitude > solutionPart2)
                        solutionPart2 = magnitude;

                    Tree t2 = new Tree(lines[j]);
                    t2.add(new Tree(lines[i]));
                    magnitude = t2.magnitude();
                    if(magnitude > solutionPart2)
                        solutionPart2 = magnitude;
                }
            }
            Console.WriteLine("Day 18 part 2, result: " + solutionPart2);
        }
    }

    class Tree
    {
        public Node root { get; set; }

        public Tree(string text)
        {
            root = new Node(text);
        }

        public void add(Tree other)
        {
            Node newRoot = new Node(null, this.root, other.root);
            root.parent = newRoot;
            other.root.parent = newRoot;
            root = newRoot;
            reduce();
        }

        public void reduce()
        {
            int reduceActionsCompleted = 0;
            while(reduceActionsCompleted != 2)
            {
                reduceActionsCompleted = 0;

                if(explode())
                    continue;
                else
                    reduceActionsCompleted++;

                if(split())
                    continue;
                else
                    reduceActionsCompleted++;
            }
        }

        private bool explode()
        {
            Stack<(Node, int)> backtrace = new Stack<(Node, int)>();
            backtrace.Push((this.root, 0));
            Node lastSeenValueNode = null;

            do
            {
                (Node node, int depth) elem = backtrace.Pop();
                Node node = elem.node;

                if(node.value == -1)
                {
                    if(elem.depth >= 4 && node.left.value != -1 && node.right.value != -1)
                    {
                        Node nextValueNode = (backtrace.Count > 0) ? backtrace.Peek().Item1 : null;
                        explodePair(node, lastSeenValueNode, nextValueNode);
                        return true;
                    }

                    /* Traverse to the children. */
                    backtrace.Push((node.right, elem.depth+1));
                    backtrace.Push((node.left, elem.depth+1));
                    continue;
                }
                else
                    lastSeenValueNode = node;

            } while(backtrace.Count > 0);
            return false;
        }

        private void explodePair(Node n, Node left, Node right)
        {
            if(left != null)
                left.value += n.left.value;

            if(right != null)
            {
                while(right.value == -1)
                    right = right.left;
                right.value += n.right.value;
            }

            /* Replace exploded node with 0. */
            n.left = null;
            n.right = null;
            n.value = 0;
        }

        private bool split()
        {
            Stack<Node> backtrace = new Stack<Node>();
            backtrace.Push(this.root);

            do
            {
                Node node = backtrace.Pop();

                if(node.value != -1)
                {
                    if(node.value > 9)
                    {
                        splitNumber(node);
                        return true;
                    }
                }
                else
                {
                    /* Traverse to the children. */
                    backtrace.Push(node.right);
                    backtrace.Push(node.left);
                }
            } while(backtrace.Count > 0);
            return false;
        }

        private void splitNumber(Node n)
        {
            Node leftChild = new Node((int)Math.Floor((double)n.value / 2.0));
            leftChild.parent = n;
            Node rightChild = new Node((int)Math.Ceiling((double)n.value / 2.0));
            rightChild.parent = n;

            n.left = leftChild;
            n.right = rightChild;
            n.value = -1;
        }

        public int magnitude()
        {
            return this.root.magnitude();
        }
    }

    class Node
    {
        public Node left { get; set; }
        public Node right { get; set; }
        public Node parent { get; set; }
        public int value { get; set; }
        static Regex regex = new Regex(@"^\d+$");

        public Node(int v)
        {
            this.value = v;
            this.parent = null;
            this.left = null;
            this.right = null;
        }

        public Node(string text)
        {
            if(regex.IsMatch(text))
            {
                this.value = Int32.Parse(text);
                return;
            }

            this.value = -1;
            Stack<char> stack = new Stack<char>();
            string leftPair = "";
            string rightPair = "";
            for(int i = 0; i < text.Length; i++)
            {
                switch(text[i])
                {
                    case '[':
                        stack.Push('[');
                        break;
                    case ']':
                        stack.Pop();
                        break;
                    case ',':
                        if(stack.Count == 1)
                        {
                            leftPair = text.Substring(1, i - 1);
                            rightPair = text.Substring(i+1, text.Length - i - 2);
                        }
                        break;
                    default:
                        continue;
                }
            }

            this.left = new Node(leftPair);
            this.right = new Node(rightPair);
            this.left.parent = this;
            this.right.parent = this;
        }

        public Node(Node parent, Node left, Node right)
        {
            this.parent = parent;
            this.left = left;
            this.right = right;
            this.value = -1;
        }

        public int magnitude()
        {
            if(this.value != -1)
                return this.value;
            else
                return 3 * this.left.magnitude() + 2 * this.right.magnitude();
        }
    }
}
