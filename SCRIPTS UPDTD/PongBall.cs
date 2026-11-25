using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PongBall : MonoBehaviour
{
    [FormerlySerializedAs("speed")]
    [SerializeField] private float startSpeed = 8f;
    [SerializeField] private float proStartSpeed = 6f;
    [SerializeField] private float speedIncreasePerBounce = 0.5f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private Vector2 respawnPosition = Vector2.zero;
    [SerializeField] private float finaleDelay = 3f;
    [SerializeField] private float finaleSpeed = 100f;

    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    private float currentSpeed;
    private float speedBonus;
    private float normalTimeStep;

    public Rigidbody2D Body => rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start() => Launch();

    private void FixedUpdate() => lastVelocity = rb.linearVelocity;

    private void OnDestroy()
    {
        if (normalTimeStep > 0f)
            Time.fixedDeltaTime = normalTimeStep;
    }

    public void ResetToCenter()
    {
        rb.position = respawnPosition;
        Launch();
    }

    public void RandomizeDirection()
    {
        float x = Random.value < 0.5f ? -1f : 1f;
        Vector2 direction = new Vector2(x, Random.Range(-1f, 1f)).normalized;

        rb.linearVelocity = direction * (currentSpeed + speedBonus);
    }

    public void StartFinale(Vector2 position) => StartCoroutine(FinaleRoutine(position));

    private IEnumerator FinaleRoutine(Vector2 position)
    {
        rb.position = position;
        rb.linearVelocity = Vector2.zero;
        speedBonus = 0f;

        yield return new WaitForSeconds(finaleDelay);

        normalTimeStep = Time.fixedDeltaTime;
        Time.fixedDeltaTime = normalTimeStep / 4f;

        currentSpeed = finaleSpeed;
        rb.linearVelocity = Vector2.left * finaleSpeed;
    }

    private void Launch()
    {
        currentSpeed = GameManager.Instance.ProMode ? proStartSpeed : startSpeed;
        speedBonus = 0f;
        RandomizeDirection();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentSpeed = Mathf.Min(currentSpeed + speedIncreasePerBounce, maxSpeed);

        if (speedBonus <= 0f && collision.gameObject.TryGetComponent(out AIPaddle enemy))
            speedBonus = enemy.RollFireballBonus();

        Vector2 bounced = Vector2.Reflect(lastVelocity, collision.GetContact(0).normal);
        rb.linearVelocity = bounced.normalized * (currentSpeed + speedBonus);
    }
}
