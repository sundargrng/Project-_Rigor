using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
    public Vector3 playerPosition;
    public int currentHealth;

    // New camera properties
    public Vector3 cameraPosition;
    public Vector2 cameraMinPosition;
    public Vector2 cameraMaxPosition;
    public float cameraSize;

    public GameData()
    {
        this.deathCount = 0;
        this.playerPosition = Vector3.zero;
        this.currentHealth = 10;
        // Initialize camera properties
        this.cameraPosition = Vector3.zero;
        this.cameraMinPosition = Vector2.zero;
        this.cameraMaxPosition = Vector2.zero;
        this.cameraSize = 3.9f; // Set default camera size
    }
}
