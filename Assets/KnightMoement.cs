using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;


public class KnightMoement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    private Animator animator;

    private Vector2 input;

    private Vector2 lastMoveDirection;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame - used for Inputs and Timers
    void Update()
    {
        ProcessInputs();
        Animate();
    }


    // Called once per Physics frame used for physics
    private void FixedUpdate()
    {
        rb.velocity = input * moveSpeed;
    }


    void ProcessInputs()
    {
        // Store last move direction when we stop moving
        float moveX = UnityEngine.Input.GetAxisRaw("Horizontal");
        float moveY = UnityEngine.Input.GetAxisRaw("Vertical");

        if ((moveX == 0 && moveY == 0) && (input.x != 0 || input.y != 0))
        {
            lastMoveDirection = input;
        }

        input.x = UnityEngine.Input.GetAxisRaw("Horizontal");
        input.y = UnityEngine.Input.GetAxisRaw("Vertical");
    }



    void Animate()
    {
        // set our animator parameters
        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);
        animator.SetFloat("MoveMagnitude", input.magnitude);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }
}
