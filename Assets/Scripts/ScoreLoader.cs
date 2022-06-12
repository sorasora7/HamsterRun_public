using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLoader : MonoBehaviour
{
    [SerializeField] GameObject dateText;
    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject commentText;
    Text date_text;
    Text score_text;
    Text comment_text;

    void Start()
    {
        SaveManager savemanager = this.GetComponent<SaveManager>();
        date_text  = dateText.GetComponent<Text>();
        score_text = scoreText.GetComponent<Text>();
        comment_text = commentText.GetComponent<Text>();
        List<SaveData> saveData = savemanager.LoadScore();
        string outputDate = "";
        string outputScore = "";
        string outputComment = "";
        foreach (SaveData sd in saveData){
            outputDate += (sd.date + "\n");
            outputScore += (sd.score + "m\n");
            outputComment += (sd.comment + "\n");
        }
        date_text.text = outputDate;
        score_text.text = outputScore;
        comment_text.text = outputComment;
    }
}
