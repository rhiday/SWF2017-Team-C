//Attach to Scene (main) camera to enable swiping!
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class SwipeDetector : MonoBehaviour
{

    //Debug messages
    private readonly string[] mMessage = {
        "",
        "Swipe Left",
        "Swipe Right",
        "Swipe Up",
        "Swipe Down",
        "Tap"
    };

    public static string quizCorrectAnswer;
    public static string playerAnswer;
    public static bool correct;

    //Used to calculate the angle of swipe
    private readonly Vector2 mXAxis = new Vector2(1, 0);
    private readonly Vector2 mYAxis = new Vector2(0, 1);

    private int mMessageIndex = 0;

    // The angle range for detecting swipe
    private const float mAngleRange = 30;

    // To recognize as swipe user should at lease swipe for this many pixels
    private const float mMinSwipeDist = 50.0f;

    // To recognize as a swipe the velocity of the swipe
    // should be at least mMinVelocity
    // Reduce or increase to control the swipe speed
    private const float mMinVelocity = 2000.0f;

    private Vector2 mStartPosition;
    private float mSwipeStartTime;

    //Android specific
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    // Use this for initialization
    void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
        quizCorrectAnswer = GameObject.Find("Question").GetComponent<QuestionLoader>().questions[GameObject.Find("Question").GetComponent<QuestionLoader>().currentQuestion].Correctanswer;

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        // Mouse button down, possible chance for a swipe
        if (Input.GetMouseButtonDown(0))
        {
            // Record start time and position
            mStartPosition = new Vector2(Input.mousePosition.x,
                                         Input.mousePosition.y);
            mSwipeStartTime = Time.time;
        }

        // Mouse button up, possible chance for a swipe
        if (Input.GetMouseButtonUp(0))
        {
            float deltaTime = Time.time - mSwipeStartTime;

            Vector2 endPosition = new Vector2(Input.mousePosition.x,
                                               Input.mousePosition.y);
            Vector2 swipeVector = endPosition - mStartPosition;

            float velocity = swipeVector.magnitude / deltaTime;

            if (velocity > mMinVelocity &&
                swipeVector.magnitude > mMinSwipeDist)
            {
                // if the swipe has enough velocity and enough distance

                swipeVector.Normalize();

                float angleOfSwipe = Vector2.Dot(swipeVector, mXAxis);
                angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;

                // Detect left and right swipe
                if (angleOfSwipe < mAngleRange)
                {
                    OnSwipeRight();
                }
                else if ((180.0f - angleOfSwipe) < mAngleRange)
                {
                    OnSwipeLeft();
                }
                else
                {
                    // Detect top and bottom swipe
                    angleOfSwipe = Vector2.Dot(swipeVector, mYAxis);
                    angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;
                    if (angleOfSwipe < mAngleRange)
                    {
                        OnSwipeUp();
                    }
                    else if ((180.0f - angleOfSwipe) < mAngleRange)
                    {
                        OnSwipeDown();
                    }
                    else
                    {
                        mMessageIndex = 0;
                    }
                }
            }
        }
#else
        // user is touching the screen with a single touch 
          if (Input.touchCount == 1) 
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list
 
                //Check if drag distance is greater than 15% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                            OnSwipeRight();
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                            OnSwipeLeft();
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                            OnSwipeUp();
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");
                            OnSwipeDown();
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 15% of the screen height
                    Debug.Log("Tap");
                    OnTap();
                }
            }
        }


#endif

    }

    public void OnButtonClick(int option)
    {
        switch (option)
        {
            case 1:
                mMessageIndex = 1;
                playerAnswer = GameObject.Find("Question").GetComponent<QuestionLoader>().questions[GameObject.Find("Question").GetComponent<QuestionLoader>().currentQuestion].Option1;
                CheckCorrectAnswer();
                break;
            case 2:
                mMessageIndex = 2;
                playerAnswer = GameObject.Find("Question").GetComponent<QuestionLoader>().questions[GameObject.Find("Question").GetComponent<QuestionLoader>().currentQuestion].Option4;
                CheckCorrectAnswer();
                break;
            case 3:
                mMessageIndex = 3;
                playerAnswer = GameObject.Find("Question").GetComponent<QuestionLoader>().questions[GameObject.Find("Question").GetComponent<QuestionLoader>().currentQuestion].Option3;
                CheckCorrectAnswer();
                break;
            case 4:
                mMessageIndex = 4;
                playerAnswer = GameObject.Find("Question").GetComponent<QuestionLoader>().questions[GameObject.Find("Question").GetComponent<QuestionLoader>().currentQuestion].Option2;
                CheckCorrectAnswer();
                break;
            default:
                break;
        }


    }

    private void OnSwipeLeft()
    {
        //mMessageIndex = 1;
        //playerAnswer = GameObject.Find("Question").GetComponent<QuestionLoader>().questions[GameObject.Find("Question").GetComponent<QuestionLoader>().currentQuestion].Option1;
        //CheckCorrectAnswer();
    }

    private void OnSwipeRight()
    {
        //mMessageIndex = 2;
        //playerAnswer = GameObject.Find("Question").GetComponent<QuestionLoader>().questions[GameObject.Find("Question").GetComponent<QuestionLoader>().currentQuestion].Option4;
        //CheckCorrectAnswer();
    }

    private void OnSwipeUp()
    {
        SlideUp();
    }

    private void OnSwipeDown()
    {
        SlideDown();
    }
    private void OnTap()
    {
        mMessageIndex = 5;
    }

    private void CheckCorrectAnswer()
    {
#if UNITY_EDITOR
        if (playerAnswer == quizCorrectAnswer)
        {
            correct = true;
        }
        else
        {
            correct = false;
        }
        SceneManager.LoadScene("Result");
#else

        if (playerAnswer.Equals(quizCorrectAnswer.Substring(0, quizCorrectAnswer.Length - 1)) || playerAnswer.Equals(quizCorrectAnswer))
        {
            correct = true;
        }
        else
        {
            correct = false;
        }
        SceneManager.LoadScene("Result");
#endif
    }

    private void SlideUp()
    {
        Vector3 destination = new Vector3(0f, -100f, 0f);
        GameObject questionpanel = GameObject.Find("QuestionPanel");
        for (int i = 0; i < 25; i++)
        {
            questionpanel.transform.localPosition = Vector3.Lerp(questionpanel.transform.localPosition, destination, 0.1f * Time.deltaTime);
            //yield return new WaitForSeconds(0.1f);
        }
        questionpanel.transform.localPosition = destination;
    }

    private void SlideDown()
    {
        Vector3 destination = new Vector3(0f, -1000f, 0f);
        GameObject questionpanel = GameObject.Find("QuestionPanel");

        for (int i = 0; i < 25; i++)
        {
            questionpanel.transform.localPosition = Vector3.Lerp(questionpanel.transform.localPosition, destination, 0.1f * Time.deltaTime);
            //yield return new WaitForSeconds(0.1f);
        }
        questionpanel.transform.localPosition = destination;
    }
}