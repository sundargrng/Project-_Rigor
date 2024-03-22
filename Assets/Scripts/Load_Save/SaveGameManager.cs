using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    private static SaveGameManager instance;

    public List<SaveableObject> SaveableObjects {  get; private set; }

    public static SaveGameManager Instance
    {
        get 
        { 
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SaveGameManager>();
            }
            return instance; 
        }
        set { instance = value; }
    }
    // Start is called before the first frame update
    void Awake()
    {
        SaveableObjects = new List<SaveableObject>();
    }

    public void Save()
    {
        
    }

    public void Load()
    {

    }

    public Vector3 StringToVector(string value)
    {
        return Vector3.zero;
    }

    public Quaternion StringToQuaternion(string value)
    {
        return Quaternion.identity;
    }
}
