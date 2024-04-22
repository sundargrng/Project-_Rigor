using UnityEngine;

public class LootCollision : MonoBehaviour
{
    public Loot loot;
    private bool collected = false; // Track if the loot has been collected

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || loot == null || !other.CompareTag("Player"))
        {
            return; // Exit early if already collected or invalid collision
        }

        HealthManager healthManager = other.GetComponent<HealthManager>();
        if (healthManager != null)
        {
            if (loot.healthRestoreAmount > 0)
            {
                healthManager.RestoreHealth(loot);
                Debug.Log("Health Restored: " + loot.healthRestoreAmount);
            }

            if (loot.healthMinusAmount > 0)
            {
                healthManager.ReduceHealth(loot.healthMinusAmount);
                Debug.Log("Health Reduced: " + loot.healthMinusAmount);
            }
        }

        // Check if ExperienceManager.Instance is not null and loot.expGainAmount is positive
        if (ExperienceManager.Instance != null && loot.expGainAmount > 0 && loot != null)
        {
            ExperienceManager.Instance.AddExperience(loot.expGainAmount);
            Debug.Log("Experience Gained: " + loot.expGainAmount);
        }

        collected = true; // Mark the loot as collected
        Destroy(gameObject); // Destroy the loot item when collected
    }
}
