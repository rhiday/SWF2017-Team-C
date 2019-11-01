using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{

    private bool showgui;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            showgui = true;
        }
    }
    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            showgui = false;
        }
    }

    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        if (showgui == true)
        {
            GUIStyle custom = GUI.skin.button;
            custom.fontSize = 36;
            //GUI.Label(new Rect(500, 500, 200, 100), "player arrived in event zone", GUI.skin.box);
            if (GUI.Button(new Rect(Screen.width / 2 + 100, Screen.height / 2 - 50, Screen.width / 5, Screen.height / 5), "Open quiz", custom))
            {
                var manager = new ButtonManager();
                Debug.Log("click");
                manager.LoadNextScene("Quiz");
            }
        }
    }
}
