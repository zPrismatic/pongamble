using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PongBall : MonoBehaviour
{
    [Header("Speed")]
    [FormerlySerializedAs("speed")]
    [SerializeField] private float startSpeed = 8f;
    [SerializeField] private float speedIncreasePerBounce = 0.5f;
    [SerializeField] private float maxSpeed = 100f;

    [Header("Respawn")]
    [SerializeField] private Vector2 respawnPosition = Vector2.zero;

    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        Launch();
    }

    private void Launch()
    {
        currentSpeed = startSpeed;

        float x = Random.value < 0.5f ? -1f : 1f;
        Vector2 direction = new Vector2(x, Random.Range(-1f, 1f)).normalized;

        rb.linearVelocity = direction * currentSpeed;
        lastVelocity = rb.linearVelocity;
    }

    public void ResetToCenter()
    {
        rb.position = respawnPosition;
        Launch();
    }

    private void FixedUpdate()
    {
        lastVelocity = rb.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentSpeed = Mathf.Min(currentSpeed + speedIncreasePerBounce, maxSpeed);

        Vector2 normal = collision.GetContact(0).normal;
        Vector2 reflected = Vector2.Reflect(lastVelocity, normal);

        rb.linearVelocity = reflected.normalized * currentSpeed;
    }
}