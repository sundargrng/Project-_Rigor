using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public int currentHealth;
    public int maxhealth;

    private bool hurtFlash;

    private float flashDuration = 0.1f;
    private float flashCountDown = 0f;

    private SpriteRenderer enemySprite;


    private bool isFlashing;
    // Start is called before the first frame update
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlashing)
        {
            FlashRed();
        }



    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        isFlashing = true;
        flashCountDown = flashDuration;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void FlashRed()
    {
        if (flashCountDown > 0f)
        {
            enemySprite.color = new Color(1f, 0f, 0f, 1f); // Set the sprite color to red
            flashCountDown -= Time.deltaTime;
        }
        else
        {
            enemySprite.color = new Color(1f, 1f, 1f, 1f); // Set the sprite color back to white
            isFlashing = false;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
