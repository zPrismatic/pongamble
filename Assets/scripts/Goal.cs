using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private int playerScoredOn = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PongBall ball = other.GetComponent<PongBall>();
        if (ball == null) return;

        GameManager.Instance.GoalScoredOn(playerScoredOn);
        ball.ResetToCenter();
    }
}