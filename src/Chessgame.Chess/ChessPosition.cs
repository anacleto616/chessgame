using Chessgame.Board;

namespace Chessgame.Chess;

public class ChessPosition(char column, int line)
{
    public char Column { get; set; } = column;
    public int Line { get; set; } = line;

    public Position ToPosition()
    {
        return new Position(8 - Line, Column - 'a');
    }

    public override string ToString()
    {
        return $"{Column}{Line}";
    }
}
