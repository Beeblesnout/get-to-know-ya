using System.Collections;
using System.Collections.Generic;
using Popcron.Console;
using Popcron.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    public GameObject pauseMenu;
    private bool gamePaused = false;
    
    void Start()
    {
        Console.Open = false;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if(Input.GetKeyDown(KeyCode.Escape) == true)
            {
                EscapeMenu();
            }
        }
    }

    public void LoadScene(int buildIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        //Quit the application when the exit button is pressed
        Application.Quit();
    }

    // public void HostGame()
    // {
    //     LoadNextScene();
    //     CommandsNetworking.Host();
    // }

    // public void JoinGame()
    // {
    //     LoadNextScene();
    //     CommandsNetworking.Connect();
    // }

    public void EscapeMenu()
    {
        if(pauseMenu != null && gamePaused == false)
        {
            pauseMenu.SetActive(true);
            gamePaused = true;
            Time.timeScale = 0.1f;
        }
        else if (pauseMenu != null && gamePaused == true)
        {
            pauseMenu.SetActive(false);
            gamePaused = false;
            Time.timeScale = 1f;
        }
    }
}
