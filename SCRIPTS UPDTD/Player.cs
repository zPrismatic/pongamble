using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private KeyCode upKey = KeyCode.Z;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;
    [SerializeField] private Sprite hitSprite;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool frozen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPosition = rb.position;
    }

    private void Start() => GameManager.Instance.OnFinale += StartFinale;

    private void FixedUpdate()
    {
        if (frozen || !GameManager.Instance.IsPlaying) return;

        float input = (Input.GetKey(upKey) ? 1f : 0f) - (Input.GetKey(downKey) ? 1f : 0f);
        float newY = Mathf.Clamp(rb.position.y + input * speed * Time.fixedDeltaTime, minY, maxY);

        rb.MovePosition(new Vector2(rb.position.x, newY));
    }

    private void StartFinale()
    {
        frozen = true;
        rb.position = startPosition;
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitSprite != null && other.GetComponent<PongBall>() != null)
            GetComponentInChildren<SpriteRenderer>().sprite = hitSprite;
    }
}
