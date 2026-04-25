using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    bool jump = false;
    public bool crouch = false;
    public Animator animator;
    public Rigidbody2D rb;

    public float dashSpeed = 20f;
    public float dashTime = 0.15f;
    public float dashCooldown = 0.8f;

    private bool isDashing;
    private float dashTimeLeft;
    private float lastDashTime;
    private float dashDirection;

    private float lastMoveDirection = 1f;

    private bool isInLadder = false;
    public bool isClimbing = false;
    private float climbSpeed = 5f;
    private float ladderX;

    void Update()
    {
        PauseMenu pauseMenu = FindAnyObjectByType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.GameIsPaused) return;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Mathf.Abs(horizontalMove) > 0.01f)
        {
            lastMoveDirection = Mathf.Sign(horizontalMove);
        }

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        crouch = verticalMove < -0.7f;
        if (isInLadder)
        {
            crouch = false;
        }

        // Aloita kiipeäminen vain jos ollaan tikapuissa JA painetaan ylös/alas
        if (isInLadder && Mathf.Abs(verticalMove) > 0.5f)
        {
            isClimbing = true;
            animator.SetBool("IsClimbing", true);
        }

        if (isClimbing)
        {
            horizontalMove = 0;
        }

        // Poistu tikapuilta jos ei enää paineta ylös/alas
        if (isClimbing && Mathf.Abs(verticalMove) < 0.5f)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // pysäytä climb
            isClimbing = false;
            animator.SetBool("IsClimbing", false);
        }

        if (Input.GetButton("Dash") && Time.time >= lastDashTime + dashCooldown)
        {
            isDashing = true;
            dashTimeLeft = dashTime;
            lastDashTime = Time.time;

            dashDirection = lastMoveDirection; 
            if (dashDirection == 0) dashDirection = transform.localScale.x > 0 ? 1 : -1;

            animator.SetTrigger("Dash");
        }

        UpdateVerticalAnimations();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isInLadder = true;
            ladderX = collision.bounds.center.x;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isInLadder = false;
            isClimbing = false;
            animator.SetBool("IsClimbing", false);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    void UpdateVerticalAnimations()
    {
        // Tarkistaa pelaajan Y-akselin nopeuden ja selvittää onko pelaaja tippumassa vai hyppäämässä
        float verticalVelocity = rb.linearVelocity.y;
        // Tarkistaa PlayerController.cs scriptistä onko pelaaja maassa(IsGrounded)
        bool isGrounded = controller.IsGrounded;

        if (verticalVelocity > 0.1f && !isGrounded)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
        }
        else if (verticalVelocity < -0.1f && !isGrounded)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

            dashTimeLeft -= Time.fixedDeltaTime;

            if (dashTimeLeft <= 0f)
            {
                isDashing = false;
            }

            return; // estää normaalin movementin dashin aikana
        }

        if (isClimbing)
        {
            transform.position = new Vector3(ladderX, transform.position.y, transform.position.z);

            //rb.gravityScale = 1f;

            rb.linearVelocity = new Vector2(0, verticalMove * climbSpeed);

            // Poistu jos hypätään
            if (jump)
            {
                isClimbing = false;
                //rb.gravityScale = 1f;
            }

            return;
        }
        else
        {
            //rb.gravityScale = 1f;
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
