using Chessgame.Board;

namespace Chessgame.Chess;

public class Rook(GameBoard board, Color color) : Piece(board, color)
{
    public override bool[,] PossibleMoves()
    {
        bool[,] matrix = new bool[Board.Lines, Board.Columns];

        Position position = new(0, 0);

        // above
        position.SetValues(Position.Line - 1, Position.Column);
        while (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
            if (Board.GetPiece(position) is not null && Board.GetPiece(position).Color != Color)
            {
                break;
            }

            position.Line--;
        }

        // below
        position.SetValues(Position.Line + 1, Position.Column);
        while (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
            if (Board.GetPiece(position) is not null && Board.GetPiece(position).Color != Color)
            {
                break;
            }

            position.Line++;
        }

        // right
        position.SetValues(Position.Line, Position.Column + 1);
        while (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
            if (Board.GetPiece(position) is not null && Board.GetPiece(position).Color != Color)
            {
                break;
            }

            position.Column++;
        }

        // left
        position.SetValues(Position.Line, Position.Column - 1);
        while (Board.ValidePosition(position) && CanMove(position))
        {
            matrix[position.Line, position.Column] = true;
            if (Board.GetPiece(position) is not null && Board.GetPiece(position).Color != Color)
            {
                break;
            }

            position.Column--;
        }

        return matrix;
    }

    public override string ToString()
    {
        return "R";
    }

    private bool CanMove(Position position)
    {
        Piece? piece = Board.GetPiece(position);
        return piece is null || piece.Color != Color;
    }
}
