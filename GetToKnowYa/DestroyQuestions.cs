using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyQuestions : MonoBehaviour
{
    public int questionHealth;
    string[] ChoicesMade = new string[] { };
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (questionHealth <= 0)
        {
            OnQuestionDeath();
        }
    }

    public float Damage()
    {
        return 0.0f;
    }

    public void OnQuestionDeath()
    {
   
    }
}


