using UnityEngine;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Button spawnButton;

    [Header("Cost")]
    [SerializeField] private int spawnCost = 1;

    private void Start()
    {
        // Hooks the button up in code, so don't also add SpawnBall to its OnClick in the Inspector
        spawnButton.onClick.AddListener(SpawnBall);

        GameManager.Instance.OnCreditsChanged += RefreshButton;
        RefreshButton(GameManager.Instance.Credits);
    }

    private void OnDestroy()
    {
        if (spawnButton != null)
            spawnButton.onClick.RemoveListener(SpawnBall);

        if (GameManager.Instance != null)
            GameManager.Instance.OnCreditsChanged -= RefreshButton;
    }

    public void SpawnBall()
    {
        // Spends the credit; does nothing if there aren't enough
        if (!GameManager.Instance.TrySpendCredits(spawnCost)) return;

        Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
    }

    // Greyed out and unclickable when there aren't enough credits
    private void RefreshButton(int credits)
    {
        spawnButton.interactable = credits >= spawnCost;
    }
}