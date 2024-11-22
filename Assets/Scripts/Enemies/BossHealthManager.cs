using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHealthManager : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    private float lerpTimer;
    public Image frontHealthBar;
    public Image backHealthBar;
    public Image rageBar;
    private bool rageBarDisabled = false;

    public float chipSpeed = 2f;
    private Animator animator;

    private bool isHealing = false;
    private bool hasStartedHealing = false;
    private bool isDead = false;

    public Image finishScreen;

    public GameObject finishObj;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rageBar.fillAmount = 0f; // Set rageBar fillAmount to 0

        finishScreen = GameObject.Find("FinishScreen").GetComponent<Image>();
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update rageBar fillAmount
        float rageFillAmount = Mathf.Clamp01((maxHealth - currentHealth) / (maxHealth * 0.8f));
        rageBar.fillAmount = rageFillAmount;

        // When fighting the boss, if the current health is 20% of the max health, the boss will start healing
        float healthPercentage = (float)currentHealth / maxHealth;
        if (healthPercentage <= 0.2f && !hasStartedHealing)
        {
            StartHealing();
            hasStartedHealing = true; // Ensure the healing logic is only applied once
        }

        UpdateHealthUI(); // Ensure UI is updated every frame
    }

    public void UpdateHealthUI()
    {
        Debug.Log(currentHealth);

        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = (float)currentHealth / maxHealth;

        if (isHealing)
        {
            // When healing
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
            if (backHealthBar.fillAmount >= hFraction)
            {
                frontHealthBar.fillAmount = hFraction;
                lerpTimer = 0f;
            }
        }
        else
        {
            // When taking damage
            if (fillB > hFraction)
            {
                frontHealthBar.fillAmount = hFraction;
                backHealthBar.color = Color.red;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
            }
            else
            {
                frontHealthBar.fillAmount = hFraction;
                lerpTimer = 0f;
                backHealthBar.fillAmount = hFraction;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isHealing || rageBarDisabled) return; // Ignore damage while healing or dead

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Color originalColor = spriteRenderer.color;

        // Change the color to a shade of green
        spriteRenderer.color = new Color(0, 1, 0, 1);

        StartCoroutine(FlashGreen(spriteRenderer, originalColor));

        currentHealth -= damage;
        lerpTimer = 0f;

        if (currentHealth <= 0)
        {
            Die();
        }

        isHealing = false; // Ensure healing flag is reset
        UpdateHealthUI(); // Update UI after taking damage
    }

    private IEnumerator FlashGreen(SpriteRenderer spriteRenderer, Color originalColor)
    {
        yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds

        // Change the color to a shade of green
        Color flashColor = new Color(0, 1, 0, 1);
        spriteRenderer.color = flashColor;

        yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds

        spriteRenderer.color = originalColor; // Change the color back to the original
    }

    private void Die()
    {
        SoundManager.StopMusic();
        finishObj.SetActive(true);
        finishScreen.color = new Color(0, 0, 0, 1f);
        LeanTween.alpha(finishScreen.rectTransform, 1f, 1f);

        SoundManager.PlaySound(SoundType.FINISH);
        StartCoroutine(NowFinishScreen());

        isDead = true;
        isHealing = false;
        gameObject.SetActive(false);
        // Notify BossController that the boss is dead
        BossController bossController = GetComponent<BossController>();
        if (bossController != null)
        {
            bossController.OnBossDeath();
        }
    }

    private IEnumerator NowFinishScreen()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Main Menu");
    }


    private void StartHealing()
    {
        rageBarDisabled = true;
        isHealing = true;
        lerpTimer = 0f;
        animator.SetTrigger("Healing");
        animator.SetBool("isHealing", true); // healing animation
        // Notify BossController that the boss is healing
        BossController bossController = GetComponent<BossController>();
        if (bossController != null)
        {
            bossController.OnBossHealing();
        }

        // Calculate amount to heal per second (10% of max health)
        float healRate = 0.2f * maxHealth; // Healing rate per second
        int amountToHealPerSecond = Mathf.RoundToInt(healRate);

        StartCoroutine(HealingRoutine(amountToHealPerSecond));
    }

    private IEnumerator HealingRoutine(int amountToHealPerSecond)
    {
        while (currentHealth < maxHealth)
        {
            Heal(amountToHealPerSecond);

            yield return new WaitForSeconds(2f); // Wait for 3 seconds before applying the next heal
        }

        // Healing completed
        if (currentHealth == maxHealth)
        {
            rageBarDisabled  = false;
            isHealing = false;
            animator.SetBool("isHealing", false); // Stop the healing animation

            Color rageBarColor = rageBar.color;
            rageBarColor.a = 0f;
            rageBar.color = rageBarColor;
        }
        // Notify BossController that the boss has finished healing
        BossController bossController = GetComponent<BossController>();
        if (bossController != null)
        {
            bossController.OnHealingComplete();
        }
    }

    public void Heal(int amountToHeal)
    {
        if (amountToHeal <= 0)
        {
            return;
        }

        // Cap the health at max health after healing
        int spaceInHealth = maxHealth - currentHealth;
        int actualHealAmount = Mathf.Min(amountToHeal, spaceInHealth);

        // Apply healing to the boss
        currentHealth += actualHealAmount;

        UpdateHealthUI(); // Update UI after healing
    }
}
