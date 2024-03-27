using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IDataPersistence
{
    private HealthManager healthManager;
    public Slider hpAmount;
    public Text hpText;
    public Text deathCountText; // Text element to display death count
    private int deathCount = 0; // Variable to store death count

    // Start is called before the first frame update
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        hpAmount.maxValue = healthManager.maxHealth;
        hpAmount.value = healthManager.currentHealth;

        hpText.text = "HP " + healthManager.currentHealth + " / " + healthManager.maxHealth;

        // Update death count text
        deathCountText.text = "Deaths: " + deathCount;
    }

    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
    }

    public void SaveData (ref GameData data)
    {
        data.deathCount = this.deathCount;
    }


    // Method to increment death count
    public void IncrementDeathCount()
    {
        deathCount++;
    }
}
