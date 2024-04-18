using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemies : MonoBehaviour
{
    public float speed;
    private Transform player;

    public float attackRange;
    public GameObject shootingRange;
    public GameObject arrow;

    public float lineOfSite;

    private float attackCoolDown;
    public float attackAnimTime;

    private Animator animator;

    public Transform[] patrolPositions;
    private int targetPoint;

    private bool isWaitingAtPatrolPoint = false;
    private bool isShooting = false;

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
            if (distanceFromPlayer < lineOfSite && distanceFromPlayer > attackRange)
            {
                // Move towards the player
                animator.SetBool("inRange", true);
                animator.SetBool("inAttackRange", false);
                animator.SetFloat("walkX", (player.position.x - transform.position.x));
                animator.SetFloat("walkY", (player.position.y - transform.position.y));
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                // Reset shooting flag if player is out of attack range
                isShooting = false;
            }

            attackCoolDown += Time.deltaTime;

            if (distanceFromPlayer < attackRange && attackCoolDown > attackAnimTime && !isShooting)
            {
                // Attack if within attack range and cooldown is ready
                attackCoolDown = 0;

                // bug aries when player is attacking the enemy from near that will make the enemy push back and never stop
                // the push is different from the knockback effect 
                rb.velocity = Vector2.zero; // when player is near the enemy to attack, the enemy wont be moved 
                animator.SetBool("inAttackRange", true);
                animator.SetFloat("lookX", (player.position.x - transform.position.x));
                animator.SetFloat("lookY", (player.position.y - transform.position.y));
                isShooting = true;

                StartCoroutine(WaitForArrowAnimation());
            }

            if (distanceFromPlayer > attackRange && distanceFromPlayer < lineOfSite)
            {
                // Move towards the player but not within attack range
                animator.SetBool("inAttackRange", false);
                animator.SetBool("inRange", true);
                animator.SetFloat("walkX", (player.position.x - transform.position.x));
                animator.SetFloat("walkY", (player.position.y - transform.position.y));
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            if (distanceFromPlayer > lineOfSite)
            {
                // Check if there are patrol positions available
                if (patrolPositions != null && patrolPositions.Length > 0)
                {
                    // Patrol towards patrol points when player is out of sight
                    animator.SetBool("inAttackRange", false);
                    animator.SetBool("inRange", true);

                    Vector2 targetPatrolPoint = patrolPositions[targetPoint].position;
                    animator.SetFloat("walkX", (targetPatrolPoint.x - transform.position.x));
                    animator.SetFloat("walkY", (targetPatrolPoint.y - transform.position.y));
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
                    animator.SetBool("inRange", false);
                }
            }
        }
    }

    IEnumerator WaitForArrowAnimation()
    {
        yield return new WaitForSeconds(attackAnimTime);

        // Check if the player is still within attack range after the animation
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer <= attackRange)
        {
            StartCoroutine(ShootArrow());
        }
        else
        {
            // Player moved out of attack range, reset the shooting state
            isShooting = false;
        }

        yield return new WaitForSeconds(1.4f);
        isShooting = false;
    }

    IEnumerator ShootArrow()
    {
        Instantiate(arrow, shootingRange.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaitingAtPatrolPoint = true;
        animator.SetBool("lookAround", true);

        yield return new WaitForSeconds(2.1f); // Wait for enemy to look around

        // Move to the next patrol point
        IncreaseTargetPointIndex();

        animator.SetBool("lookAround", false);
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
    }
}
