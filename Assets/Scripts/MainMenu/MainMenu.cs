using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            loadGameButton.interactable = false;
        }
    }

    public void OnNewGameCLicked()
    {
        DisableMenuButtons();

        DataPersistenceManager.instance.NewGame();

        SceneManager.LoadSceneAsync("OpeningScene");
    }

    public void OnLoadGameClicked()
    {
        DisableMenuButtons();

        SceneManager.LoadSceneAsync("level1");
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        loadGameButton.interactable = false;
    }
}
