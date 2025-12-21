namespace Chessgame.Board;

public class Piece(GameBoard board, Color color)
{
    public Position? Position { get; set; } = null;
    public Color Color { get; protected set; } = color;
    public int MoveCount { get; protected set; } = 0;
    public GameBoard Board { get; protected set; } = board;

    public void IncreaseMoveCount()
    {
        MoveCount++;
    }
}
