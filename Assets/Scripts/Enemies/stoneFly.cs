using UnityEngine;

public class stoneFly : MonoBehaviour
{
    public float speed;
    public int stoneDamage;

    private Rigidbody2D stoneRB;

    void Start()
    {
        stoneRB = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f); // Destroy stone after 5 seconds if not collided
    }

    void FixedUpdate()
    {
        // Move the stone forward
        stoneRB.velocity = transform.right * speed;
    }

    public void SetDirection(Vector2 direction)
    {
        // Set the direction of the stone
        transform.right = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthManager playerHealth = other.GetComponent<HealthManager>();

            if (playerHealth != null)
            {
                SoundManager.PlaySound(SoundType.STONEHURT);
                playerHealth.damagePlayer(stoneDamage);
            }

            // Destroy the stone on collision
            Destroy(gameObject);
        }
    }
}
