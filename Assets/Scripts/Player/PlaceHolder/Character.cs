using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IDataPersistence
{
    public int currentExperience, maxExperience, currentLevel;

    private HealthManager playerHealth;

    private WarriorController player;

    public GameObject exp_Up;

    private void Start()
    {
        playerHealth = FindObjectOfType<HealthManager>();
        player = FindObjectOfType<WarriorController>();
    }

    private void Awake()
    {
        ExperienceManager.Instance = FindObjectOfType<ExperienceManager>();
        if (ExperienceManager.Instance != null)
        {
            Debug.Log("Found ExperienceManager in the scene.");
        }
        else
        {
            Debug.LogError("ExperienceManager not found in the scene.");
        }

        // subscribe to events
        if (ExperienceManager.Instance != null)
        {
            ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
        }
    }

    private void OnEnable()
    {
        if (ExperienceManager.Instance != null)
        {
            // since we are subscribe to the event in the swake method
            // no need to subscribe here or else the player gains double the exp

            // Ensure we're not already subscribed
            ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
        }
        else
        {
            Debug.LogWarning("ExperienceManager.Instance is null in Character script.");
        }
    }

    private void OnDisable()
    {
        if (ExperienceManager.Instance != null)
        {
            // Unsubscribe from events
            ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;
        }
    }

    private void HandleExperienceChange(int newExperience)
    {
        currentExperience += newExperience;

        if (currentExperience >= maxExperience)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        // Increment current level
        currentLevel++;

        player.playerDamage += 10;

        // Increase max health
        playerHealth.maxHealth += 5;

        // Reset current health to max health
        playerHealth.currentHealth = playerHealth.maxHealth;

        exp_Up.SetActive(true);
        SoundManager.PlaySound(SoundType.LEVELUPSOUND);
        StartCoroutine(WaitForExp_Up());
    }

    private IEnumerator WaitForExp_Up()
    {
        yield return new WaitForSeconds(0.5f);

        exp_Up.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        // Load player experience and level from saved data
        this.currentExperience = data.currentExperience;
        this.maxExperience = data.currentExperienceMax;
        this.currentLevel = data.currentLevel;

        // Subscribe to events after loading data
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
    }

    public void SaveData(GameData data)
    {
        // Save player experience and level to data
        data.currentExperience = this.currentExperience;
        data.currentExperienceMax = this.maxExperience;
        data.currentLevel = this.currentLevel;
    }
}
