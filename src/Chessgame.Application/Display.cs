using Chessgame.Board;
using Chessgame.Chess;

namespace Chessgame.Application;

public static class Display
{
    private const int BoardSizeLines = 8;
    private const string BoardLetters = "  a b c d e f g h";

    public static void DisplayChessMatch(ChessMatch chessMatch)
    {
        DisplayGameBoard(chessMatch.GameBoard);

        DisplayCapturedPieces(chessMatch);

        Console.WriteLine();
        Console.WriteLine($"Turn: {chessMatch.Turn}");

        if (chessMatch.IsFinished)
        {
            Console.WriteLine("CHECKMATE!");
            Console.WriteLine($"Winner: {chessMatch.CurrentPlayer}");
            return;
        }

        Console.WriteLine($"Waiting player: {chessMatch.CurrentPlayer}");

        if (chessMatch.IsCheck)
        {
            Console.WriteLine("CHECK!");
        }
    }

    public static void DisplayCapturedPieces(ChessMatch chessMatch)
    {
        Console.WriteLine("Captured Pieces:");

        Console.Write("White Pieces: ");
        DisplaySetPieces(chessMatch.CapturedPiecesByColor(Color.White));

        Console.WriteLine();

        Console.Write("Black Pieces: ");

        ConsoleColor originalConsoleColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkBlue;

        DisplaySetPieces(chessMatch.CapturedPiecesByColor(Color.Black));

        Console.ForegroundColor = originalConsoleColor;

        Console.WriteLine();
    }

    public static void DisplaySetPieces(HashSet<Piece> setPieces)
    {
        Console.Write("[");

        foreach (Piece piece in setPieces)
        {
            Console.Write($"{piece} ");
        }

        Console.Write("]");
    }

    public static void DisplayGameBoard(GameBoard board)
    {
        for (int i = 0; i < board.Lines; i++)
        {
            Console.Write($"{BoardSizeLines - i} ");
            for (int j = 0; j < board.Columns; j++)
            {
                Piece? piece = board.GetPiece(i, j);

                DisplayPiece(piece);
            }

            Console.WriteLine();
        }

        Console.WriteLine(BoardLetters);

        Console.WriteLine();
    }

    public static void DisplayGameBoard(GameBoard board, bool[,] possibleMoves)
    {
        ConsoleColor originalBackground = Console.BackgroundColor;
        ConsoleColor highlightedBackground = ConsoleColor.DarkGray;

        for (int i = 0; i < board.Lines; i++)
        {
            Console.Write($"{BoardSizeLines - i} ");
            for (int j = 0; j < board.Columns; j++)
            {
                Console.BackgroundColor = possibleMoves[i, j]
                    ? highlightedBackground
                    : originalBackground;

                Piece? piece = board.GetPiece(i, j);

                DisplayPiece(piece);
                Console.BackgroundColor = originalBackground;
            }

            Console.WriteLine();
        }

        Console.WriteLine(BoardLetters);
        Console.BackgroundColor = originalBackground;
    }

    public static ChessPosition ReadChessPosition()
    {
        string chessPosition = Console.ReadLine();
        char column = chessPosition[0];
        int line = int.Parse(chessPosition[1] + "");
        return new ChessPosition(column, line);
    }

    private static void DisplayPiece(Piece? piece)
    {
        if (piece is not null)
        {
            if (piece.Color == Color.Black)
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write(piece);
                Console.ForegroundColor = previousColor;
                Console.Write(" ");
                return;
            }

            Console.Write(piece);
            Console.Write(" ");
            return;
        }

        Console.Write("- ");
    }
}
