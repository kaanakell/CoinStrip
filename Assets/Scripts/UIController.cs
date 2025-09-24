using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

/// <summary>
/// Responsible only for Unity UI wiring and rendering (single responsibility).
/// Exposes bind methods so GameManager attaches logic.
/// </summary>
public class UIController : MonoBehaviour
{
    [Header("UI refs")]
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI resultText;
    public Button take1Button;
    public Button take2Button;
    public Button restartButton;
    public TMP_Dropdown modeDropdown; // 0: PvP, 1: PvAI
    public TMP_Dropdown aiDropdown;   // 0: Grundy, 1: Minimax

    [Header("Optional coin visuals")]
    public Transform coinsParent; // parent to instantiate coinPrefab under
    public GameObject coinPrefab;

    [Header("Coin layout")]
    [Tooltip("Horizontal spacing between coin centers (world units).")]
    public float coinSpacing = 0.7f;
    [Tooltip("Vertical offset (local) of the row from the parent transform.")]
    public float coinVerticalOffset = 0f;
    [Tooltip("Scale applied to instantiated coins.")]
    public Vector3 coinScale = Vector3.one;

    /// <summary>
    /// Bind UI callbacks to game logic.
    /// </summary>
    public void BindButtons(Action<int> onTake, Action onRestart, Action<int> onModeChange, Action<int> onAITypeChange)
    {
        take1Button.onClick.RemoveAllListeners();
        take2Button.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();

        take1Button.onClick.AddListener(() => onTake(1));
        take2Button.onClick.AddListener(() => onTake(2));
        restartButton.onClick.AddListener(() => onRestart());

        modeDropdown.onValueChanged.RemoveAllListeners();
        aiDropdown.onValueChanged.RemoveAllListeners();
        modeDropdown.onValueChanged.AddListener(idx => onModeChange(idx));
        aiDropdown.onValueChanged.AddListener(idx => onAITypeChange(idx));
    }

    /// <summary>
    /// Update visible UI state.
    /// </summary>
    public void UpdateUI(int coins, Player currentPlayer, bool isGameOver, Player? winner, bool isHumanTurn)
    {
        coinsText.text = $"Coins: {coins}";
        turnText.text = isGameOver ? "Game Over" : $"Turn: {(currentPlayer == Player.Player1 ? "Player 1" : "Player 2")}";
        resultText.text = isGameOver ? $"{(winner == Player.Player1 ? "Player 1" : "Player 2")} wins!" : "";
        take1Button.interactable = isHumanTurn && coins >= 1;
        take2Button.interactable = isHumanTurn && coins >= 2;
        UpdateCoinVisuals(coins);
    }

    void UpdateCoinVisuals(int coins)
    {
        if (coinsParent == null || coinPrefab == null) return;

        // destroy existing coin children
        for (int i = coinsParent.childCount - 1; i >= 0; i--)
            Destroy(coinsParent.GetChild(i).gameObject);

        if (coins <= 0) return;

        // compute start to center the row: total width = (coins - 1) * spacing
        float spacing = Math.Max(0.001f, coinSpacing);
        float totalWidth = (coins - 1) * spacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < coins; i++)
        {
            GameObject coin = Instantiate(coinPrefab);
            // Parent without keeping world position, so we can set localPosition reliably
            coin.transform.SetParent(coinsParent, false);

            // set transform local position so coins are laid out horizontally with spacing
            Vector3 localPos = new Vector3(startX + i * spacing, coinVerticalOffset, 0f);
            coin.transform.localPosition = localPos;

            coin.transform.localRotation = Quaternion.identity;
            coin.transform.localScale = coinScale;
        }
    }
}
