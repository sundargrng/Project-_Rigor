using UnityEngine;

public class MeeleEnemyController : MonoBehaviour
{
    public float speed = 5f;
    public float attackRange = 1.5f;
    public float lineOfSight = 10f;
    [SerializeField] private float attackTime;
    public int enemyDamage = 10;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private float attackAnimationTimer = 0f; // Timer to track the attack animation time

    public Transform homePosition;

    public LayerMask players;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null)
            return;

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > attackRange)
        {
            // Player is within line of sight but outside attack range, move towards the player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
            animator.SetBool("inRange", true);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }

        if (distanceFromPlayer <= attackRange)
        {
            animator.SetBool("isDamaging", true);
            rb.velocity = Vector2.zero;
            animator.SetFloat("X", (player.position.x - transform.position.x));
            animator.SetFloat("Y", (player.position.y - transform.position.y));
            Attack();
        }

        if (distanceFromPlayer > attackRange && distanceFromPlayer < lineOfSight)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * speed;
            animator.SetBool("isDamaging", false);
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }

        if (distanceFromPlayer > lineOfSight)
        {
            // Player is out of sight, return to home position and patrol
            Vector2 directionToHome = (homePosition.position - transform.position).normalized;
            rb.velocity = directionToHome * speed;

            animator.SetBool("isDamaging", false); // Ensure isDamaging is false when out of attack range
            animator.SetFloat("moveX", directionToHome.x);
            animator.SetFloat("moveY", directionToHome.y);

            // If enemy reaches home position, reset its position and stop patrolling
            if (Vector2.Distance(transform.position, homePosition.position) < 1f)
            {
                animator.SetBool("inRange", false);
                transform.position = homePosition.position;
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void Attack()
    {
        // Increment the attack animation timer
        attackAnimationTimer += Time.deltaTime;

        // Check if the attack animation time has reached 1 second
        if (attackAnimationTimer >= attackTime)
        {
            attackAnimationTimer = 0; // Reset the timer
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, players);
            foreach (Collider2D hit in hits)
            {
                hit.GetComponent<HealthManager>().damagePlayer(enemyDamage);
            }
        }
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }
}
