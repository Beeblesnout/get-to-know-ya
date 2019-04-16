using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text text;

    void Start()
    {
        string end = string.Format("  {0,-20} | {1,-20} | {2,-20}", "Question", "Your Choice", "Partner's Choice");
        foreach (Results r  in QuestionManager.allResults)
        {
            end += "\n" + r.ToString();
        }
        text.text = end;
    }
}
