using Chessgame.Board;

namespace Chessgame.Chess;

public class Pawn(GameBoard board, Color color, ChessMatch chessMatch) : Piece(board, color)
{
    private readonly ChessMatch _chessMatch = chessMatch;

    public override bool[,] PossibleMoves()
    {
        bool[,] matrix = new bool[Board.Lines, Board.Columns];
        Position position = new(0, 0);
        int dir = Color == Color.White ? -1 : 1;

        // uma casa à frente
        position.SetValues(Position.Line + dir, Position.Column);
        if (Board.ValidePosition(position) && Board.GetPiece(position) is null)
        {
            matrix[position.Line, position.Column] = true;
        }

        // duas casas no primeiro lance
        Position firstStep = new(Position.Line + dir, Position.Column);
        Position secondStep = new(Position.Line + 2 * dir, Position.Column);
        if (
            MoveCount == 0
            && Board.ValidePosition(secondStep)
            && Board.GetPiece(firstStep) is null
            && Board.GetPiece(secondStep) is null
        )
        {
            matrix[secondStep.Line, secondStep.Column] = true;
        }

        // capturas diagonais
        position.SetValues(Position.Line + dir, Position.Column - 1);
        if (
            Board.ValidePosition(position)
            && Board.GetPiece(position) is { } left
            && left.Color != Color
        )
        {
            matrix[position.Line, position.Column] = true;
        }

        position.SetValues(Position.Line + dir, Position.Column + 1);

        if (
            Board.ValidePosition(position)
            && Board.GetPiece(position) is { } right
            && right.Color != Color
        )
        {
            matrix[position.Line, position.Column] = true;
        }

        // en passant
        if (position.Line == 3)
        {
            Position leftPosition = new(position.Line, position.Column - 1);
            if (
                Board.ValidePosition(leftPosition)
                && HasOpponent(leftPosition)
                && Board.GetPiece(leftPosition) == _chessMatch.CanEnPassant
            )
            {
                matrix[leftPosition.Line - 1, leftPosition.Column] = true;
            }

            Position rightPosition = new(position.Line, position.Column + 1);
            if (
                Board.ValidePosition(rightPosition)
                && HasOpponent(rightPosition)
                && Board.GetPiece(rightPosition) == _chessMatch.CanEnPassant
            )
            {
                matrix[rightPosition.Line - 1, rightPosition.Column] = true;
            }
        }

        if (position.Line == 4)
        {
            Position leftPosition = new(position.Line, position.Column - 1);
            if (
                Board.ValidePosition(leftPosition)
                && HasOpponent(leftPosition)
                && Board.GetPiece(leftPosition) == _chessMatch.CanEnPassant
            )
            {
                matrix[leftPosition.Line + 1, leftPosition.Column] = true;
            }

            Position rightPosition = new(position.Line, position.Column + 1);
            if (
                Board.ValidePosition(rightPosition)
                && HasOpponent(rightPosition)
                && Board.GetPiece(rightPosition) == _chessMatch.CanEnPassant
            )
            {
                matrix[rightPosition.Line + 1, rightPosition.Column] = true;
            }
        }

        return matrix;
    }

    public override string ToString()
    {
        return "P";
    }

    private bool CanMove(Position position)
    {
        Piece? piece = Board.GetPiece(position);
        return piece is null || piece.Color != Color;
    }

    private bool HasOpponent(Position position)
    {
        Piece piece = Board.GetPiece(position);

        return piece is not null && piece.Color != Color;
    }

    private bool FreePosition(Position position)
    {
        return Board.GetPiece(position) is null;
    }
}
