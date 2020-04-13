using System;
using System.Collections.Generic;
using System.Threading;

namespace DemoTetris
{
    // TODO: When we have colision during moving (left / right)we have unusual behaviour
    public static class Program
    {
        // Settings
        static int TetrisRows = 20;
        static int TetrisCols = 10;
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
        
        
        public static void Main()
        {
            ScoreManager scoreManager = new ScoreManager("scores.txt");
            var musicPlayer = new MusicPlayer();
            musicPlayer.Play();

            var tetrisConsoleWriter = new TetrisConsoleWriter(TetrisRows, TetrisCols);
            var random = new Random();

            var game = new TetrisGame(TetrisRows, TetrisCols);
            game.CurrentFigure = TetrisFigures[random.Next(0, TetrisFigures.Count)];
            while (true)
            {
                game.Frame++;
                game.UpdateLevel(scoreManager.Score);
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
                        if (game.CurrentFigureCol >= 1)
                        {
                            game.CurrentFigureCol--;
                        }
                    }
                    if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                    {
                        if (game.CurrentFigureCol < TetrisCols - game.CurrentFigure.Height)
                        {
                            game.CurrentFigureCol++;
                        }
                    }
                    if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        game.Frame = 1;
                        scoreManager.AddToScore(game.Level);
                        game.CurrentFigureRow++;
                    }
                    if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                    {
                        var newFigure = game.CurrentFigure.GetRotate();
                        if (!game.Collision(game.CurrentFigure))
                        {
                            game.CurrentFigure = newFigure;
                        }
                    }
                }

                // Update the game state
                if (game.Frame % (game.FramesToMoveFigure - game.Level) == 0)
                {
                    game.CurrentFigureRow++;
                    game.Frame = 0;
                }

                if (game.Collision(game.CurrentFigure))
                {
                    game.AddCurrentFigureToTetrisField();
                    int lines = game.CheckForFullLines();
                    scoreManager.AddToScore(ScorePerLines[lines] * game.Level);
                    game.CurrentFigure = TetrisFigures[random.Next(0, TetrisFigures.Count)];
                    game.CurrentFigureRow = 0;
                    game.CurrentFigureCol = 0;
                    if (game.Collision(game.CurrentFigure)) // game is over
                    {
                        scoreManager.AddToHighScore();
                        tetrisConsoleWriter.DrawAll(game, scoreManager);
                        tetrisConsoleWriter.WriteGameOver(scoreManager.Score);
                        Thread.Sleep(100000);
                        return;
                    }
                }

                // Redraw UI
                tetrisConsoleWriter.DrawAll(game, scoreManager);
                Thread.Sleep(40);
            }
        }

        
    }
}