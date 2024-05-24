using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private WarriorController warriorController;

    private bool inSettingsMenu = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (inSettingsMenu && Input.GetKeyDown(KeyCode.Space))
        {
            inSettingsMenu = false;
            settingsPanel.SetActive(false);
            warriorController.ReturnPauseMenu();
        }
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
        if (Time.timeScale == 0)
        {
            settingsPanel.SetActive(true);
            inSettingsMenu = true;
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        inSettingsMenu = false;
        settingsPanel.SetActive(false);
    }

    /*public void CloseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        inSettingsMenu = false;

        Resume();
    }*/

    public void SaveGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        DataPersistenceManager.instance.SaveGame();
        Debug.Log("GAMESAVED. . .");
    }

    public void Quit()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        Application.Quit();
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
