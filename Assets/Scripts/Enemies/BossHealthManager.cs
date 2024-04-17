using System.Collections;
using UnityEngine;

public class BossHealthManager : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    private Animator animator;
    private Rigidbody2D rb2d;

    private bool isHealing = false;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // when fighting the boss if the current health is 40% of the max health than the boss will start healing
        float healthPercentage = (float)currentHealth / maxHealth;
        if (healthPercentage <= 0.4f && !isHealing)
        {
            StartHealing();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //animator.SetTrigger("isDead");
        rb2d.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void StartHealing()
    {
        isHealing = true;
        animator.SetTrigger("Healing"); // healing animation
        rb2d.velocity = Vector2.zero; // Stop boss movement during healing animation

        // Calculate amount to heal per second (10% of max health)
        float healRate = 0.1f * maxHealth; // Healing rate per second
        int amountToHealPerSecond = Mathf.RoundToInt(healRate);

        StartCoroutine(HealingRoutine(amountToHealPerSecond));
    }

    private IEnumerator HealingRoutine(int amountToHealPerSecond)
    {
        while (isHealing && currentHealth < maxHealth)
        {
            Heal(amountToHealPerSecond);

            yield return new WaitForSeconds(1.0f); // Wait for 1 second before applying the next heal
        }

        // Healing completed
        isHealing = false;
    }

    public void Heal(int amountToHeal)
    {
        if (amountToHeal <= 0)
        {
            return;
        }

        // cap the health at max health after healing
        int spaceInHealth = maxHealth - currentHealth;
        int actualHealAmount = Mathf.Min(amountToHeal, spaceInHealth);

        // Apply healing to the boss
        currentHealth += actualHealAmount;
    }
}
