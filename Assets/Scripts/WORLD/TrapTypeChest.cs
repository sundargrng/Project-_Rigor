using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTypeChest : Interactable, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private bool completed = false;

    public Sprite chestOpen;
    public Sprite chestClose;
    public List<GameObject> objectsToToggle = new List<GameObject>();

    private SpriteRenderer spriteRenderer;
    private bool oPen = false;

    public GameObject goodPortal;

    public GameObject barrier;

    [SerializeField] private float spawnRate = 2f;

    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private bool canSpawn = true;

    private int numEnemiesSpawned = 0;
    private int numEnemiesToSpawn = 0;

    private WarriorController warriorController;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetChestClose(); // Set the initial state to closed
        ToggleObjects(false); // Set all objects in the list to inactive
        
        warriorController = GameObject.FindGameObjectWithTag("Player").GetComponent<WarriorController>();
    }

    public override void Interact()
    {
        if (oPen && !completed)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        completed = false;
        warriorController.disableInteraction = true;
        spriteRenderer.sprite = chestOpen;
        oPen = true;
        ToggleObjects(true); // Set all objects in the list to active
        barrier.SetActive(true);
        goodPortal.SetActive(false);

        numEnemiesToSpawn += 3;
        StartCoroutine(Spawner());
        SoundManager.PlayMusic(MusicType.ENEMIES_SPAWNNED);
    }

    private void Close()
    {
        spriteRenderer.sprite = chestClose;
        oPen = false;
        ToggleObjects(false); // Set all objects in the list to inactive
    }

    private void SetChestClose()
    {
        spriteRenderer.sprite = chestClose;
        oPen = false;
    }

    private void ToggleObjects(bool active)
    {
        foreach (GameObject obj in objectsToToggle)
        {
            obj.SetActive(active);
        }
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (canSpawn && numEnemiesSpawned < numEnemiesToSpawn)
        {
            yield return wait;
            int ran = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[ran];

            if (objectsToToggle.Count > 0)
            {
                GameObject spawnPoint = objectsToToggle[Random.Range(0, objectsToToggle.Count)];
                Instantiate(enemyToSpawn, spawnPoint.transform.position, Quaternion.identity);
                numEnemiesSpawned++;

                
            }
            else
            {
                Debug.LogError("No spawn points found in objectsToToggle list.");
            }
        }
    }

    public void OnEnemyKilled()
    {
        numEnemiesSpawned--;
        if (numEnemiesSpawned <= 0)
        {
            OnAllEnemiesKilled();
        }
    }

    private void OnAllEnemiesKilled()
    {
        SoundManager.PlayAreaSound(AreaSound.RAIN);
        completed = true;
        barrier.SetActive(false);
        ToggleObjects(false);
        goodPortal.SetActive(true);

        warriorController.disableInteraction = false;
    }

    public void LoadData(GameData data)
    {
        data.gameObjectives.TryGetValue(id, out completed);
        if (completed)
        {
            barrier.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.gameObjectives.ContainsKey(id))
        {
            data.gameObjectives.Remove(id);
        }
        data.gameObjectives.Add(id, completed);
    }
}