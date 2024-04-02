using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleEnemyController : MonoBehaviour
{
    public float speed;
    
    public float attackRange;
    public float lineOfSite;

    private float attackCoolDown;
    public float attackAnimTime;

    private Animator animator;

    public Transform homePosition;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<WarriorController>().transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("inRange", true);

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > attackRange)
        {
            animator.SetBool("inRange", true);
            animator.SetFloat("moveX", (player.position.x - this.transform.position.x));
            animator.SetFloat("moveY", (player.position.y - this.transform.position.y));
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }

        attackCoolDown += Time.deltaTime;

        if (distanceFromPlayer < attackRange && attackCoolDown > attackAnimTime)
        {
            attackCoolDown = 0;

            animator.SetBool("isDamaging", true);
            animator.SetFloat("X", (player.position.x - this.transform.position.x));
            animator.SetFloat("Y", (player.position.y - this.transform.position.y));
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }

        if (distanceFromPlayer > attackRange && distanceFromPlayer < lineOfSite)
        {
            animator.SetBool("isDamaging", false);
            animator.SetFloat("moveX", (player.position.x - this.transform.position.x));
            animator.SetFloat("moveY", (player.position.y - this.transform.position.y));
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }

        if (distanceFromPlayer > lineOfSite)
        {
            animator.SetBool("isDamaging", false);
            animator.SetFloat("moveX", (homePosition.position.x - this.transform.position.x));
            animator.SetFloat("moveY", (homePosition.position.y - this.transform.position.y));
            transform.position = Vector2.MoveTowards(this.transform.position, homePosition.position, speed * Time.deltaTime);

            if (Vector2.Distance(this.transform.position, homePosition.position) == 0)
            {
                animator.SetBool("inRange", false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
