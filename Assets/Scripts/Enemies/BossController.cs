using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    private Transform player;

    [Header("Boss Attributes")]
    public float speed = 1f;
    public float maxDashSpeed = 10f;
    public float attackInterval = 1f; // Interval between attack ticks
    public float dashAcceleration = 5f;
    public float knockbackForce;

    [Header("Boss behaviour range")]
    public float dashRange;
    public float normalAttackRange;
    public float bossMaxRange;
    public float stopDashRange;

    [Header("Flags to track behaviour")]
    private bool isDashing;
    private bool isDashPointActive; // Flag to track if dashPoint is active
    private bool isAttacking; // Flag to track if currently attacking
    private bool hasAppliedDashDamage; // Flag to track if dash damage has been applied

    [Header("Boss Agro Attributes")]
    public int dashDamage;
    public int attackDamage;

    [Header("Gizmos")]
    public GameObject dashPoint;
    public float dashPointRange;
    public GameObject attackPoint;
    public float attackPointRange;

    [Header("Boss Components")]
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D dashPointCollider; // Reference to the dashPoint (child object) collider
    private Collider2D attackPointCollider; // Reference to the attackPoint (child object) collider


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Find the dashPoint GameObject as a child of this Boss GameObject
        dashPoint = transform.Find("dashPoint")?.gameObject;
        if (dashPoint != null)
        {
            dashPointCollider = dashPoint.GetComponent<Collider2D>();
            if (dashPointCollider != null)
            {
                dashPointCollider.enabled = false; // Disable dashPoint collider initially
            }
            else
            {
                Debug.LogError("Collider2D component not found on dashPoint GameObject.");
            }
        }
        else
        {
            Debug.LogError("dashPoint GameObject not found as a child of this Boss GameObject.");
        }

        // Find the attackPoint GameObject as a child of this Boss GameObject
        attackPoint = transform.Find("attackPoint")?.gameObject;
        if (attackPoint != null)
        {
            attackPointCollider = attackPoint.GetComponent<Collider2D>();
            if (attackPointCollider != null)
            {
                attackPointCollider.enabled = false; // Disable attackPoint collider initially
            }
            else
            {
                Debug.LogError("Collider2D component not found on attackPoint GameObject.");
            }
        }
        else
        {
            Debug.LogError("attackPoint GameObject not found as a child of this Boss GameObject.");
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer <= dashRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
            animator.SetBool("isMoving", true);
            animator.SetBool("closeRange", false);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }

        if (distanceFromPlayer <= normalAttackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = Vector2.zero; // Stop movement when attacking
            animator.SetBool("closeRange", true);
            animator.SetBool("isDashing", false);
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);

            // Enable attackPoint collider when within attack range
            if (!attackPointCollider.enabled)
            {
                attackPointCollider.enabled = true;
                StartCoroutine(DelayBeforeAttackCoroutine()); // Start delay before attack coroutine
            }
        }
        else
        {
            // Disable attackPoint collider when not within attack range
            if (attackPointCollider.enabled)
            {
                attackPointCollider.enabled = false;
                StopAttackCoroutine(); // Stop attacking coroutine
            }
        }

        if (distanceFromPlayer > dashRange && distanceFromPlayer < bossMaxRange)
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
        float dashTimer = 0f; // Timer to track the duration of the dash

        // Deactivate attackPoint collider during dash
        if (attackPointCollider.enabled)
        {
            attackPointCollider.enabled = false;
            StopAttackCoroutine(); // Stop attacking coroutine if dashing
        }

        // Activate dashPoint collider during dash
        dashPointCollider.enabled = true;
        isDashPointActive = true;
        hasAppliedDashDamage = false; // Reset flag for dash damage application

        while (currentSpeed < maxDashSpeed && dashTimer < 5f) // Limit dash duration to 5 seconds
        {
            currentSpeed += dashAcceleration * Time.deltaTime;
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * currentSpeed;
            UpdateAnimator(direction);

            // Check if distance to player is less than stopDashRange while dashing
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer < stopDashRange)
            {
                break;
            }

            dashTimer += Time.deltaTime; // Increment dash timer
            yield return null;
        }

        // Deactivate dashPoint collider after dash
        dashPointCollider.enabled = false;
        isDashPointActive = false;
        hasAppliedDashDamage = false; // Reset flag after dash completes

        isDashing = false; // Set isDashing to false when dash completes

        // a way by increasing the dash timer is eithe making the boss stuck on some other colliding objects
        // or players can dash rapidly to dodge until the timer runs out
        // this ensures that the boss is no longer dashing on above kura haru
        if (dashTimer >= 5f)
        {
            Debug.Log("Dash duration exceeded 5 seconds.");
            animator.SetBool("isDashing", false);
            animator.SetBool("isMoving", true);
        }
    }

    private IEnumerator DelayBeforeAttackCoroutine()
    {
        // Wait for 1 second before starting the attack coroutine
        yield return new WaitForSeconds(1f);
        StartAttackCoroutine();
    }

    private void StartAttackCoroutine()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackCoroutine());
        }
    }

    private void StopAttackCoroutine()
    {
        if (isAttacking)
        {
            isAttacking = false;
            StopCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        while (isAttacking)
        {
            // Deal damage to player using attackDamage
            HealthManager healthManager = player.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.damagePlayer(attackDamage);

                // Calculate knockback direction
                Vector2 knockbackDirection = (player.position - transform.position).normalized;

                // Apply knockback force to the player
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                playerRb.velocity = Vector2.zero;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                Debug.Log("Applied knockback force to player from attack.");

                yield return new WaitForSeconds(attackInterval);
            }
            else
            {
                // Player is null or missing HealthManager component
                yield break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDashPointActive && other.CompareTag("Player") && !hasAppliedDashDamage)
        {
            HealthManager healthManager = other.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                // Deal damage to player
                healthManager.damagePlayer(dashDamage);

                // Calculate knockback direction
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;

                // Apply knockback force to the player
                Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
                playerRb.velocity = Vector2.zero;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                Debug.Log("Applied knockback force to player from dash.");

                hasAppliedDashDamage = true; // Set flag to true once dash damage is applied
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashPointRange);
        Gizmos.DrawWireSphere(transform.position, attackPointRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopDashRange);
    }

    private void UpdateAnimator(Vector2 direction)
    {
        animator.SetBool("isDashing", true);
        animator.SetBool("closeRange", false);
        animator.SetFloat("X2", direction.x);
        animator.SetFloat("Y2", direction.y);
    }
}
