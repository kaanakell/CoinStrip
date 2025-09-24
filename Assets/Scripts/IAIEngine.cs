using UnityEngine;

/// <summary>
/// Minimal AI interface (Dependency Inversion).
/// </summary>

public interface IAIEngine
{
    ///<summary>Return best move (1 or 2). Return 0 if no move.</summary>
    int GetBestMove(int coins);
}
