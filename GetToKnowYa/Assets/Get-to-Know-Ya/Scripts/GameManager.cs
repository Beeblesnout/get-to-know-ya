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

    public void OnStartPressed()
    {
        //Load the "Question" scene when the "start!" button is pressed
        SceneManager.LoadScene("Question", LoadSceneMode.Single);
    }

    public void OnExitPressed()
    {
        //Quit the application when the exit button is pressed
        Application.Quit();
    }
}
