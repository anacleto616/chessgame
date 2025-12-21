using Chessgame.Application;
using Chessgame.Board;
using Chessgame.Board.Exceptions;
using Chessgame.Chess;

// try
// {
GameBoard board = new(8, 8);

board.PlacePiece(new Rook(board, Color.White), new Position(0, 0));
board.PlacePiece(new Rook(board, Color.Black), new Position(7, 7));
board.PlacePiece(new King(board, Color.Black), new Position(0, 4));

// Display.DisplayGameBoard(board);

// Console.ReadLine();

ChessPosition chessPosition = new('c', 7);

Console.WriteLine(chessPosition);

Console.WriteLine(chessPosition.ToPosition());

// }
// catch (GameBoardException ex)
// {
//     Console.WriteLine($"Game Board Error: {ex.Message}");
// }
