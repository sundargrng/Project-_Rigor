using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUi;

    public void GameOver()
    {
        gameOverUi.SetActive(true);
        Time.timeScale = 0;
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void exitGame()
    {
        Application.Quit();
    }

}
