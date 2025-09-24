using System;
using UnityEngine;

public enum Player { Player1, Player2 }


/// <summary>
/// Pure model class that contains the game state and rules.
/// Single Responsibility: only state + legal moves + apply move.
/// </summary>

public class CoinStripModel
{
    public int Coins { get; private set; }
    public Player CurrentPlayer { get; private set; }
    public Player? LastMover { get; private set; }

    public CoinStripModel(int startCoins)
    {
        Reset(startCoins);
    }

    public void Reset(int startCoins)
    {
        Coins = Math.Max(0, startCoins);
        CurrentPlayer = Player.Player1;
        LastMover = null;
    }

    public bool CanTake(int amount) => amount >= 1 && amount <= 2 && amount <= Coins;

    /// <summary>
    /// Apply a legal move. Record last mover. Do not toggle player if game ended.
    /// </summary>

    public void ApplyMove(int amount)
    {
        if (!CanTake(amount)) throw new ArgumentException("Illegal move");
        LastMover = CurrentPlayer;
        Coins -= amount;
        if (!isGameOver) TogglePlayer();
    }

    public bool isGameOver => Coins == 0;

    /// <summary>
    /// Returns the winner (player who made last move), or null if game not finished.
    /// </summary>

    public Player? GetWinner() => isGameOver ? LastMover : (Player?)null;

    private void TogglePlayer() => CurrentPlayer = (CurrentPlayer == Player.Player1) ? Player.Player2 : Player.Player1;
}
