using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private GameObject lLeg;
    [SerializeField] private GameObject rLeg;

    [HideInInspector] public bool isFacingRight;

    private Rigidbody2D rb;
    private float moveInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartDirectionCheck();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        moveInput = UserInput.instance.moveInput.x;

        if (moveInput > 0 || moveInput < 0)
        {
            TurnCheck();
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

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
}
