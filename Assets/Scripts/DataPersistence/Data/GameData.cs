using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
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
    public int currentLevel;

    public GameData()
    {
        this.deathCount = 0;
        this.playerPosition = Vector3.zero;
        // Initialize camera properties
        this.cameraPosition = Vector3.zero;
        this.cameraMinPosition = Vector2.zero;
        this.cameraMaxPosition = Vector2.zero;
        this.cameraSize = 3.9f; // Set default camera size

        this.playerHealth = 10;
        this.playerHealthMax = 10;
    }
}
