using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    private Rigidbody2D rb;
    private float input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        input = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        float newY = rb.position.y + input * speed * Time.fixedDeltaTime;
        newY = Mathf.Clamp(newY, minY, maxY);
        rb.MovePosition(new Vector2(rb.position.x, newY));
    }
}