using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Blank Piece Set", menuName = "Chess/Piece Set")]
public class PieceSet : ScriptableObject
{
    public Sprite BK,BQ,BR,BB,BN,BP,
                  WK,WQ,WR,WB,WN,WP;

    public Sprite IntToSprite(int i)
    {
        switch (i)
        {
            case -6: return BK;
            case -5: return BQ;
            case -4: return BR;
            case -3: return BB;
            case -2: return BN;
            case -1: return BP;
            case  1: return WP;
            case  2: return WN;
            case  3: return WB;
            case  4: return WR;
            case  5: return WQ;
            case  6: return WK;
            default: return null;
        }
    }
}