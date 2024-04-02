using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour, IDataPersistence
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI playerLevel;
    [SerializeField] private TextMeshProUGUI deathCount;
    [SerializeField] private TextMeshProUGUI percentageComplete;

    [Header("Delete data button")]
    [SerializeField] private Button deleteButton;

    private Button saveSlotButton;

    public bool hasData { get; private set; } = false;

    private void Awake()
    {
        saveSlotButton = GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if (data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            deleteButton.gameObject.SetActive(false);
        }
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            deleteButton.gameObject.SetActive(true);

            // Update percentage completed
            percentageComplete.text = data.GetPercentageComplete() + "% COMPLETE";

            // Update player level and death count
            playerLevel.text = "Player Level: " + data.currentLevel;
            deathCount.text = "Death Count: " + data.deathCount;
        }
    }

    public string GetProfileId() 
    { 
        return profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        deleteButton.interactable = interactable;
    }

    // Implement IDataPersistence interface methods

    public void LoadData(GameData data)
    {
        // Update player level and death count when loading data
        if (data != null)
        {
            playerLevel.text = "Player Level: " + data.currentLevel;
            deathCount.text = "Death Count: " + data.deathCount;
        }
    }

    public void SaveData(GameData data)
    {
        // No need to save data from save slot
    }
}
