using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    public Animator animator;
    public Rigidbody2D rb;

    void Update()
    {
        PauseMenu pauseMenu = FindFirstObjectByType<PauseMenu>();
        if (pauseMenu != null && pauseMenu.GameIsPaused) return;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        UpdateVerticalAnimations();
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
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
