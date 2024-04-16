using UnityEngine;
using System.Collections;

public class MeeleEnemyController : MonoBehaviour
{
    public float speed = 5f;
    public float attackRange;
    public float lineOfSight;
    [SerializeField] private float attackTime;
    public int enemyDamage = 10;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private float attackAnimationTimer = 0f; // Timer to track the attack animation time

    public Transform homePosition;

    public LayerMask players;

    [SerializeField] private Transform[] patrolPositions;
    private int currentPatrolPoint = 0; // Index of the current patrol point


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
            rb.velocity = Vector2.zero;
            animator.SetBool("isDamaging", true);
            animator.SetFloat("X", (player.position.x - transform.position.x));
            animator.SetFloat("Y", (player.position.y - transform.position.y));

            StartCoroutine(Attack()); // Start the attack coroutine
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
            // Player is out of sight, patrol between points
            Vector2 targetPatrolPoint = patrolPositions[currentPatrolPoint].position;
            Vector2 directionToPatrolPoint = (targetPatrolPoint - (Vector2)transform.position).normalized;
            rb.velocity = directionToPatrolPoint * speed;

            animator.SetBool("inRange", true);
            animator.SetFloat("moveX", directionToPatrolPoint.x);
            animator.SetFloat("moveY", directionToPatrolPoint.y);

            // Check if reached the current patrol point
            if (Vector2.Distance(transform.position, targetPatrolPoint) < 0.1f)
            {
                // Move to the next patrol point
                IncreasePatrolPointIndex();
            }
        }
    }

    private void IncreasePatrolPointIndex()
    {
        currentPatrolPoint++;
        if (currentPatrolPoint >= patrolPositions.Length)
        {
            currentPatrolPoint = 0; // Wrap around to the first patrol point if index exceeds array length
        }
    }

    private IEnumerator Attack()
    {
        // Increment the attack animation timer
        attackAnimationTimer = 0f;

        // Wait until the attack animation time (attackTime) is reached
        while (attackAnimationTimer < attackTime)
        {
            attackAnimationTimer += Time.deltaTime;
            yield return null;
        }

        // After attackTime is reached, damage the player if they are within attack range
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer <= attackRange)
        {
            // Damage the player
            player.GetComponent<HealthManager>().damagePlayer(enemyDamage);
        }

        // Reset attack animation variables
        animator.SetBool("isDamaging", false);
        animator.SetFloat("X", 0f);
        animator.SetFloat("Y", 0f);
    }
}
