using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class AIPaddle : MonoBehaviour
{
    [SerializeField] private float noobSpeed = 3f;
    [SerializeField] private float proSpeedBelowBall = 1f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    [Header("Teleport (pro mode)")]
    [SerializeField] private string teleportMessage = "TELEPORT SPELL";
    [SerializeField] private float teleportTriggerDistance = 3f;
    [SerializeField] private float teleportCooldown = 0f;

    [Header("Fireball (pro mode)")]
    [SerializeField] private string fireballMessage = "FIREBALL";
    [SerializeField, Range(0f, 1f)] private float fireballChance = 0.3f;
    [SerializeField] private float fireballBonusSpeed = 10f;

    [Header("Finale")]
    [SerializeField] private float finaleBallGap = 0.1f;

    private Rigidbody2D rb;
    private Collider2D paddleCollider;
    private Vector2 startPosition;
    private float nextTeleportTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        paddleCollider = GetComponent<Collider2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPosition = rb.position;
    }

    private void Start() => GameManager.Instance.OnFinale += StartFinale;

    private void FixedUpdate()
    {
        GameManager manager = GameManager.Instance;
        if (!manager.IsPlaying) return;

        Rigidbody2D ball = manager.Ball.Body;
        if (manager.ProMode && TryTeleport(ball)) return;

        float speed = manager.ProMode
            ? Mathf.Max(0f, ball.linearVelocity.magnitude - proSpeedBelowBall)
            : noobSpeed;

        float targetY = Mathf.Clamp(ball.position.y, minY, maxY);
        float newY = Mathf.MoveTowards(rb.position.y, targetY, speed * Time.fixedDeltaTime);

        rb.MovePosition(new Vector2(rb.position.x, newY));
    }

    private void StartFinale()
    {
        rb.position = startPosition;

        PongBall ball = GameManager.Instance.Ball;
        float gap = paddleCollider.bounds.extents.x + ball.GetComponent<Collider2D>().bounds.extents.x + finaleBallGap;

        ball.StartFinale(startPosition + Vector2.left * gap);
    }

    private bool TryTeleport(Rigidbody2D ball)
    {
        float distance = rb.position.x - ball.position.x;
        bool incoming = ball.linearVelocity.x * distance > 0f && Mathf.Abs(distance) <= teleportTriggerDistance;
        if (!incoming || Time.time < nextTeleportTime) return false;

        float targetY = Mathf.Clamp(ball.position.y, minY, maxY);
        if (Mathf.Abs(targetY - rb.position.y) <= paddleCollider.bounds.extents.y) return false;

        rb.position = new Vector2(rb.position.x, targetY);
        nextTeleportTime = Time.time + teleportCooldown;
        GameManager.Instance.AnnouncePower(teleportMessage);
        return true;
    }

    public float RollFireballBonus()
    {
        if (!GameManager.Instance.ProMode || Random.value >= fireballChance) return 0f;

        GameManager.Instance.AnnouncePower(fireballMessage);
        return fireballBonusSpeed;
    }
}
