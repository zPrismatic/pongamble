using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private KeyCode upKey = KeyCode.Z;
    [SerializeField] private KeyCode downKey = KeyCode.S;

    [Header("Movement")]
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
        input = 0f;

        if (Input.GetKey(upKey)) input += 1f;
        if (Input.GetKey(downKey)) input -= 1f;
    }

    private void FixedUpdate()
    {
        float newY = rb.position.y + input * speed * Time.fixedDeltaTime;
        newY = Mathf.Clamp(newY, minY, maxY);

        rb.MovePosition(new Vector2(rb.position.x, newY));
    }
}