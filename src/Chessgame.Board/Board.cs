namespace Chessgame.Board;

public class Board(int lines, int columns)
{
    public int Lines { get; set; } = lines;
    public int Columns { get; set; } = columns;
    private readonly Piece[,] _pieces = new Piece[lines, columns];
}
