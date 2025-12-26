using Chessgame.Board;
using Chessgame.Board.Exceptions;

namespace Chessgame.Chess;

public class ChessMatch
{
    public GameBoard GameBoard { get; private set; }
    public bool IsFinished { get; private set; }
    public int Turn { get; private set; }
    public Color CurrentPlayer { get; private set; }

    public ChessMatch()
    {
        GameBoard = new GameBoard(8, 8);
        IsFinished = false;
        Turn = 1;
        CurrentPlayer = Color.White;
        PutPieces();
    }

    public void MovePiece(Position source, Position target)
    {
        Piece piece = GameBoard.RemovePiece(source);
        piece.IncreaseMoveCount();
        Piece capturedPiece = GameBoard.RemovePiece(target);
        GameBoard.PlacePiece(piece, target);
    }

    public void MakeMove(Position source, Position target)
    {
        MovePiece(source, target);
        Turn++;
        ChangePlayer();
    }

    public void ValidateSourcePosition(Position position)
    {
        if (GameBoard.GetPiece(position) is null)
        {
            throw new GameBoardException("There is no piece on source position.");
        }
        if (CurrentPlayer != GameBoard.GetPiece(position).Color)
        {
            throw new GameBoardException("The chosen piece is not yours.");
        }
        if (!GameBoard.GetPiece(position).HasPossibleMoves())
        {
            throw new GameBoardException("There are no possible moves for the chosen piece.");
        }
    }

    public void ValidateTargetPosition(Position sourcePosition, Position targetPosition)
    {
        if (!GameBoard.GetPiece(sourcePosition).CanMoveTo(targetPosition))
        {
            throw new GameBoardException("Target position invalid!");
        }
    }

    private void ChangePlayer()
    {
        CurrentPlayer = CurrentPlayer == Color.White ? Color.Black : Color.White;
    }

    private void PutPieces()
    {
        GameBoard.PlacePiece(new Rook(GameBoard, Color.White), new Position(0, 0));
        GameBoard.PlacePiece(new Rook(GameBoard, Color.White), new Position(0, 5));
        GameBoard.PlacePiece(new Rook(GameBoard, Color.Black), new Position(7, 7));
        GameBoard.PlacePiece(new King(GameBoard, Color.Black), new Position(0, 4));
    }
}
