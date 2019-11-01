using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;

public class QuestionLoader : MonoBehaviour
{

    public class Question
    {
        public string Title { get; set; }
        public string Correctanswer { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string PictureName { get; set; }
    }

    public List<Question> questions = new List<Question>();
    public int currentQuestion;
    void Start()
    {
        LoadQuestions();
        if (questions.Count > 0)
        {
            Debug.Log("Loaded " + questions.Count + " questions!");
            var question = GameObject.Find("Question").GetComponent<Text>();
            var option1 = GameObject.Find("Option 1").GetComponent<Text>();
            var option2 = GameObject.Find("Option 2").GetComponent<Text>();
            var option3 = GameObject.Find("Option 3").GetComponent<Text>();
            var option4 = GameObject.Find("Option 4").GetComponent<Text>();
            var questionImage = GameObject.Find("ImageQuestion").GetComponent<Image>();
            System.Random r = new System.Random();
            int iq = r.Next(0, questions.Count);
            question.text = questions[iq].Title;
            if (questions[iq].PictureName != null)
            {
                questionImage.gameObject.SetActive(true);
                Debug.Log(questions[iq].PictureName);
#if UNITY_EDITOR
                questionImage.sprite = Resources.Load<Sprite>(questions[iq].PictureName);
#else
                questionImage.sprite = Resources.Load<Sprite>(questions[iq].PictureName.Substring(0, questions[iq].PictureName.Length - 1));
#endif
                questionImage.preserveAspect = true;
            } else
            {
                questionImage.gameObject.SetActive(false);
            }
            option1.text = questions[iq].Option1;
            option2.text = questions[iq].Option2;
            option3.text = questions[iq].Option3;
            option4.text = questions[iq].Option4;
            currentQuestion = iq;
        }
        else
        {
            Debug.Log("Loading questions FAILED, 0 questions found in the file questions.txt");
        }
    }

    private void LoadQuestions()
    {
        try
        {
            var questionfile = Resources.Load("questions") as TextAsset;
            //Debug.Log(questionfile.text);
            string[] lines = questionfile.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            //Debug.Log(lines);
            for (var i = 0; i < lines.Length - 1; i++)
            {
                if (lines[i] != null && !(lines[i].Contains("--")))
                {

                    string[] entries = lines[i].Split(',');
                    if (entries.Length == 6)
                    {
                        //Debug.Log("Successful load - line no: " + i + " " + lines[i]);
                        var question = new Question()
                        {
                            Title = entries[0],
                            Option1 = entries[1],
                            Option2 = entries[2],
                            Option3 = entries[3],
                            Option4 = entries[4],
                            Correctanswer = entries[5]
                        };
                        questions.Add(question);
                    }
                    else if (entries.Length == 7)
                    {
                        var question = new Question()
                        {
                            Title = entries[0],
                            Option1 = entries[1],
                            Option2 = entries[2],
                            Option3 = entries[3],
                            Option4 = entries[4],
                            Correctanswer = entries[5],
                            PictureName = entries[6]
                        };
                        questions.Add(question);
                    }
                    else
                    {
                        Debug.Log("Error: The format is not correct for line no: " + i + " contents: " + lines[i] + " " + entries);
                    }
                }
            }



        }
        catch (Exception e)
        {
            Console.WriteLine("{0}\n", e.Message);

        }
    }
}
