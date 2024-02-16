using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{

    public Transform player;

    public GameObject arrow;

    private float shotCooldown;

    [SerializeField]
    private float startToShootIn;

    // Start is called before the first frame update
    void Start()
    {
        shotCooldown = startToShootIn;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);

        transform.up = direction;

        if (shotCooldown<= 0)
        {
            Instantiate(arrow, transform.position, transform.rotation);
            shotCooldown = startToShootIn;
        }else
        {
            shotCooldown -= Time.deltaTime;
        }
    }
}
