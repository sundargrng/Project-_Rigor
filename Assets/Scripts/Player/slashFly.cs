using Unity.VisualScripting;
using UnityEngine;

public class slashFly : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D slashRB;
    public float speed;
    public int damage = 10;

    //public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        slashRB = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        slashRB.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 180);
        Destroy(this.gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Get the EnemyHealth component from the enemy GameObject
            EnemyHealthManager enemyHealth = collision.GetComponent<EnemyHealthManager>();

            
            // If the enemy has an EnemyHealth component, apply damage to it
            if (enemyHealth != null)
            {
                enemyHealth.TakingSwordWaves(damage);
                /*GameObject effect = Instantiate(hitEffect, this.transform.position, Quaternion.identity);
                Destroy(effect, 0.5f);*/
            }

            // Destroy the sword slash GameObject after it hits an enemy
            Destroy(gameObject);
        }

        if (collision.CompareTag("Boss"))
        {
            BossHealthManager bossHealth = collision.GetComponent<BossHealthManager>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the slashFly GameObject after hitting the boss
        }
    }
}