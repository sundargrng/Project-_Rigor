using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WarriorController : MonoBehaviour
{
    private Rigidbody2D rb;

    private Animator animator;

    [SerializeField]
    private float speed;

    private float attackTime = 0.25f;
    private float attackCountDOwn = 0.25f;
    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (DialogManager.isActive == true)
        {
            return;
        }

        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed * Time.fixedDeltaTime;

        animator.SetFloat("moveX", rb.velocity.x);
        animator.SetFloat("moveY", rb.velocity.y);

        if(Input.GetAxisRaw("Horizontal") ==1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }

        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            attackCountDOwn -= Time.deltaTime;
            if (attackCountDOwn <= 0)
            {
                animator.SetBool("isAttacking", false);
                isAttacking = false;
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) 
        {
            attackCountDOwn = attackTime;
            animator.SetBool("isAttacking", true);
            isAttacking = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerKB")
        {
            Vector2 difference = transform.position - other.transform.position;
            transform.position = new Vector2(transform.position.x + difference.x, transform.position.y + difference.y);
        }
    }

}
