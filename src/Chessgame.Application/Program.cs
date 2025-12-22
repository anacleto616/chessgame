using Chessgame.Application;
using Chessgame.Board;
using Chessgame.Board.Exceptions;
using Chessgame.Chess;

try
{
    ChessMatch chessMatch = new();

    while (!chessMatch.IsFinished)
    {
        Console.Clear();
        Display.DisplayGameBoard(chessMatch.GameBoard);

        Console.WriteLine();
        Console.Write("Source: ");
        Position source = Display.ReadChessPosition().ToPosition();

        bool[,] possibleMoves = chessMatch.GameBoard.GetPiece(source).PossibleMoves();

        Console.Clear();
        Display.DisplayGameBoard(chessMatch.GameBoard, possibleMoves);

        Console.WriteLine();
        Console.Write("Target: ");
        Position target = Display.ReadChessPosition().ToPosition();

        chessMatch.MovePiece(source, target);
    }
}
catch (GameBoardException gbex)
{
    Console.WriteLine(gbex.Message);
}

Console.ReadLine();
