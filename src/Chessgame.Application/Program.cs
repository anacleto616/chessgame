using Chessgame.Application;
using Chessgame.Board;
using Chessgame.Chess;

GameBoard board = new(8, 8);

board.PlacePiece(new Rook(board, Color.White), new Position(0, 0));
board.PlacePiece(new Rook(board, Color.Black), new Position(7, 7));
board.PlacePiece(new King(board, Color.Black), new Position(7, 4));

Display.DisplayGameBoard(board);

Console.ReadLine();
