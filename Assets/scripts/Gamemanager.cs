using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Credits")]
    [SerializeField] private int startingCredits = 5;
    [SerializeField] private TMP_Text money;

    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;

    public int Credits { get; private set; }
    public int Player1Score { get; private set; }
    public int Player2Score { get; private set; }

    public event Action<int> OnCreditsChanged;
    public event Action<int, int> OnScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Credits = startingCredits;
    }

    private void Start()
    {
        RefreshUI();
        RefreshScoreUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public bool HasCredits(int amount)
    {
        return Credits >= amount;
    }

    public bool TrySpendCredits(int amount)
    {
        if (Credits < amount) return false;

        Credits -= amount;
        CreditsChanged();
        return true;
    }

    public void AddCredits(int amount)
    {
        Credits += amount;
        CreditsChanged();
    }

    private void CreditsChanged()
    {
        RefreshUI();
        OnCreditsChanged?.Invoke(Credits);
    }

    private void RefreshUI()
    {
        if (money != null)
            money.text = $"money: ${Credits}";
    }

    public void GoalScoredOn(int playerScoredOn, int points = 1)
    {
        if (playerScoredOn == 1)
        {
            Player1Score -= points;
            Player2Score += points;
        }
        else if (playerScoredOn == 2)
        {
            Player2Score -= points;
            Player1Score += points;
        }
        else
        {
            Debug.LogError("GoalScoredOn expects 1 or 2, got " + playerScoredOn);
            return;
        }

        RefreshScoreUI();
        OnScoreChanged?.Invoke(Player1Score, Player2Score);
    }

    private void RefreshScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"{Player1Score} / {Player2Score}";
    }
}