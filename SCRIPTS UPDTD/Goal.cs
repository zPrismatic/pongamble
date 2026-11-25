using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private int playerScoredOn = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PongBall>() != null)
            GameManager.Instance.GoalScoredOn(playerScoredOn);
    }
}
