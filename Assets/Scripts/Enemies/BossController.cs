using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public float speed = 1f;
    public float closeRange;
    public float closeAttackRange;
    public float bossMaxRange;

    public float dashAcceleration = 5f; // Acceleration factor for dashing
    public float maxDashSpeed = 10f;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    private bool isDashing;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Ignore collisions with the "Player" layer
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));
    }

    private void Update()
    {
        if (player == null)
            return;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer <= closeRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
            animator.SetBool("isMoving", true);
            animator.SetBool("closeRange", false);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }

        if (distanceFromPlayer <= closeAttackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
            animator.SetBool("isMoving", false);
            animator.SetBool("closeRange", true);
            animator.SetBool("isDashing", false);
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
        }

        if (distanceFromPlayer > closeRange && distanceFromPlayer < bossMaxRange)
        {
            DashTowardsPlayer();
        }
    }

    private void DashTowardsPlayer()
    {
        if (!isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        float currentSpeed = speed;

        while (currentSpeed < maxDashSpeed)
        {
            currentSpeed += dashAcceleration * Time.deltaTime;
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * currentSpeed;
            UpdateAnimator(direction);
            yield return null;
        }

        isDashing = false;
    }

    private void UpdateAnimator(Vector2 direction)
    {
        animator.SetBool("isDashing", true);
        animator.SetBool("closeRange", false);
        animator.SetFloat("X2", direction.x);
        animator.SetFloat("Y2", direction.y);
    }
}
