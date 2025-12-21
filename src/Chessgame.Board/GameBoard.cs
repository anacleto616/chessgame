namespace Chessgame.Board;

public class GameBoard(int lines, int columns)
{
    public int Lines { get; set; } = lines;
    public int Columns { get; set; } = columns;
    private readonly Piece[,] _pieces = new Piece[lines, columns];

    public Piece? GetPiece(int line, int column)
    {
        return _pieces[line, column];
    }

    public void PlacePiece(Piece piece, Position position)
    {
        _pieces[position.Line, position.Column] = piece;
        piece.Position = position;
    }
}
