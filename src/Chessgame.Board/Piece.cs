namespace Chessgame.Board;

public class Piece(Position position, Board board, Color color)
{
    public Position Position { get; set; } = position;
    public Color Color { get; protected set; } = color;
    public int MoveCount { get; protected set; } = 0;
    public Board Board { get; protected set; } = board;
}
