using Chessgame.Board;

namespace Chessgame.Chess;

public class Queen(GameBoard board, Color color) : Piece(board, color)
{
    public override bool[,] PossibleMoves()
    {
        bool[,] matrix = new bool[Board.Lines, Board.Columns];
        Position position = new(0, 0);
        int[,] directions =
        {
            { -1, 0 },
            { -1, 1 },
            { 0, 1 },
            { 1, 1 },
            { 1, 0 },
            { 1, -1 },
            { 0, -1 },
            { -1, -1 },
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            position.SetValues(
                Position.Line + directions[i, 0],
                Position.Column + directions[i, 1]
            );
            while (Board.ValidePosition(position) && CanMove(position))
            {
                matrix[position.Line, position.Column] = true;
                if (
                    Board.GetPiece(position) is not null
                    && Board.GetPiece(position)!.Color != Color
                )
                {
                    break;
                }

                position.SetValues(
                    position.Line + directions[i, 0],
                    position.Column + directions[i, 1]
                );
            }
        }

        return matrix;
    }

    public override string ToString()
    {
        return "Q";
    }

    private bool CanMove(Position position)
    {
        Piece? piece = Board.GetPiece(position);
        return piece is null || piece.Color != Color;
    }
}
