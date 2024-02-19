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

    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<WarriorController>().transform;
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("inAttackRange", true);

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < lineOfSite && distanceFromPlayer>attackRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }

        attackCoolDown += Time.deltaTime;

        if (distanceFromPlayer < attackRange && attackCoolDown > attackAnimTime)
        {
            attackCoolDown = 0;

            animator.SetBool("inAttackRange", true);
            animator.SetFloat("lookX", (player.position.x - this.transform.position.x));
            animator.SetFloat("lookY", (player.position.y - this.transform.position.y));

            Instantiate(arrow, shootingRange.transform.position, Quaternion.identity);
        }


        if (distanceFromPlayer > attackRange)
        {
            animator.SetBool("inAttackRange", false);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
