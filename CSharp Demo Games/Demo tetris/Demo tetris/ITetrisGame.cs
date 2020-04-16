namespace DemoTetris
{
    public interface ITetrisGame
    {
        Tetrominoe CurrentFigure { get; set; }
        int CurrentFigureCol { get; set; }
        int CurrentFigureRow { get; set; }
        int Level { get; }
        int TetrisColumns { get; }
        bool[,] TetrisField { get; }
        int TetrisRows { get; }

        void AddCurrentFigureToTetrisField();
        int CheckForFullLines();
        bool Collision(Tetrominoe figure);
        void NewRandomFugire();
        void UpdateLevel(int score);
    }
}