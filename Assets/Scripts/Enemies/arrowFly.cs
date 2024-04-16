using System.Collections;
using UnityEngine;

public class arrowFly : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D arrowRB;
    public int arrowDamage;

    public float destroyTime; // Time in seconds before the arrow is destroyed

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        arrowRB = GetComponent<Rigidbody2D>();
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        arrowRB.velocity = new Vector2(moveDir.x, moveDir.y);

        float rotate = Mathf.Atan2(-moveDir.y, -moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotate + 45);

        // Destroy the arrow after a specified time
        Destroy(this.gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthManager playerHealth = other.GetComponent<HealthManager>();

            if (playerHealth != null)
            {
                playerHealth.damagePlayer(arrowDamage);
            }
        }
    }
}
