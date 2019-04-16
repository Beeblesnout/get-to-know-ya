using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Events;
using Popcron.Networking;
using System.Threading;
using System.Threading.Tasks;

public enum QSystemState
{
    Dormant, Asking, Resolving
}

public class QuestionManager : SingletonBase<QuestionManager>
{
    // Static Variables
    public static string[] allQuestions = new string[] {
        "Which one's better?", "Which one's worse?"
    };
    public static string[] allChoices = new string[] { 
        "Hot Pepper", "Cats", "Dogs","Cake", "Ketchup", 
        "Sushi", "B-Movies", "Peanuts", "Drag Queens", "Ska Music", 
        "Baby Boomers", "Black Jelly Beans", "Sake", "The Great Depression", "The Letter J",
        "Orange (fruit)", "Orange (colour)", "Orange (concept)", "The Earth", "Your Next-Door Neighbour"
    };
    public static List<Results> allResults = new List<Results>();

    // State Variables
    public QSystemState systemState;
    QSystemState lastSystemState;
    
    // Connected Components
    [Header("Connected Components")]
    public Canvas questionCanvas;
    public TMP_Text questionText;
    public TMP_Text choice1Text;
    public TMP_Text choice2Text;
    public RectTransform choice1Zone;
    public RectTransform choice2Zone;
    public Slider choiceTimeSlider;
    public Transform player;

    // Question System Variables
    [Header("Question System")]
    int questionIndex, choice1Index, choice2Index;
    int selectedChoice, partnerChoice;
    public float askTime = 5f;
    float startAskTime;
    public AudioSource audioSource;
    public AudioClip correct, incorrect;

    public SpawnEnemies enemySpawner;

    void Update()
    {
        if (lastSystemState != systemState)
        {
            lastSystemState = systemState;
            StateEnter();
        }
        StateUpdate();
    }

    public void StateSet(QSystemState state)
    {
        systemState = state;
    }

    public void StateEnter()
    {
        switch (systemState)
        {
            case QSystemState.Asking:
                questionCanvas.gameObject.SetActive(true);
                GenerateNewQuestion();
                startAskTime = Time.time;
                break;

            case QSystemState.Resolving:
                questionCanvas.gameObject.SetActive(true);
                partnerChoice = Random.Range(1, 3);
                Results result = new Results(
                    allQuestions[questionIndex],
                    allChoices[choice1Index],
                    allChoices[choice2Index],
                    selectedChoice,
                    partnerChoice
                );
                audioSource.PlayOneShot(result.matches ? correct : incorrect);
                allResults.Add(result);
                enemySpawner.SpawnWave(result.matches ? 10 : 20);
                systemState = QSystemState.Dormant;
                break;

            case QSystemState.Dormant:
                questionCanvas.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    public void StateUpdate()
    {
        switch (systemState)
        {
            case QSystemState.Asking:
                Vector3 playerLoc = Camera.main.WorldToScreenPoint(player.transform.position);
                if (RectTransformUtility.RectangleContainsScreenPoint(choice1Zone, playerLoc))
                {
                    selectedChoice = 1;
                    choice1Zone.GetComponent<Selectable>().interactable = true;
                    choice2Zone.GetComponent<Selectable>().interactable = false;
                }
                else if (RectTransformUtility.RectangleContainsScreenPoint(choice2Zone, playerLoc))
                {
                    selectedChoice = 2;
                    choice1Zone.GetComponent<Selectable>().interactable = false;
                    choice2Zone.GetComponent<Selectable>().interactable = true;
                }
                float askTimePerc = (Time.time - startAskTime) / askTime;
                choiceTimeSlider.value = 1-askTimePerc;
                if (askTimePerc > 1) systemState = QSystemState.Resolving;
                break;

            case QSystemState.Resolving:
                break;

            case QSystemState.Dormant:
                if (!enemySpawner.isSpawning && enemySpawner.existingEnemies.Count == 0) systemState = QSystemState.Asking;
                break;

            default:
                break;
        }
    }

    public void GenerateNewQuestion()
    {
        questionIndex = Random.Range(0, allQuestions.Length);
        choice1Index = Random.Range(0, allChoices.Length);
        choice2Index = Random.Range(0, allChoices.Length);
        if (choice1Index == choice2Index) choice2Index++;
        if (choice2Index == allChoices.Length) choice2Index = 0;

        questionText.text = allQuestions[questionIndex];
        choice1Text.text = allChoices[choice1Index];
        choice2Text.text = allChoices[choice2Index];
    }

    public void SetQuestionVars(int newQuestionIndex, int newChoice1Index, int newChoice2Index)
    {
        questionIndex = newQuestionIndex;
        choice1Index = newChoice1Index;
        choice2Index = newChoice2Index;

        questionText.text = allQuestions[questionIndex];
        choice1Text.text = allChoices[choice1Index];
        choice2Text.text = allChoices[choice2Index];
    }
}

public struct Results
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
    public Results(string question, string choiceA, string choiceB, int chosenAnswer, int partnerAnswer)
    {
        this.question = question;
        this.choiceA = choiceA;
        this.choiceB = choiceB;
        this.chosenAnswer = chosenAnswer;
        this.partnerAnswer = partnerAnswer;
        matches = string.Equals(chosenAnswer, partnerAnswer);
    }

    public override string ToString()
    {
        return string.Format(
            "{0} {1,-20} | {2,-20} | {3,-20}", 
            matches ? "O" : "X", 
            question, 
            chosenAnswer == 1 ? choiceA : choiceB, 
            partnerAnswer == 1 ? choiceA : choiceB
        );
    }
}
