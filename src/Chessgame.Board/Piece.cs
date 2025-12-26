namespace Chessgame.Board;

public abstract class Piece(GameBoard board, Color color)
{
    public Position? Position { get; set; } = null;
    public Color Color { get; protected set; } = color;
    public int MoveCount { get; protected set; } = 0;
    public GameBoard Board { get; protected set; } = board;

    public void IncreaseMoveCount()
    {
        MoveCount++;
    }

    public bool HasPossibleMoves()
    {
        bool[,] possibleMoves = PossibleMoves();

        for (int i = 0; i < Board.Lines; i++)
        {
            for (int j = 0; j < Board.Columns; j++)
            {
                if (possibleMoves[i, j])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool CanMoveTo(Position position)
    {
        return PossibleMoves()[position.Line, position.Column];
    }

    public abstract bool[,] PossibleMoves();
}
