using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Isola
{
    public class IsolaClass
    {
        public static void Main()
        {
            Console.Write("Gameboard size: ");
            int boardsize = int.Parse(Console.ReadLine());
            Console.Write("Who is first? ( [1]CPU or [2]You ):  ");
            int cpuTurnInput = int.Parse(Console.ReadLine());
            bool cpuFirst = false;
            if (cpuTurnInput == 1)
            {
                cpuFirst = true;
            }
            Console.Write("You start on row: ");
            int playerStartPositionX = int.Parse(Console.ReadLine());
            Console.Write("You start on column: ");
            int playerStartPositionY = int.Parse(Console.ReadLine());
            Console.Write("CPU starts on row: ");
            int cpuStartPositionX = int.Parse(Console.ReadLine());
            Console.Write("CPU starts on column: ");
            int cpuStartPositionY = int.Parse(Console.ReadLine());
            int[] user = new int[2] { playerStartPositionX, playerStartPositionY };
            int[] cpu = new int[2] { cpuStartPositionX, cpuStartPositionY };
            string[,] board = new string[boardsize, boardsize];
            string emptyCell = "-";
            string removedCell = "*";
            string userCell = "U";
            string cpuCell = "C";
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    board[row, col] = emptyCell;
                }
            }
            board[user[0], user[1]] = userCell;
            board[cpu[0], cpu[1]] = cpuCell;
            PrintBoard(board);
            bool gameOver = false;
            Stopwatch stopwatch = new Stopwatch();
            //Main progress starts here
            stopwatch.Start();
            if (cpuFirst)
            {
                while (!gameOver)
                {
                    Console.WriteLine("CPU Move:");
                    CPUTurn(board, cpu, user, emptyCell, removedCell);
                    gameOver = GameOverCheck(board, user, emptyCell);
                    if (gameOver)
                    {
                        Console.WriteLine();
                        Console.WriteLine("CPU Wins!");
                        break;
                    }
                    UserTurn(board, user, emptyCell, removedCell);
                    gameOver = GameOverCheck(board, cpu, emptyCell);
                    if (gameOver)
                    {
                        Console.WriteLine();
                        Console.WriteLine("You Win!");
                        break;
                    }
                }
            }
            else
            {
                while (!gameOver)
                {
                    UserTurn(board, user, emptyCell, removedCell);
                    gameOver = GameOverCheck(board, cpu, emptyCell);
                    if (gameOver)
                    {
                        Console.WriteLine();
                        Console.WriteLine("You Win!");
                        break;
                    }
                    Console.WriteLine("CPU Move:");
                    CPUTurn(board, cpu, user, emptyCell, removedCell);
                    gameOver = GameOverCheck(board, user, emptyCell);
                    if (gameOver)
                    {
                        Console.WriteLine();
                        Console.WriteLine("CPU Wins!");
                        break;
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine("Time played: " + stopwatch.Elapsed);
            Console.WriteLine();
            Console.Write("Press E to exit: ");
            string exit = Console.ReadLine();
            if(exit == "E" || exit == "e")
            {
                Console.WriteLine();
            }
        }

        public static void UserTurn(string[,] board, int[] player, string emptyCell, string removedCell)
        {
            bool succesfulMove = true;
            bool succesfulRemove = true;
            do
            {
                succesfulMove = true;
                try
                {
                    Console.Write("Move to row: ");
                    int userNewCellPositionX = int.Parse(Console.ReadLine());
                    Console.Write("Move to column: ");
                    int userNewCellPositionY = int.Parse(Console.ReadLine());
                    MoveToCell(board, player, userNewCellPositionX, userNewCellPositionY, emptyCell);
                }
                catch (Exception) 
                {
                    succesfulMove = false;
                    Console.WriteLine("Try Again");
                }
            } while (!succesfulMove);

            PrintBoard(board);
            do
            {
                succesfulRemove = true;
                try
                {
                    Console.Write("Remove cell on row: ");
                    int userRemoveCellX = int.Parse(Console.ReadLine());
                    Console.Write("Remove cell on column: ");
                    int userRemoveCellY = int.Parse(Console.ReadLine());
                    RemoveCell(board, userRemoveCellX, userRemoveCellY, emptyCell, removedCell);
                }
                catch (Exception)
                {
                    succesfulRemove = false;
                    Console.WriteLine("Try Again");
                }
            } while (!succesfulRemove);
            PrintBoard(board);
        }

        public static void CPUTurn(string[,] board, int[] player, int[] enemy, string emptyCell, string removedCell)
        {
            int newPositionCPUX = 0;
            int newPositionCPUY = 0;
            try
            {
                newPositionCPUX = AI(board, player, emptyCell)[0];
                newPositionCPUY = AI(board, player, emptyCell)[1];
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
            MoveToCell(board, player, newPositionCPUX, newPositionCPUY , emptyCell);

            int removeCellCPUX = 0;
            int removeCellCPUY = 0;
            try
            {
                removeCellCPUX = AI(board, enemy, emptyCell)[0];
                removeCellCPUY = AI(board, enemy, emptyCell)[1];
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
            RemoveCell(board, removeCellCPUX, removeCellCPUY, emptyCell, removedCell);
            PrintBoard(board);
        }

        public static int[] AI(string[,] board, int[] target, string emptyCell)
        {
            int counter = 0;
            int memoryCounter = counter;
            int range = 1;
            Random randomGenerator = new Random();
            int[] newCell = new int[2];
            List<int[]> availableCells = new List<int[]>();
            for (int row = target[0] - range; row <= target[0] + range; row++)
            {
                if (row < 0)
                {
                    continue;
                }
                if (row >= board.GetLength(0))
                {
                    break;
                }
                for (int col = target[1] - range; col <= target[1] + range; col++)
                {
                    if (col < 0)
                    {
                        continue;
                    }
                    if (col >= board.GetLength(1))
                    {
                        break;
                    }
                    if (board[row, col] == emptyCell)
                    {
                        newCell[0] = row;
                        newCell[1] = col;
                        for (int insideRow = newCell[0] - range; insideRow <= newCell[0] + range; insideRow++)
                        {
                            if (insideRow < 0)
                            {
                                continue;
                            }
                            if (insideRow >= board.GetLength(0))
                            {
                                break;
                            }
                            for (int insideCol = newCell[1] - range; insideCol <= newCell[1] + range; insideCol++)
                            {
                                if (insideCol < 0)
                                {
                                    continue;
                                }
                                if (insideCol >= board.GetLength(1))
                                {
                                    break;
                                }
                                if (board[insideRow, insideCol] == emptyCell)
                                {
                                    counter++;
                                }

                            }
                        }
                        if (counter > memoryCounter)
                        {
                            availableCells.Clear();
                            memoryCounter = counter;
                            availableCells.Add(new int[2] { row, col });
                        }
                        else if (counter == memoryCounter)
                        {
                            availableCells.Add(new int[2] { row, col });
                        }
                        counter = 0;

                    }
                }
            }
            if (availableCells.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            int random = randomGenerator.Next(0, availableCells.Count - 1);
            int[] returnArray = new int[2] { availableCells[random][0], availableCells[random][1] };
            return returnArray;
        }

        public static void MoveToCell(string[,] board, int[] player, int newPositionX, int newPositionY, string emptyCell)
        {
            bool legitMove = false;
            string currentPlayerSymbol = board[player[0], player[1]];
            for (int row = player[0] - 1; row <= player[0] + 1; row++)
            {
                if (row == newPositionX)
                {
                    for (int col = player[1] - 1; col <= player[1] + 1; col++)
                    {
                        if (col == newPositionY)
                        {
                            if (board[row, col] == emptyCell)
                            {
                                legitMove = true;
                            }
                        }
                    }
                }
            }

            if (legitMove)
            {
                board[newPositionX, newPositionY] = currentPlayerSymbol;
                board[player[0], player[1]] = emptyCell;
                player[0] = newPositionX;
                player[1] = newPositionY;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static void RemoveCell(string[,] board, int removeCellPositionX, int removeCellPositionY, string emptyCell, string removedCell)
        {
            if( removeCellPositionX < 0 || removeCellPositionX >= board.GetLength(0))
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (removeCellPositionY < 0 || removeCellPositionY >= board.GetLength(1))
            {
                throw new ArgumentOutOfRangeException();
            }
            if(board[removeCellPositionX, removeCellPositionY] != emptyCell)
            {
                throw new ArgumentOutOfRangeException();
            }
            board[removeCellPositionX, removeCellPositionY] = removedCell;
        }

        public static bool GameOverCheck(string[,] board, int[] player, string emptyCell)
        {
            bool gameOver = true;
            for (int row = player[0] - 1; row <= player[0] + 1; row++)
            {
                if(row < 0)
                {
                    continue;
                }
                if(row >= board.GetLength(0))
                {
                    break;
                }
                for (int col = player[1] - 1; col <= player[1] + 1; col++)
                {
                    if(col < 0)
                    {
                        continue;
                    }
                    if(col >= board.GetLength(1))
                    {
                        break;
                    }
                    if(board[row, col] == emptyCell)
                    {
                        gameOver = false;
                        break;
                    }
                }
            }
            return gameOver;
        }

        public static void PrintBoard(string[,] board)
        {
            Console.WriteLine();
            Console.Write("   ");
            for (int col = 0; col < board.GetLength(1); col++)
            {
                Console.Write(col + "  ");
            }
            Console.WriteLine();
            for (int row = 0; row < board.GetLength(0); row++)
            {
                Console.Write(row + "  ");
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    Console.Write(board[row, col] + "  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
