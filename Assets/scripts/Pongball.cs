using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PongBall : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField, Range(0.1f, 1f)] private float wallExitAngleMultiplier = 0.25f;
    [SerializeField, Range(0f, 1f)] private float normalWallBounceChance = 0.15f;
    [SerializeField] private float minAngle = 5f;
    [SerializeField] private float maxAngle = 45f;

    private Rigidbody2D rb;
    private Vector2 lastVelocity;

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
        Vector2 direction = new Vector2(-1f, Random.Range(-1f, 1f)).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void FixedUpdate() {lastVelocity = rb.linearVelocity;}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 contactNormal = collision.GetContact(0).normal;
        bool hitTopOrBottom = Mathf.Abs(contactNormal.y) > Mathf.Abs(contactNormal.x);
        Vector2 normal = hitTopOrBottom
            ? new Vector2(0f, Mathf.Sign(contactNormal.y))
            : new Vector2(Mathf.Sign(contactNormal.x), 0f);

        Vector2 reflected = Vector2.Reflect(lastVelocity, normal);

        float angle = Mathf.Atan2(Mathf.Abs(reflected.y), Mathf.Abs(reflected.x)) * Mathf.Rad2Deg;

        if (hitTopOrBottom && Random.value >= normalWallBounceChance)
            angle *= wallExitAngleMultiplier;

        angle = Mathf.Clamp(angle, minAngle, maxAngle);

        float rad = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(
            Mathf.Sign(reflected.x) * Mathf.Cos(rad),
            Mathf.Sign(reflected.y) * Mathf.Sin(rad));
        	rb.linearVelocity = direction * speed;
    }
}