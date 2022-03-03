using System;
using System.Collections.Generic;
using System.Text;


interface IPiece
{
    (List<Pos2> m, List<Pos2> c) ListMoves(Board board, Pos2 pos, int player);
    void MakeMove(Board board, Move move, int player);
}
