using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Joystick")]
    [SerializeField] private JoystickController joystick;

    private Vector2 input;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal") + joystick.Horizontal;
        input.y = Input.GetAxisRaw("Vertical") + joystick.Vertical;

        input = Vector2.ClampMagnitude(input, 1f);

        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        Vector2 targetPos = rb.position + input * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }
}