using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnemyController : MonoBehaviour
{
    private Animator animator; // reference to animator for firstEnemy

    private Transform target; // the enemy follows the target if the target is in range. Target is player

    public Transform homePos;

    [SerializeField] // Will allow us to change the speed anytime in the unity
    private float speed; // movement speed for enemy

    [SerializeField]
    private float maxRange;

    [SerializeField]
    private float minRange;

    private Rigidbody2D eRb;

    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // the enemy will look for the object with WarriorController script attached in its component.
        target = FindObjectOfType<WarriorController>().transform;

        eRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) > minRange)
        {
            followPlayer();
            noAttackPlayer();
        }
        else if (Vector3.Distance(target.position, transform.position) <= minRange)
        {
            attackPlayer();
        }
        else if(Vector3.Distance(target.position, transform.position)> maxRange)
        {
            GoHome();
        }
    }
       
    public void followPlayer()
    {
        if(EnemyHealthManager.deadFr == true)
        {
            return;
        }

        animator.SetBool("inRange", true);

        // when enemy follows the player, sets the animation to the direction of player
        animator.SetFloat("moveX", (target.position.x - transform.position.x));
        animator.SetFloat("moveY", (target.position.y - transform.position.y));

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public void attackPlayer() 
    {
        animator.SetBool("isDamaging", true);

        animator.SetFloat("X", (target.position.x - transform.position.x));
        animator.SetFloat("Y", (target.position.y - transform.position.y));

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public void noAttackPlayer()
    {
        animator.SetBool("isDamaging", false);

        animator.SetFloat("moveX", (target.position.x - transform.position.x));
        animator.SetFloat("moveY", (target.position.y - transform.position.y));
    }

    public void GoHome()
    {
        animator.SetFloat("moveX", (homePos.position.x - transform.position.x));
        animator.SetFloat("moveY", (homePos.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, homePos.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, homePos.position) == 0)
        {
            animator.SetBool("inRange", false);
        }
    }
}
