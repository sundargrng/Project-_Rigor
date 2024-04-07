using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemyData
{
    public GameObject enemyPrefab;
    public int cost;
}

public class RoundBasedWaveSpawner : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public PortalManager portalManager; // Reference to the PortalManager

    public UnityEvent<int> OnRoundStart = new UnityEvent<int>();

    public UnityEvent OnAllRoundsCompleted = new UnityEvent(); // Event to signal all rounds completed

    public List<EnemyData> enemies = new List<EnemyData>();
    public int[] roundValues; // Total values for each of the 3 rounds
    public float spawnInterval = 2f; // Interval between enemy spawns
    public float delayBetweenRounds = 2f; // Delay between rounds

    private int currentRound = 0;
    private int currentWaveValue;
    private bool isSpawning;
    private SpriteRenderer portalRenderer;
    private bool triggerActivated = false; // Flag to track if trigger has been activated

    public Transform spawnLocation; // Location to spawn enemies

    private bool completed = false;

    void Start()
    {
        // Get the SpriteRenderer component from the spawnLocation object
        portalRenderer = spawnLocation.GetComponent<SpriteRenderer>();
    }

    // Method to start the wave spawner when triggered
    public void ActivateWaveSpawner()
    {
        if (!triggerActivated)
        {
            triggerActivated = true;
            StartRound();
        }
    }

    void StartRound()
    {
        if (currentRound < roundValues.Length)
        {
            // Raise event to notify round start (pass current round number)
            OnRoundStart.Invoke(currentRound + 1);

            currentWaveValue = roundValues[currentRound];
            StartCoroutine(SpawnEnemiesInWave());
            currentRound++;
        }
        else
        {
            Debug.Log("All rounds completed.");
            OnAllRoundsCompleted.Invoke(); // Signal that all rounds are completed
            completed = true;
            StartCoroutine(FadeOutPortalAndDisableSpawnLocation());
            // Deactivate the wave spawner after all rounds
            // gameObject.SetActive(false);

            if (portalManager != null)
            {
                portalManager.ActivateNewPortal(); // Activate the new portal
            }
        }
    }

    IEnumerator SpawnEnemiesInWave()
    {
        isSpawning = true;
        int remainingValue = currentWaveValue;

        // Wait for a delay between rounds before starting the next round
        yield return new WaitForSeconds(delayBetweenRounds);


        while (remainingValue > 0)
        {
            // Select a random enemy from the list
            EnemyData selectedEnemy = GetRandomEnemy();

            if (selectedEnemy != null)
            {
                // Check if we can afford to spawn this enemy
                if (remainingValue >= selectedEnemy.cost)
                {
                    // Spawn the enemy at the spawn location
                    SpawnEnemy(selectedEnemy.enemyPrefab);
                    remainingValue -= selectedEnemy.cost;
                }
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                Debug.LogWarning("No valid enemies to spawn.");
                break;
            }
        }

        // Wait until all enemies are defeated
        while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
            yield return null;
        }

        // Start next round after all enemies are defeated
        isSpawning = false;
        StartRound();
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (spawnLocation != null && enemyPrefab != null)
        {
            // Spawn the enemy at the spawn location
            Instantiate(enemyPrefab, spawnLocation.position, Quaternion.identity);
        }
    }

    IEnumerator FadeOutPortalAndDisableSpawnLocation()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds before starting the fade-out
        

        if (portalRenderer != null)
        {
            Color color = portalRenderer.color;
            while (color.a > 0f)
            {
                color.a -= Time.deltaTime;
                portalRenderer.color = color;
                yield return null;
            }
        }

        // Disable the spawnLocation object after fading out the portal
        if (spawnLocation != null)
        {
            spawnLocation.gameObject.SetActive(false);
        }
    }

    public void LoadData(GameData data)
    {
        data.objectives.TryGetValue(id, out completed);
        if (completed)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.objectives.ContainsKey(id))
        {
            data.objectives.Remove(id);
        }
        data.objectives.Add(id, completed);
    }

    EnemyData GetRandomEnemy()
    {
        if (enemies.Count > 0)
        {
            return enemies[Random.Range(0, enemies.Count)];
        }
        return null;
    }
}
