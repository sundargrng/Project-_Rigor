using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        DisableButtonsDependingOnData();
        SoundManager.PlayMusic(MusicType.BACKGROUND_MUSIC_1, 1f);
    }

    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            ContinueButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }

    public void OnNewGameCLicked()
    {
        /*DisableMenuButtons();

        DataPersistenceManager.instance.NewGame();

        SceneManager.LoadSceneAsync("OpeningScene");*/

        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
        SoundManager.PlayMusic(MusicType.BACKGROUND_MUSIC_2, 0.5f);
    }

    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
        SoundManager.PlayMusic(MusicType.BACKGROUND_MUSIC_2, 0.5f);
    }

    public void OnContinueClicked()
    {
        DisableMenuButtons();

        DataPersistenceManager.instance.SaveGame();

        SceneManager.LoadSceneAsync("level1");
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        ContinueButton.interactable = false;
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
