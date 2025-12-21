namespace Chessgame.Board;

public class Position(int line, int column)
{
    public int Line { get; set; } = line;
    public int Column { get; set; } = column;

    public override string ToString()
    {
        return $"{Line}, {Column}";
    }
}
