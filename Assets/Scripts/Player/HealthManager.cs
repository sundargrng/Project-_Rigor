using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDataPersistence
{
    public int currentHealth = 10;
    public int maxHealth = 10;

    private bool hurtFlash;


    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.5f);

    private SpriteRenderer playerSprite;
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    private Animator playerAnim;

    // Start is called before the first frame update
    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component

        GameObject animator = GameObject.FindGameObjectWithTag("Player");
        if (animator != null)
        {
            playerAnim = animator.GetComponent<Animator>();
        }
    }


    public void damagePlayer(int damageTaken)
    {
        currentHealth -= damageTaken;
        if (currentHealth <= 0)
        {
            playerAnim.SetTrigger("death");
            FindObjectOfType<UIManager>()?.IncrementDeathCount(); // Increment death count
        }

        StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        // Flash the sprite with the specified color for the duration
        playerSprite.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        playerSprite.color = Color.white; // Reset to original color
    }

    public void LoadData(GameData data)
    {
        // Existing loading logic
        this.currentHealth = data.playerHealth;
        this.maxHealth = data.playerHealthMax;
    }

    public void SaveData(GameData data)
    {
        // Existing saving logic
        data.playerHealth = this.currentHealth;
        data.playerHealthMax = this.maxHealth;
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

    public void ReduceHealth(int amount)
    {
        currentHealth -= amount;
    }
}
