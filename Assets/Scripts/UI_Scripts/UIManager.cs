using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private SaveSlot saveSlot;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        playerStat = FindObjectOfType<Character>();

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
}
