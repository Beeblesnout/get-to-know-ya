using System.Collections;
using System.Collections.Generic;
using Popcron.Console;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBase<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        Console.Open = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartPressed()
    {
        //Load the "Question" scene when the "start!" button is pressed
        SceneManager.LoadScene("Shooter", LoadSceneMode.Single);
    }

    [Command("quit")]
    public void OnExitPressed()
    {
        //Quit the application when the exit button is pressed
        Application.Quit();
    }
}
