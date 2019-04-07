using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class QuestionManager : SingletonBase<QuestionManager>
{
    public static string[] allChoices = new string[] { 
        "Hot Pepper", "Cats", "Dogs","Cake", "Ketchup", 
        "Sushi", "B-Movies", "Peanuts", "Drag Queens", "Ska Music", 
        "Baby Boomers", "Black Jelly Beans", "Sake"
        };
    public static List<QuestionResults> allResults = new List<QuestionResults>();

    int choice1Index;
    int choice2Index;
    int choiceNum = 0;
    int chosenChoice = 0;
    float timeLeft;
    public float choiceTime = 5f;
    bool choiceDestroyed = true;
    
    public TMP_Text choice1Text, choice2Text;
    public RectTransform choice1Zone, choice2Zone;
    public Transform player;

    void Start() 
    {
        timeLeft = choiceTime;
    }
    
    void Update()
    {
        Vector3 playerLoc = Camera.main.WorldToScreenPoint(player.position);
        Debug.Log(playerLoc);

        

        if (RectTransformUtility.RectangleContainsScreenPoint(choice1Zone, playerLoc))
        {
            chosenChoice = 1;
            choice1Zone.GetComponent<Selectable>().interactable = true;
            choice2Zone.GetComponent<Selectable>().interactable = false;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(choice2Zone, playerLoc))
        {
            chosenChoice = 2;
            choice1Zone.GetComponent<Selectable>().interactable = false;
            choice2Zone.GetComponent<Selectable>().interactable = true;
        }

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            QuestionResults results = new QuestionResults();
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }

    public void generateChoices() {
        choice1Index = Random.Range(0, allChoices.Length);
        choice2Index = Random.Range(0, allChoices.Length);
        if (choice1Index == choice2Index) choice2Index++;
        if (choice2Index == allChoices.Length) choice2Index = 0;

        choice1Text.text = allChoices[choice1Index];
        choice2Text.text = allChoices[choice2Index];
    }

}

public struct QuestionResults
{
    public string question;
    public string choiceA, choiceB;
    public int chosenAnswer;
    public int partnerAnswer;
    public bool matches;

    /// <summary>
    /// Creates a question result object
    /// </summary>
    /// <param name="question">The question as a string</param>
    /// <param name="choiceA">Choice A as a string</param>
    /// <param name="choiceB">Choice B as a string</param>
    /// <param name="chosenAnswer">The player's chosen answer as an int (should be either 1 or 2)</param>
    /// <param name="partnerAnswer">The partner's chosen answer as an int (should be either 1 or 2)</param>
    public QuestionResults(string question, string choiceA, string choiceB, int chosenAnswer, int partnerAnswer)
    {
        this.question = question;
        this.choiceA = choiceA;
        this.choiceB = choiceB;
        this.chosenAnswer = chosenAnswer;
        this.partnerAnswer = partnerAnswer;
        matches = string.Equals(chosenAnswer, partnerAnswer);
    }
}
