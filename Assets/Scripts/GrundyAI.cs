using System;
using UnityEngine;
/// <summary>
/// Very fast AI using the Grundy observation: losing positions are n % 3 == 0.
/// Choose a move that leaves opponent a multiple of 3 when possible.
/// </summary>
public class GrundyAI : IAIEngine
{
    public int GetBestMove(int coins)
    {
        if (coins <= 0) return 0;
        for (int move = 1; move <= 2 && move <= coins; move++)
        {
            if ((coins - move) % 3 == 0)
            {
                return move;
            }
        }
        return Math.Min(1, coins);
    }
}
