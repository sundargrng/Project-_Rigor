using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        Debug.Log("Pause Menu opened");
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void MainMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void Settings()
    {
        // Implement your settings logic here
        Debug.Log("Settings opened");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void CloseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // Implementing IDataPersistence interface methods
    public void LoadData(GameData data)
    {
        // Implement loading logic here if needed
    }

    public void SaveData(GameData data)
    {
        // Implement saving logic here if needed
    }

}
