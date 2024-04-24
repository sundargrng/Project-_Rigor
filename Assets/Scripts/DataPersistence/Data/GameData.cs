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

    public bool isNPCDisabled;

    // Background music for the new area
    public MusicType newAreaBGM;

    public GameData()
    {
        this.deathCount = 0;
        this.playerPosition = Vector3.zero;
        this.enemiesDefeated = new SerializableTypeDictionary<string, bool>();
        this.gameObjectives = new SerializableTypeDictionary<string, bool>();

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
        foreach (bool finished in gameObjectives.Values)
        {
            if (finished)
            {
                totalObjectives++;
            }
        }

        int percentageCompleted = 0; // Default value in case enemiesDefeated is empty
        if (enemiesDefeated.Count != 0 && gameObjectives.Count != 0)
        {
            percentageCompleted = (((totalDefeated + totalObjectives) * 100) / (enemiesDefeated.Count + gameObjectives.Count));
        }
        return percentageCompleted;
    }
}
