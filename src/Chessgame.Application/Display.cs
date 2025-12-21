using Chessgame.Board;
using Chessgame.Chess;

namespace Chessgame.Application;

public static class Display
{
    private const int BoardSizeLines = 8;
    private const string BoardLetters = "  a b c d e f g h";

    public static void DisplayGameBoard(GameBoard board)
    {
        for (int i = 0; i < board.Lines; i++)
        {
            Console.Write($"{BoardSizeLines - i} ");
            for (int j = 0; j < board.Columns; j++)
            {
                Piece? piece = board.GetPiece(i, j);

                if (piece is null)
                {
                    Console.Write("- ");
                    continue;
                }

                DisplayPiece(piece);
                Console.Write(" ");
            }

            Console.WriteLine();
        }

        Console.WriteLine(BoardLetters);
    }

    public static ChessPosition ReadChessPosition()
    {
        string chessPosition = Console.ReadLine();
        char column = chessPosition[0];
        int line = int.Parse(chessPosition[1] + "");
        return new ChessPosition(column, line);
    }

    private static void DisplayPiece(Piece piece)
    {
        if (piece.Color == Color.Black)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write(piece);
            Console.ForegroundColor = previousColor;
            return;
        }

        Console.Write(piece);
    }
}
