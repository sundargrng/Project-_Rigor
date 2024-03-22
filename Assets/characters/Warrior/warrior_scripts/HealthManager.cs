using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    private bool hurtFlash;

    [SerializeField]
    private float flashTime = 0f;
    private float flashCountDown = 0f;

    private SpriteRenderer playerSprite;

    private Animator playerAnim;
    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();

        GameObject animator = GameObject.FindGameObjectWithTag("Player");
        if (animator != null)
        {
            playerAnim = animator.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hurtFlash)
        {
            if (flashCountDown> flashTime * .99f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else if (flashCountDown > flashTime * .75f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }
            else if (flashCountDown > flashTime * .50f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else if (flashCountDown > flashTime * .25f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            }
            else if (flashCountDown >  0f)
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            }
            else
            {
                playerSprite.color = new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
                hurtFlash = false;
            }
            flashCountDown -= Time.deltaTime;
        }
    }


    public void damagePlayer(int damageTaken)
    {
        currentHealth -= damageTaken;
        hurtFlash = true;
        flashCountDown = flashTime;

        if (currentHealth <= 0)
        {
            playerAnim.SetTrigger("death");
        }
    }

    public void RestoreHealth(Loot loot)
    {
        if (loot == null)
        {
            return;
        }

        // Check if current health is less than maximum health
        if (currentHealth < maxHealth)
        {
            // Calculate amount of healing
            int amountToHeal = Mathf.Min(loot.healthRestoreAmount, maxHealth - currentHealth);

            // Apply healing
            currentHealth += amountToHeal;
        }
    }
}
