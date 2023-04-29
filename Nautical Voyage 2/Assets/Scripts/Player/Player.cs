using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpTime = 0.5f;

    [Header("Turn Check")]
    [SerializeField] private GameObject lLeg;
    [SerializeField] private GameObject rLeg;

    [Header("Ground Check")]
    [SerializeField] private float extraHeight = 0.25f;
    [SerializeField] private LayerMask whatIsGround;

    [HideInInspector] public bool isFacingRight;

    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;
    private float moveInput;

    private bool isJumping;
    private bool isFalling;
    private float jumpTimeCounter;

    private RaycastHit2D groundHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        StartDirectionCheck();
    }

    #region Movement Functions

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        moveInput = UserInput.instance.moveInput.x;

        if (moveInput > 0 || moveInput < 0)
        {
            anim.SetBool("isRunning", true);
            TurnCheck();
        }

        else
        {
            anim.SetBool("isRunning", false);
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        //button was pushed this frame
        if (UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame() && IsGrounded())
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //button is being held
        if (UserInput.instance.controls.Jumping.Jump.IsPressed())
        {
            if (jumpTimeCounter > 0 && isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }

            else
            {
                isJumping = false;
            }
        }

        //button was released this frame
        if (UserInput.instance.controls.Jumping.Jump.WasReleasedThisFrame())
        {
            isJumping = false;
        }
    }

    #endregion

    #region Ground Check

    private bool IsGrounded()
    {
        groundHit = Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, extraHeight, whatIsGround);

        if (groundHit.collider != null)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    #endregion

    #region Turn Checks

    private void StartDirectionCheck()
    {
        if (rLeg.transform.position.x > lLeg.transform.position.x)
        {
            isFacingRight = true;
        }

        else
        {
            isFacingRight = false;
        }
    }

    private void TurnCheck()
    {
        if (UserInput.instance.moveInput.x > 0 && !isFacingRight)
        {
            Turn();
        }

        else if (UserInput.instance.moveInput.x < 0 && isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }

        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }

    #endregion
}
