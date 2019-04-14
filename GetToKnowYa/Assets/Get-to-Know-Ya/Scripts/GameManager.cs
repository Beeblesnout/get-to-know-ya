using System.Collections;
using System.Collections.Generic;
using Popcron.Console;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    public GameObject pauseMenu;
    private bool gamePaused = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Console.Open = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) == true && SceneManager.GetActiveScene().name != "MainMenu")
        {
            EscapeMenu();
        }
    }

    public void LoadNextScene()
    {
        //Load the "Question" scene when the "start!" button is pressed
        SceneManager.LoadScene("Shooter", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        //Quit the application when the exit button is pressed
        Application.Quit();
    }

    public void HostGame()
    {
        Debug.Log("Host game selected!");
    }

    public void JoinGame()
    {
        Debug.Log("Join Game selected!");
    }

    public void EscapeMenu()
    {
        if(pauseMenu != null && gamePaused == false)
        {
            pauseMenu.SetActive(true);
            gamePaused = true;
        }
        else if (pauseMenu != null && gamePaused == true)
        {
            pauseMenu.SetActive(false);
            gamePaused = false;
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
