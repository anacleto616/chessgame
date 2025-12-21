using Chessgame.Board;

namespace Chessgame.Application;

public class Display
{
    public static void DisplayGameBoard(GameBoard board)
    {
        for (int i = 0; i < board.Lines; i++)
        {
            for (int j = 0; j < board.Columns; j++)
            {
                Piece? piece = board.GetPiece(i, j);

                if (piece is null)
                {
                    Console.Write("- ");
                    continue;
                }

                Console.Write($"{piece} ");
            }

            Console.WriteLine();
        }
    }
}
