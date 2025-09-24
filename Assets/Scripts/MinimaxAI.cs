using System;
using JetBrains.Annotations;
using UnityEngine;
/// <summary>
/// Minimax with memoization for the subtraction set {1,2}.
/// Returns 1 (winning) or -1 (losing) via local recursive evaluator, and picks a move.
/// </summary>
public class MinimaxAI : IAIEngine
{
    public int GetBestMove(int coins)
    {
        if (coins <= 0) return 0;

        var memo = new int[coins + 1];

        int Eval(int n)
        {
            if (n == 0) return -1;
            if (memo[n] != 0) return memo[n];
            for (int mv = 1; mv <= 2 && mv <= n; mv++)
            {
                if (Eval(n - mv) == -1)
                {
                    memo[n] = 1;
                    return 1;
                }
                memo[n] = -1;
            }
            return -1;
        }

        for (int mv = 1; mv <= 2 && mv <= coins; mv++)
        {
            if (Eval(coins - mv) == -1)
            {
                return mv;
            }
        }

        return Math.Min(1, coins);
    }
}
