using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpTime = 0.3f;

    [Header("Ground Check")]
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;
    private bool isJumping;
    private float jumpTimer;

    private void Awake()
    {
        // Safety check (biar nggak error)
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

        // Update Animator
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        HandleJump();
        // HandleBetterGravity();
    }

    private void HandleJump()
    {
        // Mulai lompat
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            jumpTimer = 0f;
            rb.linearVelocity = Vector2.up * jumpForce;
        }

        // Hold jump (biar bisa tinggi rendah)
        if (isJumping && Input.GetButton("Jump"))
        {
            if (jumpTimer < jumpTime)
            {
                rb.linearVelocity = Vector2.up * jumpForce;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        // Lepas tombol
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }

    // private void HandleBetterGravity()
    // {
    //     // Gravity
    //     if (rb.linearVelocity.y < 0)
    //     {
    //         rb.gravityScale = 3f; // jatuh lebih cepat
    //     }
    //     else
    //     {
    //         rb.gravityScale = 1f; // naik normal
    //     }
    // }

    private void OnDrawGizmosSelected()
    {
        if (feetPos != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(feetPos.position, groundDistance);
        }
    }
}