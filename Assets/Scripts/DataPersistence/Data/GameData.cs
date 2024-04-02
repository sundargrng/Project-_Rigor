using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public int deathCount;
    public SerializableTypeDictionary<string, bool> enemiesDefeated;
    public Vector3 playerPosition;

    // New camera properties
    public Vector3 cameraPosition;
    public Vector2 cameraMinPosition;
    public Vector2 cameraMaxPosition;
    public float cameraSize;

    // New fields for player health, experience, and level
    public int playerHealth;
    public int playerHealthMax;
    public int currentExperience;
    public int currentExperienceMax;
    public int currentLevel;

    public bool isNPCDisabled;

    public GameData()
    {
        this.deathCount = 0;
        this.playerPosition = Vector3.zero;
        enemiesDefeated = new SerializableTypeDictionary<string, bool>();

        // Initialize camera properties
        this.cameraPosition = Vector3.zero;
        this.cameraMinPosition = Vector2.zero;
        this.cameraMaxPosition = Vector2.zero;
        this.cameraSize = 3.9f; // Set default camera size

        this.playerHealth = 10;
        this.playerHealthMax = 10;
        this.currentLevel = 1;
        this.currentExperience = 0;
        this.currentExperienceMax = 200;
    }

    public int GetPercentageComplete()
    {
        int totalDefeated = 0;
        foreach (bool defeated in enemiesDefeated.Values)
        {
            if (defeated)
            {
                totalDefeated++;
            }
        }

        int percentageCompleted = -1; // Default value in case enemiesDefeated is empty
        if (enemiesDefeated.Count != 0)
        {
            percentageCompleted = (totalDefeated * 100 / enemiesDefeated.Count);
        }
        return percentageCompleted;
    }
}
