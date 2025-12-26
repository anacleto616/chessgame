using Chessgame.Board;
using Chessgame.Board.Exceptions;

namespace Chessgame.Chess;

public class ChessMatch
{
    public Color CurrentPlayer { get; private set; }
    public GameBoard GameBoard { get; private set; }
    public bool IsFinished { get; private set; }
    public int Turn { get; private set; }
    public HashSet<Piece> Pieces;
    public HashSet<Piece> CapturedPieces;

    public ChessMatch()
    {
        GameBoard = new GameBoard(8, 8);
        IsFinished = false;
        Turn = 1;
        CurrentPlayer = Color.White;
        Pieces = new HashSet<Piece>();
        CapturedPieces = new HashSet<Piece>();
        PutPieces();
    }

    public void MovePiece(Position source, Position target)
    {
        Piece piece = GameBoard.RemovePiece(source);
        piece.IncreaseMoveCount();
        Piece capturedPiece = GameBoard.RemovePiece(target);
        GameBoard.PlacePiece(piece, target);

        if (capturedPiece is not null)
        {
            CapturedPieces.Add(capturedPiece);
        }
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

    public HashSet<Piece> CapturedPiecesByColor(Color color)
    {
        HashSet<Piece> pieces = new HashSet<Piece>();

        foreach (Piece piece in CapturedPieces)
        {
            if (piece.Color == color)
            {
                pieces.Add(piece);
            }
        }

        return pieces;
    }

    public HashSet<Piece> PiecesInGame(Color color)
    {
        HashSet<Piece> pieces = new HashSet<Piece>();

        foreach (Piece piece in Pieces)
        {
            if (piece.Color == color)
            {
                pieces.Add(piece);
            }
        }

        pieces.ExceptWith(CapturedPiecesByColor(color));

        return pieces;
    }

    public void PutNewPieces(char column, int line, Piece piece)
    {
        GameBoard.PlacePiece(piece, new ChessPosition(column, line).ToPosition());
        Pieces.Add(piece);
    }

    private void ChangePlayer()
    {
        CurrentPlayer = CurrentPlayer == Color.White ? Color.Black : Color.White;
    }

    private void PutPieces()
    {
        PutNewPieces('c', 1, new Rook(GameBoard, Color.White));
        PutNewPieces('d', 1, new King(GameBoard, Color.White));
        PutNewPieces('c', 8, new Rook(GameBoard, Color.Black));
        PutNewPieces('d', 8, new King(GameBoard, Color.Black));
    }
}
