using System;

namespace DemoTetris
{
    public class ConsoleInputHandler : IInputHandler
    {
        public TetrisGameInput GetInput()
        {
            // Read user input
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                {
                    return TetrisGameInput.Exit;
                }
                if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                {
                    return TetrisGameInput.Left;
                }
                if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                {
                    return TetrisGameInput.Rigth;
                }
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                {
                    //tetrisConsoleWriter.Frame = 1;
                    //scoreManager.AddToScore(game.Level, 0);
                    //game.CurrentFigureRow++;

                    return TetrisGameInput.Down;
                }
                if (key.Key == ConsoleKey.Spacebar || key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                {
                    //var newFigure = game.CurrentFigure.GetRotate();
                    //if (!game.Collision(game.CurrentFigure))
                    //{
                    //    game.CurrentFigure = newFigure;
                    //}

                    return TetrisGameInput.Rotate;
                }
            }

            return TetrisGameInput.None;
        }
    }

    public enum TetrisGameInput
    {
        None = 0,
        Left = 1,
        Rigth = 2,
        Down = 3,
        Rotate = 4,
        Exit = 99,
    }
}
