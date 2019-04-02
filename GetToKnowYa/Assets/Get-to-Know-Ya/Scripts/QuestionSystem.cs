﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionSystem : MonoBehaviour
{
    // Start is called before the first frame update
    string[] Choices1 = new string[] { "Hot Pepper", "Cats", "Dogs","Cake", "Ketchup" , "Sushi", "B-Movies"};
    string[] Choices2 = new string[] { "Peanuts", "Drag Queens", "Ska Music", "Baby Boomers", "Black Jelly Beans", "Sake"};
    string[] ChosenOption = new string[] { };
    int choiceNum = 0;
    //List<String> myList = new List<String>();
    public TMP_Text choice2Text;
    public TMP_Text choice1Text;
    float timeLeft = 5.0f;
    bool choiceDestroyed = true;
   Choice1 choice1;
    
    void Awake()
    {
       
    }
    void Start()
    {
        choice1 = GetComponent<Choice1>();
        //choice1.ChoiceSelected = 100;
    }

    // Update is called once per frame
    void Update()
    {



        //displays after timer
        //goes away and resets timer after destroyed
        if (choiceDestroyed == true)
        {

            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                getChoices();
                choiceDestroyed = false;

                //needs if statment for detecting bullets 
                if (choice1.ChoiceSelected==1) {
                    string choice1 = Choices1[Random.Range(0, Choices1.Length)];

                    ChosenOption[choiceNum] = (choice1);

                }

            }
        }

        Debug.Log(timeLeft);
    }

    public void getChoices() {
  //      string[] Choices = new string[] { "Hot Pepper", "Cats", "Dogs", "Cake", "Ketchup" };

        string choice1 = Choices1[Random.Range(0, Choices1.Length)];
        string choice2 = Choices2[Random.Range(0, Choices2.Length)];
        choice1Text.text = choice1;
        choice2Text.text = choice2;
        Debug.Log(choice1);


    }

}
