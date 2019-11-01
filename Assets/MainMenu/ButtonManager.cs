using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class ButtonManager : MonoBehaviour
{
    public Image loadImage;
    public string sceneName;
    // Use this for initialization

    public void LoadNextScene(string sceneName)
    {
        if (sceneName == "login" && PlayerPrefs.HasKey("player"))
        {
            SceneManager.LoadScene("ViewSwitch");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }

    }

    IEnumerator Start()
    {
        if (loadImage != null)
        {
            loadImage.canvasRenderer.SetAlpha(0.0f);

            FadeIn();
            yield return new WaitForSeconds(2.5f);

            FadeOut();
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene(sceneName);
        }

    }

    void FadeIn()
    {
        loadImage.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOut()
    {
        loadImage.CrossFadeAlpha(0.0f, 1.5f, false);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "ViewSwitch")
            {
                SceneManager.LoadScene("Intro");
            }
            else if (SceneManager.GetActiveScene().name == "Intro")
            {
                Quit();
            }
            else
            {
                SceneManager.LoadScene("ViewSwitch");
            }

            return;
        }

    }

    public void Quit()
    {
        Application.Quit();
    }
}

