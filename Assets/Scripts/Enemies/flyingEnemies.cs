using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingEnemies : MonoBehaviour
{
    public float speed;
    private float projectileCooldown;
    public float projectileAnimTime;
    private float wingsCooldown;
    public float wingsAnimTime;

    public GameObject throwFrom;
    public GameObject projectile;
    public GameObject wavePoint;

    public float lineOfSite;
    public float attackRange;
    public float aggroRange;
    public float waveForceRange;

    private Animator animator;

    private Transform player;

    public Transform[] patrolPositions;
    private int targetPoint;

    private bool isWaitingAtPatrolPoint = false;

    public float knockbackForce; // Define knockback force here

    public LayerMask players;

    private Rigidbody2D rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        targetPoint = 0;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (!isWaitingAtPatrolPoint)
        {
            if (distanceFromPlayer < lineOfSite && distanceFromPlayer > aggroRange)
            {
                // Move towards the player
                animator.SetBool("isMoving", true);
                animator.SetFloat("mX", (player.position.x - transform.position.x));
                animator.SetFloat("mY", (player.position.y - transform.position.y));
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            projectileCooldown += Time.deltaTime;

            if (distanceFromPlayer < aggroRange && projectileCooldown > projectileAnimTime && distanceFromPlayer > attackRange)
            {
                // Attack if within attack range and cooldown is ready
                projectileCooldown = 0;
                animator.SetBool("isMoving", true);
                animator.SetBool("inCloseRange", false);
                animator.SetFloat("mX", (player.position.x - transform.position.x));
                animator.SetFloat("mY", (player.position.y - transform.position.y));

                SoundManager.PlaySound(SoundType.INSTANTIATEFIREBALL);
                Instantiate(projectile, throwFrom.transform.position, Quaternion.identity);
            }

            wingsCooldown += Time.deltaTime;

            if (distanceFromPlayer < attackRange && wingsCooldown > wingsAnimTime)
            {
                wingsCooldown = 0;
                rb.velocity = Vector2.zero;
                SoundManager.PlaySound(SoundType.DRAGONDEF);
                // Call WaveForce() when player is within attack range
                WaveForce();
                // Move towards the player but not within attack range
                animator.SetBool("inCloseRange", true);
                animator.SetFloat("caX", (player.position.x - transform.position.x));
                animator.SetFloat("caY", (player.position.y - transform.position.y));
            }

            if (distanceFromPlayer > lineOfSite)
            {
                // Check if there are patrol positions available
                if (patrolPositions != null && patrolPositions.Length > 0)
                {
                    // Patrol towards patrol points when player is out of sight
                    animator.SetBool("inCloseRange", false);
                    animator.SetBool("isMoving", true);

                    Vector2 targetPatrolPoint = patrolPositions[targetPoint].position;
                    animator.SetFloat("mX", (targetPatrolPoint.x - transform.position.x));
                    animator.SetFloat("mY", (targetPatrolPoint.y - transform.position.y));
                    transform.position = Vector2.MoveTowards(transform.position, targetPatrolPoint, speed * Time.deltaTime);

                    // Check if reached the current patrol point
                    if (Vector2.Distance(transform.position, targetPatrolPoint) < 0.1f)
                    {
                        // Start waiting at patrol point
                        StartCoroutine(WaitAtPatrolPoint());
                    }
                }
                else
                {
                    // No patrol points available, set inRange to false
                    animator.SetBool("isMoving", false);
                }
            }
        }
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaitingAtPatrolPoint = true;
        animator.SetBool("isMoving", false);

        yield return new WaitForSeconds(2.1f); // Wait for enemy to look around

        // Move to the next patrol point
        IncreaseTargetPointIndex();

        animator.SetBool("isMoving", true);
        isWaitingAtPatrolPoint = false; // Reset flag to allow movement
    }

    private void IncreaseTargetPointIndex()
    {
        targetPoint++;
        if (targetPoint >= patrolPositions.Length)
        {
            targetPoint = 0; // Wrap around to the first patrol point if index exceeds array length
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, aggroRange);
        Gizmos.DrawWireSphere(transform.position, waveForceRange);
    }

    private void WaveForce()
    {
        // Detect colliders within attackRange
        Collider2D[] player = Physics2D.OverlapCircleAll(wavePoint.transform.position, waveForceRange, players);

        foreach (Collider2D players in player)
        {
            Debug.Log("Player pushed");
        }
    }
}
