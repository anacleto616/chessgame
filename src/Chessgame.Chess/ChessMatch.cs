using System.Reflection.Metadata;
using Chessgame.Board;
using Chessgame.Board.Exceptions;

namespace Chessgame.Chess;

public class ChessMatch
{
    public Color CurrentPlayer { get; private set; }
    public GameBoard GameBoard { get; private set; }
    public bool IsFinished { get; private set; }
    public int Turn { get; private set; }
    public bool IsCheck { get; private set; }
    public HashSet<Piece> Pieces;
    public HashSet<Piece> CapturedPieces;

    public ChessMatch()
    {
        GameBoard = new GameBoard(8, 8);
        IsFinished = false;
        IsCheck = false;
        Turn = 1;
        CurrentPlayer = Color.White;
        Pieces = [];
        CapturedPieces = [];
        PutPieces();
    }

    public Piece MovePiece(Position source, Position target)
    {
        Piece piece = GameBoard.RemovePiece(source);
        piece.IncreaseMoveCount();
        Piece capturedPiece = GameBoard.RemovePiece(target);
        GameBoard.PlacePiece(piece, target);

        if (capturedPiece is not null)
        {
            CapturedPieces.Add(capturedPiece);
        }

        return capturedPiece;
    }

    public void MakeMove(Position source, Position target)
    {
        Piece capturedPiece = MovePiece(source, target);

        if (IsInCheck(CurrentPlayer))
        {
            UndoMove(source, target, capturedPiece);

            throw new GameBoardException("You can't put yourself in check!");
        }

        var opponentColor = OpponentColor(CurrentPlayer);

        IsCheck = IsInCheck(opponentColor);
        IsFinished = IsInCheckmate(opponentColor);

        if (!IsFinished)
        {
            Turn++;
            ChangePlayer();
        }
    }

    public void UndoMove(Position source, Position target, Piece capturedPiece)
    {
        Piece piece = GameBoard.RemovePiece(target);
        piece.DecreaseMoveCount();

        if (capturedPiece is not null)
        {
            GameBoard.PlacePiece(capturedPiece, source);
            CapturedPieces.Remove(capturedPiece);
        }

        GameBoard.PlacePiece(piece, source);
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
        HashSet<Piece> pieces = [];

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
        HashSet<Piece> pieces = Pieces.Where(piece => piece.Color == color).ToHashSet();

        var capturedPiecesByColor = CapturedPiecesByColor(color);

        pieces.ExceptWith(capturedPiecesByColor);

        return pieces;
    }

    public void PutNewPieces(char column, int line, Piece piece)
    {
        GameBoard.PlacePiece(piece, new ChessPosition(column, line).ToPosition());
        Pieces.Add(piece);
    }

    public bool IsInCheck(Color color)
    {
        Piece king = KingByColor(color);

        foreach (Piece piece in PiecesInGame(OpponentColor(color)))
        {
            bool[,] possibleMoves = piece.PossibleMoves();

            if (possibleMoves[king.Position.Line, king.Position.Column])
            {
                return true;
            }
        }

        return false;
    }

    public bool IsInCheckmate(Color color)
    {
        if (!IsInCheck(color))
        {
            return false;
        }

        var piecesInGame = PiecesInGame(color);

        foreach (Piece piece in piecesInGame)
        {
            bool[,] possibleMoves = piece.PossibleMoves();

            for (int i = 0; i < GameBoard.Lines; i++)
            {
                for (int j = 0; j < GameBoard.Columns; j++)
                {
                    if (possibleMoves[i, j])
                    {
                        Position source = piece.Position;
                        Position target = new(i, j);
                        Piece capturedPiece = MovePiece(source, target);

                        bool testCheck = IsInCheck(color);
                        UndoMove(source, target, capturedPiece);

                        if (!testCheck)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private static Color OpponentColor(Color color)
    {
        return color == Color.White ? Color.Black : Color.White;
    }

    private Piece KingByColor(Color color)
    {
        var piecesInGame = PiecesInGame(color);
        var king = piecesInGame.FirstOrDefault(piece => piece is King);

        return king ?? throw new GameBoardException($"The King not found color {color}");
    }

    private void ChangePlayer()
    {
        CurrentPlayer = CurrentPlayer == Color.White ? Color.Black : Color.White;
    }

    private void PutPieces()
    {
        PutNewPieces('d', 1, new Rook(GameBoard, Color.White));
        PutNewPieces('h', 7, new Rook(GameBoard, Color.White));
        PutNewPieces('e', 1, new King(GameBoard, Color.White));
        PutNewPieces('b', 8, new Rook(GameBoard, Color.Black));
        PutNewPieces('a', 8, new King(GameBoard, Color.Black));
    }
}
