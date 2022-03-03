using System;
using System.Collections.Generic;
using System.Text;

class Queen : IPiece
{
    /*-------------------------------------LAZY SINGLETON IMPLEMENTATION-*/

    private static readonly Lazy<Queen> lazy =
        new Lazy<Queen>( ()=>new Queen() );

    public static Queen Instance { get { return lazy.Value; } }

    private Queen() { }

    /*-----------------------------------------------------------METHODS-*/

    public void MakeMove(Board board, Move move, int player) { }

    public (List<Pos2> m, List<Pos2> c) ListMoves(Board board, Pos2 pos, int player)
    {
        List<Pos2> maneuvers = new List<Pos2>();
        List<Pos2> captures = new List<Pos2>();

        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                var (tmpm, tmpc) = board.CastMoves(pos, dy, dx, player);

                maneuvers.AddRange(tmpm);
                captures.AddRange(tmpc);
            }
        }
        return (maneuvers, captures);
    }
}
