using Chessgame.Board;

namespace Chessgame.Chess;

public class King(GameBoard board, Color color, ChessMatch chessMatch) : Piece(board, color)
{
    private ChessMatch _chessMatch = chessMatch;

    public override bool[,] PossibleMoves()
    {
        bool[,] matrix = new bool[Board.Lines, Board.Columns];

        Position position = new(0, 0);
        Position kingPosition = Position!;

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

        if (MoveCount == 0 && !_chessMatch.IsCheck)
        {
            // special move short castling
            Position positionRook1 = new(kingPosition.Line, kingPosition.Column + 3);

            if (CanRookCastling(positionRook1))
            {
                Position position1 = new(kingPosition.Line, kingPosition.Column + 1);
                Position position2 = new(kingPosition.Line, kingPosition.Column + 2);

                if (Board.GetPiece(position1) is null && Board.GetPiece(position2) is null)
                {
                    matrix[kingPosition.Line, kingPosition.Column + 2] = true;
                }
            }

            // special move big castling
            Position positionRook2 = new(kingPosition.Line, kingPosition.Column - 4);

            if (CanRookCastling(positionRook2))
            {
                Position position1 = new(kingPosition.Line, kingPosition.Column - 1);
                Position position2 = new(kingPosition.Line, kingPosition.Column - 2);
                Position position3 = new(kingPosition.Line, kingPosition.Column - 3);

                if (
                    Board.GetPiece(position1) is null
                    && Board.GetPiece(position2) is null
                    && Board.GetPiece(position3) is null
                )
                {
                    matrix[kingPosition.Line, kingPosition.Column - 2] = true;
                }
            }
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

    private bool CanRookCastling(Position position)
    {
        if (!Board.ValidePosition(position))
        {
            return false;
        }

        Piece piece = Board.GetPiece(position);

        return piece is not null && piece is Rook && piece.Color == Color && MoveCount == 0;
    }
}
