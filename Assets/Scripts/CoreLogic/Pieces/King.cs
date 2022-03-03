﻿using System;
using System.Collections.Generic;
using System.Text;


class King : IPiece
{
    /*-------------------------------------LAZY SINGLETON IMPLEMENTATION-*/

    private static readonly Lazy<King> lazy =
        new Lazy<King>( () => new King() );

    public static King Instance { get { return lazy.Value; } }

    private King() { }

    /*-----------------------------------------------------------METHODS-*/

    public void MakeMove(Board board, Move move, int player)
    {
        int dy = move.dst.y - move.src.y;
        int dx = move.dst.x - move.src.x;

        if (dy == 0 // if castling
            && Math.Abs(dx) == 2)
        {
            // move rook
            board.value[move.src.y, (int)(4 + (3.5f * Math.Sign(dx)))] = 0;
            board.value[move.src.y, move.src.x + Math.Sign(dx)] = (int)Pieces.ROOK * player;
        }
    }

    public (List<Pos2> m, List<Pos2> c) ListMoves(Board board, Pos2 pos, int player)
    {
        List<Pos2> maneuvers = new List<Pos2>();
        List<Pos2> captures = new List<Pos2>();

        // first find all normal moves
        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                int y = pos.y + dy;
                int x = pos.x + dx;

                if (y >= 0 && y < 8 // within bounds
                    && x >= 0 && x < 8 // ^^^
                    && Math.Sign(board.value[y, x]) != player) // or is not friendly piece
                {
                    if (Math.Sign(board.value[y, x]) == -player)
                    {
                        captures.Add(new Pos2(y, x));
                    }
                    else maneuvers.Add(new Pos2(y, x));
                }
            }
        }

        // now find castling moves
        // TODO: implement chess960 castling rules (more general)
        if (board.touch[pos.y, pos.x] == 0 // untouched king
            && !board.PosIsHitBy(pos, new uint[] { Pieces.KING }, player)) // not in check
        {
            for (int i = 0; i < 2; i++)
            {
                int dx = 1 - 2 * i; // -1 or 1

                if (board.touch[pos.y, (int)(4 + 3.5f * dx)] == 0) // untouched rook
                {
                    bool flag = false;

                    // make sure squares are open
                    for (int j = 3 + dx; j > 0 && j < 7; j += dx)
                    {
                        if (board.value[pos.y, j] != 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag) continue;

                    maneuvers.Add(new Pos2(pos.y, 3 + 2 * dx));
                }
            }
        }

        return (maneuvers, captures);
    }
}
