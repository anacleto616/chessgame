using Chessgame.Board;

namespace Chessgame.Chess;

public class ChessMatch
{
    public GameBoard GameBoard { get; private set; }
    public bool IsFinished { get; private set; }
    private int _turn;
    private Color _currentPlayer;

    public ChessMatch()
    {
        GameBoard = new GameBoard(8, 8);
        IsFinished = false;
        _turn = 1;
        _currentPlayer = Color.White;
        PutPieces();
    }

    public void MovePiece(Position source, Position target)
    {
        Piece piece = GameBoard.RemovePiece(source);
        piece.IncreaseMoveCount();
        Piece capturedPiece = GameBoard.RemovePiece(target);
        GameBoard.PlacePiece(piece, target);
    }

    private void PutPieces()
    {
        GameBoard.PlacePiece(new Rook(GameBoard, Color.White), new Position(0, 0));
        GameBoard.PlacePiece(new Rook(GameBoard, Color.White), new Position(0, 5));
        GameBoard.PlacePiece(new Rook(GameBoard, Color.Black), new Position(7, 7));
        GameBoard.PlacePiece(new King(GameBoard, Color.Black), new Position(0, 4));
    }
}
