using UnityEngine;

public class WaveSpawnTrigger : MonoBehaviour
{
    public RoundBasedWaveSpawner waveSpawner; // Reference to the RoundBasedWaveSpawner

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // If the player enters the trigger zone, activate the wave spawner
            if (waveSpawner != null)
            {
                waveSpawner.ActivateWaveSpawner();
            }

            // Disable this trigger object once activated (optional)
            gameObject.SetActive(false);
        }
    }
}
