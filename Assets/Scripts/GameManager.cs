// GameManager.cs
using UnityEngine;
using System.Collections;

/// <summary>
/// Orchestrates the game: model, AI selection, and UI updates.
/// Keeps responsibilities separate (flow control only).
/// </summary>
public class GameManager : MonoBehaviour
{
    public int startingCoins = 10;

    public enum GameMode { PvP = 0, PvAI = 1 }
    public GameMode mode = GameMode.PvAI;

    public enum AIType { Grundy = 0, Minimax = 1 }
    public AIType aiType = AIType.Grundy;

    [Header("References")]
    public UIController ui;

    CoinStripModel model;
    IAIEngine aiEngine;

    void Start()
    {
        ui.BindButtons(OnPlayerTake, OnRestart, OnModeChanged, OnAITypeChanged);
        StartGame();
    }

    public void StartGame()
    {
        model = new CoinStripModel(startingCoins);
        CreateAI();
        ui.modeDropdown.value = (int)mode;
        ui.aiDropdown.value = (int)aiType;
        RefreshUI();

        // If human goes second and mode==PvAI, let AI play first
        if (mode == GameMode.PvAI && model.CurrentPlayer == Player.Player2)
            StartCoroutine(AITurn());
    }

    void CreateAI()
    {
        aiEngine = (mode == GameMode.PvAI)
            ? ((aiType == AIType.Minimax) ? (IAIEngine)new MinimaxAI() : new GrundyAI())
            : null;
    }

    void OnModeChanged(int idx)
    {
        mode = (GameMode)idx;
        StartGame();
    }

    void OnAITypeChanged(int idx)
    {
        aiType = (AIType)idx;
        CreateAI();
    }

    void OnRestart()
    {
        StartGame();
    }

    void OnPlayerTake(int amount)
    {
        if (model.isGameOver) return;
        bool isHumanTurn = (mode == GameMode.PvP) || (model.CurrentPlayer == Player.Player1);
        if (!isHumanTurn) return;
        if (!model.CanTake(amount)) return;

        model.ApplyMove(amount);
        RefreshUI();

        if (model.isGameOver) return;

        if (mode == GameMode.PvAI && model.CurrentPlayer == Player.Player2)
            StartCoroutine(AITurn());
    }

    IEnumerator AITurn()
    {
        yield return new WaitForSeconds(0.5f);
        if (aiEngine == null) yield break;
        int move = aiEngine.GetBestMove(model.Coins);
        if (move <= 0 || !model.CanTake(move)) move = model.Coins >= 1 ? 1 : 0;
        model.ApplyMove(move);
        RefreshUI();
    }

    void RefreshUI()
    {
        Player? winner = model.isGameOver ? model.GetWinner() : (Player?)null;
        bool isHumanTurn = (mode == GameMode.PvP) || (model.CurrentPlayer == Player.Player1);
        ui.UpdateUI(model.Coins, model.CurrentPlayer, model.isGameOver, winner, isHumanTurn);
    }
}
