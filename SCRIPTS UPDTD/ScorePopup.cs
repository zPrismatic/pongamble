using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private float displayTime = 1.5f;
    [FormerlySerializedAs("popupText")]
    [SerializeField] private TMP_Text enemyText;
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private string[] enemyScoredMessages = { "L", "ez", "GG GO NEXT", ":skull:" };
    [SerializeField] private string[] playerScoredMessages = { "cheater !", "HOW", "reported" };

    private float enemyHideTime;
    private float playerHideTime;

    private void Start()
    {
        GameManager.Instance.OnPointScored += ShowPointMessage;
        GameManager.Instance.OnPowerUsed += ShowPowerMessage;
    }

    private void Update()
    {
        if (enemyText != null) enemyText.enabled = Time.time < enemyHideTime;
        if (playerText != null) playerText.enabled = Time.time < playerHideTime;
    }

    private void ShowPointMessage(int scorer)
    {
        if (scorer == 1)
            Show(playerText, playerScoredMessages[Random.Range(0, playerScoredMessages.Length)], ref playerHideTime);
        else
            Show(enemyText, enemyScoredMessages[Random.Range(0, enemyScoredMessages.Length)], ref enemyHideTime);
    }

    private void ShowPowerMessage(string message) => Show(enemyText, message, ref enemyHideTime);

    private void Show(TMP_Text text, string message, ref float hideTime)
    {
        if (text == null) return;

        text.text = message;
        hideTime = Time.time + displayTime;
    }
}
