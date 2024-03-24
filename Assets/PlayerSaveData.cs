using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveLoadSystem;

public class PlayerSaveData : MonoBehaviour
{

    private PlayerData MyData = new PlayerData();
    

    // Update is called once per frame
    void Update()
    {
        var transform1 = transform;

        MyData.PlayerPosition = transform1.position;
        MyData.PlayerRotation = transform1.rotation;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SaveGameManager.CurrentSaveData.PlayerData = MyData;
            SaveGameManager.SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SaveGameManager.LoadGame();
            MyData = SaveGameManager.CurrentSaveData.PlayerData;
            transform1.position = MyData.PlayerPosition;
            transform1.rotation = MyData.PlayerRotation;
        }
    }
}

[System.Serializable]

public struct PlayerData
{
    public Vector3 PlayerPosition;

    public Quaternion PlayerRotation;

    public int CurrentHealth;
}
