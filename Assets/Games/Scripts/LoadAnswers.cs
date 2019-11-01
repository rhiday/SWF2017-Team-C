using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadAnswers : MonoBehaviour
{

    private float time = 0;
    void Start()
    {
        var resultText = GameObject.Find("ResultText").GetComponent<Text>();
        if (SwipeDetector.correct)
        {
            resultText.text = "You win!";
        } else
        {
            resultText.text = "You lose!";
        }
        var infoText = GameObject.Find("InfoText").GetComponent<Text>();
        infoText.text = "Correct answer: " + SwipeDetector.quizCorrectAnswer + '\n' + "You answered: " + SwipeDetector.playerAnswer;
        
    }

}
