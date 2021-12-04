using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace day04
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("input.txt");

            List<int> bingoNumbers = new List<int>();
            foreach(string x in data[0].Split(','))
                bingoNumbers.Add(Int32.Parse(x));

            string[] rawBoard = new string[5];
            List<BingoBoard> boards = new List<BingoBoard>();
            for(int i = 2; i < data.Length; i += 6)
            {
                Array.Copy(data, i, rawBoard, 0, 5);
                boards.Add(new BingoBoard(rawBoard));
            }

            int latestWinner = -1;
            int solutionPart1 = -1;
            foreach(int num in bingoNumbers)
            {
                foreach(BingoBoard board in boards)
                {
                    if(board.addNumber(num))
                    {
                        latestWinner = num * board.sumUnmarked();
                        if(solutionPart1 == -1)
                            solutionPart1 = latestWinner;
                    }
                }
            }

            Console.WriteLine("Day 4 part 1, result: " + solutionPart1);
            Console.WriteLine("Day 4 part 2, result: " + latestWinner);
        }
    }


    class BingoBoard
    {
        public bool bingo;
        private BingoCell[,] board;
        private Dictionary<int, (int, int)> boardLookup;

        public BingoBoard(string[] rawBoard)
        {
            this.bingo = false;
            this.board = new BingoCell[5, 5];
            this.boardLookup = new Dictionary<int, (int, int)>();
            parseRawBoard(rawBoard);
        }

        public bool addNumber(int newNumber)
        {
            /* Don't add a new number if this board already got bingo or does not have the number. */
            if(this.bingo || !this.boardLookup.ContainsKey(newNumber))
                return false;

            (int row, int col) position = this.boardLookup[newNumber];
            this.board[position.row, position.col].marked = true;

            if(checkRow(position.row) || checkCol(position.col))
            {
                this.bingo = true;
                return true;
            }
            return false;
        }

        public int sumUnmarked()
        {
            int sum = 0;
            for(int i = 0; i < 5; i++)
                for(int j = 0; j < 5; j++)
                    sum += (this.board[i, j].marked) ? 0 : this.board[i, j].value;
            return sum;
        }

        private bool checkRow(int row)
        {
            for(int i = 0; i < 5; i++)
                if(!this.board[row, i].marked)
                    return false;
            return true;
        }

        private bool checkCol(int col)
        {
            for(int i = 0; i < 5; i++)
                if(!this.board[i, col].marked)
                    return false;
            return true;
        }

        private void parseRawBoard(string[] rawBoard)
        {
            string pattern = @"\s*(\d+)\s+(\d+)\s+(\d+)\s+(\d+)\s+(\d+)";
            Regex regex = new Regex(pattern);

            for(int i = 0; i < 5; i++)
            {
                Match match  = regex.Match(rawBoard[i]);
                for(int j = 0; j < 5; j++)
                {
                    Group group = match.Groups[j+1];
                    int value = Int32.Parse(group.Value);
                    BingoCell c = new BingoCell(value);
                    this.board[i, j] = c;
                    this.boardLookup[value] = (i, j);
                }
            }
        }
    }


    class BingoCell
    {
        public int value { get; }
        public bool marked { get; set; }

        public BingoCell(int cellValue)
        {
            this.value = cellValue;
            this.marked = false;
        }
    }
}
