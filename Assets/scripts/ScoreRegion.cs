using UnityEngine;

public class ScoreRegion : MonoBehaviour
{
    [SerializeField] private int playerScoredOn = 1;
    [SerializeField] private int points = 1;

    private Collider2D regionCollider;

    private void Awake()
    {
        regionCollider = GetComponent<Collider2D>();
        regionCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PongBall ball = other.GetComponent<PongBall>();
        if (ball == null) return;

        Bounds bounds = regionCollider.bounds;
        float y = ball.transform.position.y;
        if (y < bounds.min.y || y >= bounds.max.y) return;

        GameManager.Instance.GoalScoredOn(playerScoredOn, points);
        ball.ResetToCenter();
    }
}