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

    public void ValidateTargetPosition(Position sourcePosition, Position targetPosition)
    {
        if (!GameBoard.GetPiece(sourcePosition).CanMoveTo(targetPosition))
        {
            throw new GameBoardException("Target position invalid!");
        }
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

    private Piece MovePiece(Position source, Position target)
    {
        Piece piece = GameBoard.RemovePiece(source);
        piece.IncreaseMoveCount();
        Piece capturedPiece = GameBoard.RemovePiece(target);
        GameBoard.PlacePiece(piece, target);

        if (capturedPiece is not null)
        {
            CapturedPieces.Add(capturedPiece);
        }

        // short castling
        if (piece is King && target.Column == source.Column + 2)
        {
            Position positionSourceRook = new(source.Line, source.Column + 3);
            Position positionTargetRook = new(source.Line, source.Column + 1);

            Piece rook = GameBoard.RemovePiece(positionSourceRook);
            rook.IncreaseMoveCount();
            GameBoard.PlacePiece(rook, positionTargetRook);
        }

        // big castling
        if (piece is King && target.Column == source.Column - 2)
        {
            Position positionSourceRook = new(source.Line, source.Column - 4);
            Position positionTargetRook = new(source.Line, source.Column - 1);

            Piece rook = GameBoard.RemovePiece(positionSourceRook);
            rook.IncreaseMoveCount();
            GameBoard.PlacePiece(rook, positionTargetRook);
        }

        return capturedPiece;
    }

    private bool IsInCheck(Color color)
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

    private void UndoMove(Position source, Position target, Piece capturedPiece)
    {
        Piece piece = GameBoard.RemovePiece(target);
        piece.DecreaseMoveCount();

        if (capturedPiece is not null)
        {
            GameBoard.PlacePiece(capturedPiece, source);
            CapturedPieces.Remove(capturedPiece);
        }

        GameBoard.PlacePiece(piece, source);

        // short castling
        if (piece is King && target.Column == source.Column + 2)
        {
            Position positionSourceRook = new(source.Line, source.Column + 3);
            Position positionTargetRook = new(source.Line, source.Column + 1);

            Piece rook = GameBoard.RemovePiece(positionTargetRook);
            rook.DecreaseMoveCount();
            GameBoard.PlacePiece(rook, positionSourceRook);
        }

        // big castling
        if (piece is King && target.Column == source.Column - 2)
        {
            Position positionSourceRook = new(source.Line, source.Column - 4);
            Position positionTargetRook = new(source.Line, source.Column - 1);

            Piece rook = GameBoard.RemovePiece(positionTargetRook);
            rook.DecreaseMoveCount();
            GameBoard.PlacePiece(rook, positionSourceRook);
        }
    }

    private static Color OpponentColor(Color color)
    {
        return color == Color.White ? Color.Black : Color.White;
    }

    private bool IsInCheckmate(Color color)
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

    private void ChangePlayer()
    {
        CurrentPlayer = CurrentPlayer == Color.White ? Color.Black : Color.White;
    }

    private Piece KingByColor(Color color)
    {
        var piecesInGame = PiecesInGame(color);
        var king = piecesInGame.FirstOrDefault(piece => piece is King);

        return king ?? throw new GameBoardException($"The King not found color {color}");
    }

    private HashSet<Piece> PiecesInGame(Color color)
    {
        HashSet<Piece> pieces = Pieces.Where(piece => piece.Color == color).ToHashSet();

        var capturedPiecesByColor = CapturedPiecesByColor(color);

        pieces.ExceptWith(capturedPiecesByColor);

        return pieces;
    }

    private void PutPieces()
    {
        PutWhitePieces();
        PutBlackPieces();
    }

    private void PutWhitePieces()
    {
        PutNewPieces('a', 1, new Rook(GameBoard, Color.White));
        PutNewPieces('b', 1, new Knight(GameBoard, Color.White));
        PutNewPieces('c', 1, new Bishop(GameBoard, Color.White));
        PutNewPieces('d', 1, new Queen(GameBoard, Color.White));
        PutNewPieces('e', 1, new King(GameBoard, Color.White, this));
        PutNewPieces('f', 1, new Bishop(GameBoard, Color.White));
        PutNewPieces('g', 1, new Knight(GameBoard, Color.White));
        PutNewPieces('h', 1, new Rook(GameBoard, Color.White));

        for (int i = 'a'; i <= 'h'; i++)
        {
            PutNewPieces((char)i, 2, new Pawn(GameBoard, Color.White));
        }
    }

    private void PutBlackPieces()
    {
        PutNewPieces('a', 8, new Rook(GameBoard, Color.Black));
        PutNewPieces('b', 8, new Knight(GameBoard, Color.Black));
        PutNewPieces('c', 8, new Bishop(GameBoard, Color.Black));
        PutNewPieces('d', 8, new Queen(GameBoard, Color.Black));
        PutNewPieces('e', 8, new King(GameBoard, Color.Black, this));
        PutNewPieces('f', 8, new Bishop(GameBoard, Color.Black));
        PutNewPieces('g', 8, new Knight(GameBoard, Color.Black));
        PutNewPieces('h', 8, new Rook(GameBoard, Color.Black));

        for (int i = 'a'; i <= 'h'; i++)
        {
            PutNewPieces((char)i, 7, new Pawn(GameBoard, Color.Black));
        }
    }

    private void PutNewPieces(char column, int line, Piece piece)
    {
        GameBoard.PlacePiece(piece, new ChessPosition(column, line).ToPosition());
        Pieces.Add(piece);
    }
}
