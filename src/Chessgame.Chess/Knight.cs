using Chessgame.Board;

namespace Chessgame.Chess;

public class Knight(GameBoard board, Color color) : Piece(board, color)
{
    public override bool[,] PossibleMoves()
    {
        bool[,] matrix = new bool[Board.Lines, Board.Columns];
        Position position = new(0, 0);
        int[,] moves =
        {
            { -2, -1 },
            { -2, 1 },
            { -1, -2 },
            { -1, 2 },
            { 1, -2 },
            { 1, 2 },
            { 2, -1 },
            { 2, 1 },
        };

        for (int i = 0; i < moves.GetLength(0); i++)
        {
            position.SetValues(Position.Line + moves[i, 0], Position.Column + moves[i, 1]);
            if (Board.ValidePosition(position) && CanMove(position))
            {
                matrix[position.Line, position.Column] = true;
            }
        }

        return matrix;
    }

    public override string ToString()
    {
        return "N";
    }

    private bool CanMove(Position position)
    {
        Piece? piece = Board.GetPiece(position);
        return piece is null || piece.Color != Color;
    }
}
