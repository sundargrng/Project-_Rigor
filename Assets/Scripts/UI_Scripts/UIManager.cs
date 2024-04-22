using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IDataPersistence
{
    private HealthManager healthManager;
    private Character playerStat;

    public Slider hpAmount;
    public Slider expAmount;

    public Text hpText;
    public Text expText;
    public Text currentLevel;
    public Text deathCountText; // Text element to display death count
    private int deathCount = 0; // Variable to store death count

    public Text keyCountText; // Text element to display key count
    private int keyCount = 0; // Variable to store key count

    public Text enemiesDefeatedText; // Text element to display defeated enemies count
    public int enemiesToDisableBarrier = 15; // Number of enemies needed to disable the barrier
    private int enemiesDefeated = 0; // Variable to store defeated enemies count

    private SaveSlot saveSlot;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        playerStat = FindObjectOfType<Character>();
        keyCountText.gameObject.SetActive(false); // Initially hide the key count UI
        enemiesDefeatedText.gameObject.SetActive(false); // Initially hide the enemies defeated UI

        // Initialize enemies defeated text
        UpdateEnemiesDefeatedText();
    }

    // Update is called once per frame
    void Update()
    {
        hpAmount.maxValue = healthManager.maxHealth;
        hpAmount.value = healthManager.currentHealth;

        expAmount.maxValue = playerStat.maxExperience;
        expAmount.value = playerStat.currentExperience;
        // Update health bar and exp bar text
        hpText.text = "HP " + healthManager.currentHealth + " / " + healthManager.maxHealth;
        expText.text = "EXP " + playerStat.currentExperience + " / " + playerStat.maxExperience;

        // Update current level text
        currentLevel.text = "Level " + playerStat.currentLevel;

        // Update death count text
        deathCountText.text = "Deaths: " + deathCount;

        // Check if the current scene is "Scene2"
        if (SceneManager.GetActiveScene().name == "level1")
        {
            // Show the key count UI
            keyCountText.gameObject.SetActive(true);

            // Update key count text
            keyCountText.text = "Keys: " + keyCount;
        }
        else
        {
            // Hide the key count UI if not in "Scene2"
            keyCountText.gameObject.SetActive(false);
        }
    }

    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
    }

    public void SaveData (GameData data)
    {
        data.deathCount = this.deathCount;
    }


    // Method to increment death count
    public void IncrementDeathCount()
    {
        deathCount++;
    }

    public void IncrementKeyCount()
    {
        keyCount++;
    }

    // Method to increment defeated enemies count
    public void IncrementEnemiesDefeated()
    {
        enemiesDefeated++;
        UpdateEnemiesDefeatedText();

        // Check if enough enemies have been defeated to disable the barrier
        if (enemiesDefeated >= enemiesToDisableBarrier)
        {
            enemiesDefeatedText.gameObject.SetActive(false); // Disable the UI text element
            // Call method to disable the barrier (implement this method)
            DisableSpecificBarrier();
        }
    }

    // Method to update defeated enemies text
    private void UpdateEnemiesDefeatedText()
    {
        enemiesDefeatedText.text = "Enemies Defeated: " + enemiesDefeated + " / " + enemiesToDisableBarrier;
    }

    // Method to disable the specific barrier (implement this method)
    private void DisableSpecificBarrier()
    {
        // Add logic to disable the specific barrier GameObject
        // Example: specificBarrier.SetActive(false);
        Debug.Log("Barrier disabled after defeating " + enemiesToDisableBarrier + " enemies.");
    }

    // Public method to show enemies defeated text (called by EnemySpawnerActivator)
    public void ShowEnemiesDefeatedText()
    {
        enemiesDefeatedText.gameObject.SetActive(true);
    }
}
