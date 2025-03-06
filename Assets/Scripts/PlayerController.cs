using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float acceleration = 50f;
    public float deceleration = 50f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 currentVelocity;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Ensure the Rigidbody2D is set up for top-down movement
        rb.gravityScale = 0f;
        rb.linearDamping = 1f;
        rb.angularDamping = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from WASD or arrow keys
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        // Normalize diagonal movement
        if (moveInput.magnitude > 1f)
        {
            moveInput.Normalize();
        }
    }

    void FixedUpdate()
    {
        // Calculate target velocity based on input
        Vector2 targetVelocity = moveInput * speed;
        
        // Smoothly interpolate current velocity to target velocity
        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            targetVelocity,
            (moveInput.magnitude > 0 ? acceleration : deceleration) * Time.fixedDeltaTime
        );
        
        // Apply the velocity
        rb.linearVelocity = currentVelocity;

        // Flip the sprite based on movement direction
        if (moveInput.x != 0 && spriteRenderer)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }

        // Update the animator with the current velocity
        animator.SetFloat("Speed", currentVelocity.magnitude);
    }
}
