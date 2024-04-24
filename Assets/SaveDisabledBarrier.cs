using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDisabledBarrier : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private bool finished = false;

    private void OnDisable()
    {
        finished = true;
    }

    public void LoadData(GameData data)
    {
        data.gameObjectives.TryGetValue(id, out finished);
        if (finished)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.gameObjectives.ContainsKey(id))
        {
            data.gameObjectives.Remove(id);
        }
        data.gameObjectives.Add(id, finished);
    }
}
