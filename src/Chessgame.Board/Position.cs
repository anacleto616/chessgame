namespace Chessgame.Board;

public class Position(int line, int column)
{
    public int Line { get; set; } = line;
    public int Column { get; set; } = column;

    public void SetValues(int line, int column)
    {
        Line = line;
        Column = column;
    }

    public override string ToString()
    {
        return $"{Line}, {Column}";
    }
}
