using Chessgame.Board;

namespace Chessgame.Chess;

public class King(GameBoard board, Color color) : Piece(board, color)
{
    public override bool[,] PossibleMoves()
    {
        bool[,] matrix = new bool[Board.Lines, Board.Columns];

        Position position = new(0, 0);

        position.SetValues(Position.Line - 1, Position.Column);
        if (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
        }

        position.SetValues(Position.Line - 1, Position.Column + 1);
        if (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
        }

        position.SetValues(Position.Line, Position.Column + 1);
        if (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
        }

        position.SetValues(Position.Line + 1, Position.Column + 1);
        if (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
        }

        position.SetValues(Position.Line + 1, Position.Column);
        if (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
        }

        position.SetValues(Position.Line + 1, Position.Column - 1);
        if (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
        }

        position.SetValues(Position.Line, Position.Column - 1);
        if (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
        }

        position.SetValues(Position.Line - 1, Position.Column - 1);
        if (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
        }

        return matrix;
    }

    public override string ToString()
    {
        return "K";
    }

    private bool CanMove(Position position)
    {
        Piece? piece = Board.GetPiece(position);
        return piece is null || piece.Color != Color;
    }
}
