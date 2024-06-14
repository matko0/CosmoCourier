using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the character
    public float jumpForce = 10f; // Jump force of the character
    public float ladderExitSpeed = 2f; // Speed to set when exiting the ladder
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private bool isGrounded; // Check if the character is on the ground
    private bool isOnLadder; // Check if the character is on a ladder
    public Transform groundCheck; // Reference to the ground check position
    public float checkRadius = 0.1f; // Radius of the ground check
    public LayerMask whatIsGround; // Layer mask to define what is ground
    public LayerMask whatIsLadder; // Layer mask to define what is ladder

    private bool atLadderTop; // Check if the character is at the top of the ladder

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        // Get input from the player for horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Check if the character is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (isOnLadder)
        {
            // Get input from the player for vertical movement
            float verticalInput = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, verticalInput * moveSpeed);
            rb.gravityScale = 0; // Disable gravity while climbing
        }
        else
        {
            rb.gravityScale = 1; // Enable gravity when not climbing
        }

        // Jump if the player presses the jump button and the character is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = true;
            atLadderTop = false;
        }
        else if (collision.CompareTag("LadderTop"))
        {
            atLadderTop = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isOnLadder = false;

            if (atLadderTop)
            {
                // Reset vertical velocity to ladderExitSpeed when exiting the ladder at the top
                rb.velocity = new Vector2(rb.velocity.x, ladderExitSpeed);
            }
        }
        else if (collision.CompareTag("LadderTop"))
        {
            atLadderTop = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a small circle to indicate the ground check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}
