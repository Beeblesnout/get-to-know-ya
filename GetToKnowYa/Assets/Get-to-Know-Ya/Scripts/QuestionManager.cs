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

enum QSystemState
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
        "Baby Boomers", "Black Jelly Beans", "Sake"
    };
    public static List<Results> allResults = new List<Results>();

    // State Variables
    QSystemState systemState, lastSystemState;
    bool holdStateUpdate;
    bool fetchedNewQuestion, fetchedPartnerResults;
    
    // Connected Components
    [Header("Connected Components")]
    public TMP_Text questionText, choice1Text, choice2Text;
    public RectTransform choice1Zone, choice2Zone;
    public Transform player;

    // Question System Variables
    [Header("Question System")]
    int questionIndex, choice1Index, choice2Index;
    int selectedChoice, partnerChoice;
    public float askTime = 5f;
    float currentAskTime, startAskTime;

    async void Start()
    {
        if (Net.IsServer)
        {

        }
        else if (Net.IsClient)
        {
            player = User.Local.Avatar.transform;
        }

        while (enabled)
        {
            if (lastSystemState != systemState)
            {
                lastSystemState = systemState;
                holdStateUpdate = false;
                await StateEnter();
            }
            StateUpdate();
            await Task.Delay(25);
        }
    }

    public async Task StateEnter()
    {
        switch (systemState)
        {
            case QSystemState.Asking:
                if (Net.IsServer)
                {
                    GenerateNewQuestion();
                    Message message = new Message(NMType.ServerNewQuestion);
                    message.Write(questionIndex);
                    message.Write(choice1Index);
                    message.Write(choice2Index);
                    message.Send();

                    await Task.Delay((int)askTime * 1000);

                    systemState = QSystemState.Resolving;
                }
                else if (Net.IsClient)
                {
                    while (!fetchedNewQuestion)
                    {
                        await Task.Delay(25);
                    }
                    startAskTime = Time.time;
                }
                break;

            case QSystemState.Resolving:
                if (Net.IsClient)
                {

                }
                break;

            case QSystemState.Dormant:
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
                if (Net.IsClient)
                {
                    Vector3 playerLoc = Camera.main.WorldToScreenPoint(User.Local.Avatar.transform.position);
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
                }
                currentAskTime = Time.time;
                if (currentAskTime - startAskTime < askTime) systemState = QSystemState.Resolving;
                break;

            case QSystemState.Resolving:
                break;

            case QSystemState.Dormant:
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
        fetchedNewQuestion = true;
    }

    public void SetQuestionVars(int newQuestionIndex, int newChoice1Index, int newChoice2Index)
    {
        questionIndex = newQuestionIndex;
        choice1Index = newChoice1Index;
        choice2Index = newChoice2Index;

        questionText.text = allQuestions[questionIndex];
        choice1Text.text = allChoices[choice1Index];
        choice2Text.text = allChoices[choice2Index];
        fetchedNewQuestion = true;
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
}
