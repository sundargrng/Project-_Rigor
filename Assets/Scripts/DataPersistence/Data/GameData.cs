using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    public int deathCount;
    public SerializableTypeDictionary<string, bool> enemiesDefeated;
    public SerializableTypeDictionary<string, bool> gameObjectives;
    public SerializableTypeDictionary<string, bool> chestOpened;
    public Vector3 playerPosition;

    // Camera properties
    public Vector3 cameraPosition;
    public Vector2 cameraMinPosition;
    public Vector2 cameraMaxPosition;
    public float cameraSize;

    // Player stats
    public int playerHealth;
    public int playerHealthMax;
    public int currentExperience;
    public int currentExperienceMax;
    public int currentLevel;
    public int currentDamage;

    public bool isNPCDisabled;

    // Background music for the new area
    public MusicType newAreaBGM;

    public GameData()
    {
        this.deathCount = 0;
        this.playerPosition = Vector3.zero;
        this.enemiesDefeated = new SerializableTypeDictionary<string, bool>();
        this.gameObjectives = new SerializableTypeDictionary<string, bool>();
        this.chestOpened = new SerializableTypeDictionary<string, bool>();

        // Initialize camera properties
        this.cameraPosition = Vector3.zero;
        this.cameraMinPosition = Vector2.zero;
        this.cameraMaxPosition = Vector2.zero;
        this.cameraSize = 3.9f; // Default camera size

        // Initialize player stats
        this.playerHealth = 10;
        this.playerHealthMax = 10;
        this.currentLevel = 1;
        this.currentExperience = 0;
        this.currentExperienceMax = 200;
        this.currentDamage = 5;

        // Default background music for new area
        this.newAreaBGM = MusicType.AREA_1; // Set a default value
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

        int totalObjectives = 0;
        foreach (bool completed in gameObjectives.Values)
        {
            if (completed)
            {
                totalObjectives++;
            }
        }

        int totalChestOpened = 0;
        foreach(bool opened in chestOpened.Values)
        {
            if (opened)
            {
                totalChestOpened++;
            }
        }

        int totalCompleted = totalDefeated + totalObjectives + totalChestOpened;
        int totalCount = enemiesDefeated.Count + gameObjectives.Count + chestOpened.Count;

        int percentageCompleted = 0; // Default value in case totalCount is 0
        if (totalCount != 0)
        {
            percentageCompleted = totalCompleted * 100 / totalCount;
        }
        return percentageCompleted;
    }
}
