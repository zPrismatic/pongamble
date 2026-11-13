using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Credits")]
    [SerializeField] private int startingCredits = 5;
    [SerializeField] private TMP_Text money;

    public int Credits { get; private set; }
    public event Action<int> OnCreditsChanged;

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
}