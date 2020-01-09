using System;

namespace DemoTetris
{
    public class TetrisConsoleWriter
    {
        public void DrawGameState(int startColumn, TetrisGameState state, int highScore)
        {
            this.Write("Level:", 1, startColumn);
            this.Write(state.Level.ToString(), 2, startColumn);
            this.Write("Score:", 4, startColumn);
            this.Write(state.Score.ToString(), 5, startColumn);
            this.Write("Best:", 7, startColumn);
            this.Write(highScore.ToString(), 8, startColumn);
            this.Write("Frame:", 10, startColumn);
            this.Write(state.Frame.ToString() + " / " + (state.FramesToMoveFigure - state.Level).ToString(), 11, startColumn);
            this.Write("Position:", 13, startColumn);
            this.Write($"{state.CurrentFigureRow}, {state.CurrentFigureCol}", 14, startColumn);
            this.Write("Keys:", 16, startColumn);
            this.Write($"  ^ ", 18, startColumn);
            this.Write($"<   > ", 19, startColumn);
            this.Write($"  v  ", 20, startColumn);
        }

        public void DrawBorder(int tetrisColumns, int tetrisRows, int infoColumns)
        {
            Console.SetCursorPosition(0, 0);
            string line = "╔";
            line += new string('═', tetrisColumns);
            /* for (int i = 0; i < TetrisCols; i++)
            {
                line += "═";
            } */

            line += "╦";
            line += new string('═', infoColumns);
            line += "╗";
            Console.Write(line);

            for (int i = 0; i < tetrisRows; i++)
            {
                string middleLine = "║";
                middleLine += new string(' ', tetrisColumns);
                middleLine += "║";
                middleLine += new string(' ', infoColumns);
                middleLine += "║";
                Console.Write(middleLine);
            }

            string endLine = "╚";
            endLine += new string('═', tetrisColumns);
            endLine += "╩";
            endLine += new string('═', infoColumns);
            endLine += "╝";
            Console.Write(endLine);
        }

        public void WriteGameOver(int score, int row, int column)
        {
            var scoreAsString = score.ToString();
            scoreAsString = new string(' ', 7 - scoreAsString.Length) + scoreAsString;
            Write("╔═════════╗", row, column);
            Write("║ Game    ║", row + 1, column);
            Write("║   over! ║", row + 2, column);
            Write($"║ {scoreAsString} ║", row + 3, column);
            Write("╚═════════╝", row + 4, column);
        }

        public void DrawTetrisField(bool[,] tetrisField)
        {
            for (int row = 0; row < tetrisField.GetLength(0); row++)
            {
                string line = "";
                for (int col = 0; col < tetrisField.GetLength(1); col++)
                {
                    if (tetrisField[row, col])
                    {
                        line += "*";
                    }
                    else
                    {
                        line += " ";
                    }
                }

                this.Write(line, row + 1, 1);
            }
        }

        public void DrawCurrentFigure(Tetrominoe currentFigure, int currentFigureRow, int currentFigureColumn)
        {
            for (int row = 0; row < currentFigure.Width; row++)
            {
                for (int col = 0; col < currentFigure.Height; col++)
                {
                    if (currentFigure.Body[row, col])
                    {
                        Write("*", row + 1 + currentFigureRow, 1 + currentFigureColumn + col);
                    }
                }
            }
        }

        private void Write(string text, int row, int col)
        {
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }
    }
}
