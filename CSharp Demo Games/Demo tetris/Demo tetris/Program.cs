using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace DemoTetris
{
    public static class Program
    {
        // Settings
        static int TetrisRows = 20;
        static int TetrisCols = 10;
        static int InfoCols = 10;
        static int ConsoleRows = 1 + TetrisRows + 1;
        static int ConsoleCols = 1 + TetrisCols + 1 + InfoCols + 1;
        static List<Tetrominoe> TetrisFigures = new List<Tetrominoe>()
            {
                new Tetrominoe(new bool[,] // ----
                {
                    { true, true, true, true }
                }),
                new Tetrominoe(new bool[,] // O
                {
                    { true, true },
                    { true, true }
                }),
                new Tetrominoe(new bool[,] // T
                {
                    { false, true, false },
                    { true, true, true },
                }),
                new Tetrominoe(new bool[,] // S
                {
                    { false, true, true, },
                    { true, true, false, },
                }),
                new Tetrominoe(new bool[,] // Z
                {
                    { true, true, false },
                    { false, true, true },
                }),
                new Tetrominoe(new bool[,] // J
                {
                    { true, false, false },
                    { true, true, true }
                }),
                new Tetrominoe(new bool[,] // L
                {
                    { false, false, true },
                    { true, true, true }
                }),
            };
        static int[] ScorePerLines = { 0, 40, 100, 300, 1200 };

        // State
        static TetrisGameState State = new TetrisGameState(TetrisRows, TetrisCols);
        static ScoreManager ScoreManager = new ScoreManager("scores.txt");
        static Random Random = new Random();

        public static void Main()
        {
            var musicPlayer = new MusicPlayer();
            musicPlayer.Play();

            var tetrisConsoleWriter = new TetrisConsoleWriter();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Title = "Tetris v1.0";
            Console.CursorVisible = false;
            Console.WindowHeight = ConsoleRows + 1;
            Console.WindowWidth = ConsoleCols;
            Console.BufferHeight = ConsoleRows + 1;
            Console.BufferWidth = ConsoleCols;
            State.CurrentFigure = TetrisFigures[Random.Next(0, TetrisFigures.Count)];

            while (true)
            {
                State.Frame++;
                State.UpdateLevel();
                // Read user input
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        // Environment.Exit(0);
                        return; // Becase of Main()
                    }
                    if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                    {
                        if (State.CurrentFigureCol >= 1)
                        {
                            State.CurrentFigureCol--;
                        }
                    }
                    if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                    {
                        if (State.CurrentFigureCol < TetrisCols - State.CurrentFigure.Height)
                        {
                            State.CurrentFigureCol++;
                        }
                    }
                    if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        State.Frame = 1;
                        State.Score += State.Level;
                        State.CurrentFigureRow++;
                    }
                    if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                    {
                        var newFigure =  State.CurrentFigure.GetRotate();
                        if (!Collision(State.CurrentFigure))
                        {
                            State.CurrentFigure = newFigure;
                        }
                    }
                }

                // Update the game state
                if (State.Frame % (State.FramesToMoveFigure - State.Level) == 0)
                {
                    State.CurrentFigureRow++;
                    State.Frame = 0;
                }

                if (Collision(State.CurrentFigure))
                {
                    AddCurrentFigureToTetrisField();
                    int lines = CheckForFullLines();
                    State.Score += ScorePerLines[lines] * State.Level;
                    ScoreManager.UpdateHightScore(State.Score);
                    State.CurrentFigure = TetrisFigures[Random.Next(0, TetrisFigures.Count)];
                    State.CurrentFigureRow = 0;
                    State.CurrentFigureCol = 0;
                    if (Collision(State.CurrentFigure)) // game is over
                    {
                        ScoreManager.Add(State.Score);
                        tetrisConsoleWriter.WriteGameOver(State.Score, TetrisRows / 2 - 3, TetrisCols + 3 + InfoCols / 2 - 6);
                        Thread.Sleep(100000);
                        return;
                    }
                }

                // Redraw UI
                tetrisConsoleWriter.DrawBorder(TetrisCols, TetrisRows, InfoCols);
                tetrisConsoleWriter.DrawGameState(3 + TetrisCols, State, ScoreManager.HighScore);
                tetrisConsoleWriter.DrawTetrisField(State.TetrisField);
                tetrisConsoleWriter.DrawCurrentFigure(State.CurrentFigure, State.CurrentFigureRow, State.CurrentFigureCol);

                Thread.Sleep(40);
            }
        }

        

        private static int CheckForFullLines() // 0, 1, 2, 3, 4
        {
            int lines = 0;

            for (int row = 0; row < State.TetrisField.GetLength(0); row++)
            {
                bool rowIsFull = true;
                for (int col = 0; col < State.TetrisField.GetLength(1); col++)
                {
                    if (State.TetrisField[row, col] == false)
                    {
                        rowIsFull = false;
                        break;
                    }
                }

                if (rowIsFull)
                {
                    for (int rowToMove = row; rowToMove >= 1; rowToMove--)
                    {
                        for (int col = 0; col < State.TetrisField.GetLength(1); col++)
                        {
                            State.TetrisField[rowToMove, col] = State.TetrisField[rowToMove - 1, col];
                        }
                    }

                    lines++;
                }
            }

            return lines;
        }

        static void AddCurrentFigureToTetrisField()
        {
            for (int row = 0; row < State.CurrentFigure.Width; row++)
            {
                for (int col = 0; col < State.CurrentFigure.Height; col++)
                {
                    if (State.CurrentFigure.Body[row, col])
                    {
                        State.TetrisField[State.CurrentFigureRow + row, State.CurrentFigureCol + col] = true;
                    }
                }
            }
        }

        static bool Collision(Tetrominoe figure)
        {
            if (State.CurrentFigureCol > TetrisCols - figure.Height)
            {
                return true;
            }

            if (State.CurrentFigureRow + figure.Width == TetrisRows)
            {
                return true;
            }

            for (int row = 0; row < figure.Width; row++)
            {
                for (int col = 0; col < figure.Height; col++)
                {
                    if (figure.Body[row, col] &&
                        State.TetrisField[State.CurrentFigureRow + row + 1, State.CurrentFigureCol + col])
                    {
                        return true;
                    }

                }
            }

            return false;
        }



    }
}