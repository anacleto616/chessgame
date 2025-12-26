using Chessgame.Application;
using Chessgame.Board;
using Chessgame.Board.Exceptions;
using Chessgame.Chess;

try
{
    ChessMatch chessMatch = new();

    while (!chessMatch.IsFinished)
    {
        try
        {
            Console.Clear();
            Display.DisplayGameBoard(chessMatch.GameBoard);

            Console.WriteLine();
            Console.WriteLine($"Turn: {chessMatch.Turn}");
            Console.WriteLine($"Waiting player: {chessMatch.CurrentPlayer}");

            Console.WriteLine();
            Console.Write("Source: ");
            Position source = Display.ReadChessPosition().ToPosition();

            chessMatch.ValidateSourcePosition(source);

            bool[,] possibleMoves = chessMatch.GameBoard.GetPiece(source).PossibleMoves();

            Console.Clear();
            Display.DisplayGameBoard(chessMatch.GameBoard, possibleMoves);

            Console.WriteLine();
            Console.Write("Target: ");
            Position target = Display.ReadChessPosition().ToPosition();
            chessMatch.ValidateTargetPosition(source, target);

            chessMatch.MakeMove(source, target);
        }
        catch (GameBoardException gbex)
        {
            Console.WriteLine(gbex.Message);
            Console.ReadLine();
        }
    }
}
catch (GameBoardException gbex)
{
    Console.WriteLine(gbex.Message);
}

Console.ReadLine();
