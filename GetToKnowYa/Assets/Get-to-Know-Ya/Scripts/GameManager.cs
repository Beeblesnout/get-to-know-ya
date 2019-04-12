using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        //Load the "Question" scene when the "start!" button is pressed
        SceneManager.LoadScene("EnemyTests", LoadSceneMode.Single);
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
}
