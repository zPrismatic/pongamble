using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        GameManager.Instance.OnGameStarted += SpawnBall;
    }

    private void SpawnBall()
    {
        Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
        GameManager.Instance.Ball = Instantiate(ballPrefab, position, Quaternion.identity).GetComponent<PongBall>();
    }
}
