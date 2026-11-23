using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
        Instantiate(ballPrefab, position, Quaternion.identity);
    }
}