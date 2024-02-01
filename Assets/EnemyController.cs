using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private Animator animator; // reference to animator for firstEnemy

    private Transform target; // the enemy follows the target if the target is in range. Target is player

    [SerializeField] // Will allow us to change the speed anytime in the unity
    private float speed; // movement speed for enemy

    [SerializeField]
    private float range;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // the enemy will look for the object with WarriorController script attached in its component.
        target = FindAnyObjectByType<WarriorController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer();
    }

    public void followPlayer()
    {
        animator.SetBool("inRange", true);

        // when enemy follows the player, sets the animation to the direction of player
        animator.SetFloat("moveX", (target.transform.position.x - transform.position.x));
        animator.SetFloat("moveY", (target.transform.position.y - transform.position.y));

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
