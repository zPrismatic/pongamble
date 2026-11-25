using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Button startButton;
    [SerializeField] private GameObject canvasHider;
    [SerializeField] private GameObject endText;
    [SerializeField] private DialoguePopup introDialogue;
    [SerializeField] private DialoguePopup endDialogue;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int scoreToWin = 5;
    [SerializeField] private int aiWinScore = 10;
    [SerializeField] private KeyCode teleportBallKey = KeyCode.P;
    [SerializeField] private GameObject proDragon;
    [SerializeField] private GameObject noobDragon;
    [SerializeField] private GameObject proKing;
    [SerializeField] private GameObject noobKing;

    public PongBall Ball { get; set; }
    public bool IsPlaying => Ball != null;
    public bool ProMode { get; private set; }

    public event Action OnGameStarted;
    public event Action OnGameEnded;
    public event Action OnFinale;
    public event Action<int> OnPointScored;
    public event Action<string> OnPowerUsed;

    private int player1Score;
    private int player2Score;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        RefreshScore();
    }

    private void Update()
    {
        if (!IsPlaying || !Input.GetKeyDown(teleportBallKey)) return;

        Ball.Body.position = Vector2.zero;
        Ball.RandomizeDirection();
    }

    public void StartGame()
    {
        if (IsPlaying) return;

        startButton.gameObject.SetActive(false);
        introDialogue.Open();
    }

    public void BeginGame()
    {
        canvasHider.SetActive(false);
        OnGameStarted?.Invoke();
    }

    public void StartProMode()
    {
        ProMode = true;
        player1Score = 0;
        player2Score = 0;
        RefreshScore();

        if (proDragon != null) proDragon.SetActive(true);
        if (noobDragon != null) noobDragon.SetActive(false);
        if (proKing != null) proKing.SetActive(true);
        if (noobKing != null) noobKing.SetActive(false);

        OnGameStarted?.Invoke();
    }

    public void ShowEnding()
    {
        canvasHider.SetActive(true);

        if (endText != null)
            endText.SetActive(true);
    }

    public void AnnouncePower(string message)
    {
        OnPowerUsed?.Invoke(message);
    }

    public void GoalScoredOn(int playerScoredOn)
    {
        if (!IsPlaying) return;

        int scorer = playerScoredOn == 1 ? 2 : 1;

        if (scorer == 1)
            player1Score++;
        else
            player2Score++;

        RefreshScore();
        OnPointScored?.Invoke(scorer);

        bool ended = ProMode ? player2Score >= aiWinScore : player1Score >= scoreToWin;

        if (ended)
            EndGame();
        else if (ProMode && scorer == 2 && player2Score == aiWinScore - 1)
            OnFinale?.Invoke();
        else
            Ball.ResetToCenter();
    }

    private void EndGame()
    {
        Destroy(Ball.gameObject);
        Ball = null;

        if (ProMode)
            endDialogue.Open();
        else
            OnGameEnded?.Invoke();
    }

    private void RefreshScore()
    {
        if (scoreText != null)
            scoreText.text = $"{player1Score} / {player2Score}";
    }
}
