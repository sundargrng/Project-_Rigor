using UnityEngine;

public class LootCollision : MonoBehaviour
{
    public Loot loot;
    private bool collected = false; // Track if the loot has been collected

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!collected && other.CompareTag("Player")) // Check if not already collected and collides with player
        {
            HealthManager healthManager = other.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.RestoreHealth(loot);
                collected = true; // Mark the loot as collected
                Debug.Log("Health Restored");
                Destroy(gameObject); // Destroy the loot item when collected
            }
        }
    }
}
