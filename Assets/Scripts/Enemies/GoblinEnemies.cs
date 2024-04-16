using System.Collections;
using UnityEngine;

public class GoblinEnemies : MonoBehaviour
{
    public float speed;
    private float stoneCooldown;
    public float stoneAnimTime;

    public GameObject slingShotRange;

    public float lineOfSite;
    public float attackRange;

    public GameObject interactPoint;
    public float interactRadius;
    public float interactRange;

    private Animator animator;

    private Transform player;

    public Transform[] patrolPositions;
    private int targetPoint;

    private bool isWaitingAtPatrolPoint = false;
    private bool isShooting = false; // Flag to control stone shooting

    public LayerMask players;

    public GameObject stonePrefab;
    public int numberOfStones;
    public float stoneSpreadAngle;

    private Rigidbody2D gRb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        targetPoint = 0;
        gRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (!isWaitingAtPatrolPoint)
        {
            if (distanceFromPlayer < lineOfSite && distanceFromPlayer > attackRange)
            {
                // Move towards the player
                animator.SetBool("isRunning", true);
                animator.SetBool("isShooting", false);
                animator.SetFloat("runX", (player.position.x - transform.position.x));
                animator.SetFloat("runY", (player.position.y - transform.position.y));
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

                // Reset shooting flag if player is out of attack range
                isShooting = false;
            }

            stoneCooldown += Time.deltaTime;

            if (distanceFromPlayer < attackRange && stoneCooldown > stoneAnimTime && !isShooting)
            {
                // this is a reset for the whole attack that includes waiting for animation and 
                // instantiating the stones
                stoneCooldown = 0;

                gRb.velocity = Vector2.zero;

                animator.SetBool("isShooting", true);
                animator.SetFloat("shootX", (player.position.x - transform.position.x));
                animator.SetFloat("shootY", (player.position.y - transform.position.y));
                isShooting = true; // Set isShooting flag to true to prevent stone spawning

                // Start the stone shooting coroutine after stoneAnimTime seconds
                StartCoroutine(WaitForAttackAnimation());
            }

            if (distanceFromPlayer > lineOfSite)
            {
                // Check if there are patrol positions available
                if (patrolPositions != null && patrolPositions.Length > 0)
                {
                    // Patrol towards patrol points when player is out of sight
                    animator.SetBool("isShooting", false);
                    animator.SetBool("isRunning", true);

                    Vector2 targetPatrolPoint = patrolPositions[targetPoint].position;
                    animator.SetFloat("runX", (targetPatrolPoint.x - transform.position.x));
                    animator.SetFloat("runY", (targetPatrolPoint.y - transform.position.y));
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
                    // No patrol points available, set inRunning to false
                    animator.SetBool("isRunning", false);
                }
            }
        }
    }

    IEnumerator WaitForAttackAnimation()
    {
        yield return new WaitForSeconds(stoneAnimTime); // Wait for the attack animation duration

        // Start shooting stones after the animation
        StartCoroutine(ShootStones());

        // a short reloading delay for the enemy
        yield return new WaitForSeconds(0.5f);
        isShooting = false;
    }

    IEnumerator ShootStones()
    {
        Vector2 attackDirection = (player.position - transform.position).normalized;

        for (int i = 0; i < numberOfStones; i++)
        {
            // Calculate spread angle for each stone
            float spreadAngle = Random.Range(-stoneSpreadAngle, stoneSpreadAngle);
            Vector2 stoneDirection = Quaternion.Euler(0, 0, spreadAngle) * attackDirection;

            // Instantiate the stone with calculated direction
            GameObject newStone = Instantiate(stonePrefab, slingShotRange.transform.position, Quaternion.identity);
            newStone.GetComponent<stoneFly>().SetDirection(stoneDirection);

            yield return new WaitForSeconds(0.1f); // Adjust delay between stone spawns
        }
    }

    IEnumerator WaitAtPatrolPoint()
    {
        isWaitingAtPatrolPoint = true;
        animator.SetBool("isRunning", false);

        yield return new WaitForSeconds(2.1f); // Wait at a patrol point

        // Move to the next patrol point
        IncreaseTargetPointIndex();

        animator.SetBool("isRunning", true);
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
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }

    private void InteractIcon()
    {
        // Detect colliders within attackRange
        Collider2D[] player = Physics2D.OverlapCircleAll(interactPoint.transform.position, interactRadius, players);

        foreach (Collider2D players in player)
        {
            Debug.Log("Go away from the chest");
        }
    }
}
