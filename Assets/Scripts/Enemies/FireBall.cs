using System.Collections;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D fireRb;
    public int fireDamage;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isFadingIn = true; // Flag to control fading in/out

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        fireRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set initial color to transparent (alpha = 0)
        originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        fireRb.velocity = new Vector2(moveDir.x, moveDir.y);

        float rotate = Mathf.Atan2(-moveDir.y, -moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotate + 170);
        Destroy(this.gameObject, 4);
    }

    // Update is called once per frame
    void Update()
    {
        // Gradually increase alpha value as the projectile moves towards the target
        if (isFadingIn && spriteRenderer.color.a < 1f)
        {
            float newAlpha = spriteRenderer.color.a + Time.deltaTime; // Increase alpha gradually
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthManager playerHealth = other.GetComponent<HealthManager>();

            if (playerHealth != null)
            {
                SoundManager.PlaySound(SoundType.FIREHURT);
                playerHealth.damagePlayer(fireDamage);
            }
        }
    }
}
