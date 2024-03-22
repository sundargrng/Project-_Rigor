using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ObjectType { Player, Enemy, Loot }

public abstract class SaveableObject : MonoBehaviour
{
    protected string save;

    private ObjectType objectType;

    // Start is called before the first frame update
    private void Start()
    {
        SaveGameManager.Instance.SaveableObjects.Add(this);
    }

    public virtual void Save(int id)
    {

    }

    public virtual void Load(string[] values)
    {

    }

    public void DestroySaveable()
    {

    }
}
