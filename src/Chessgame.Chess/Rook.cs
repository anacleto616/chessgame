using Chessgame.Board;

namespace Chessgame.Chess;

public class Rook(GameBoard board, Color color) : Piece(board, color)
{
    public override string ToString()
    {
        return "R";
    }
}
