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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        targetPoint = 0;
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
                animator.SetFloat("walkX", (player.position.x - transform.position.x));
                animator.SetFloat("walkY", (player.position.y - transform.position.y));
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            attackCoolDown += Time.deltaTime;

            if (distanceFromPlayer < attackRange && attackCoolDown > attackAnimTime)
            {
                // Attack if within attack range and cooldown is ready
                attackCoolDown = 0;
                animator.SetBool("inAttackRange", true);
                animator.SetFloat("lookX", (player.position.x - transform.position.x));
                animator.SetFloat("lookY", (player.position.y - transform.position.y));

                Instantiate(arrow, shootingRange.transform.position, Quaternion.identity);
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
        }
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
