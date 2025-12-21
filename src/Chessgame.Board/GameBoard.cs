using Chessgame.Board.Exceptions;

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

    public Piece? GetPiece(Position position)
    {
        return _pieces[position.Line, position.Column];
    }

    public bool HasPiece(Position position)
    {
        ValidatePosition(position);
        return GetPiece(position) is not null;
    }

    public void PlacePiece(Piece piece, Position position)
    {
        if (HasPiece(position))
        {
            throw new GameBoardException("There is already a piece on this position!");
        }

        _pieces[position.Line, position.Column] = piece;
        piece.Position = position;
    }

    public bool ValidePosition(Position position)
    {
        if (
            position.Line < 0
            || position.Line >= Lines
            || position.Column < 0
            || position.Column >= Columns
        )
        {
            return false;
        }

        return true;
    }

    public void ValidatePosition(Position position)
    {
        if (!ValidePosition(position))
        {
            throw new GameBoardException("Invalid position!");
        }
    }
}
