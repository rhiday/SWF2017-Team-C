using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    #region variables
    //Static Vatiable
    public static string Email = "";
    public static string Password = "";

    //public variable
    public string CurrentMenu = "Login";





    //Private Variable
    private string CreateAcountUrl = "http://paulin.fi/SWF/CreateAccountT.php";
    private string LoginUrl = "http://paulin.fi/SWF/LoginAccountT.php";
    private string ConfirmPass = "";
    private string ConfirmEmail = "";
    private string CEmail = "";
    private string CPassword = "";

    //Panels
    private GameObject loginPanel;
    private GameObject createAccountPanel;
    private GameObject statusText;

    //GuI Test section
    public float X;
    public float Y;
    public float Width;
    public float Height;


    #endregion

    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.HasKey("player"))
        {
            SceneManager.LoadScene("ViewSwitch");
        }

        createAccountPanel = GameObject.Find("CreateAccountPanel");
        createAccountPanel.gameObject.SetActive(false);
        loginPanel = GameObject.Find("LoginPanel");
        statusText = GameObject.Find("StatusMessage");

    } // End start method

    ////Main GUI Function
    //void OnGUI()
    //{



    //    if (CurrentMenu == "Login")
    //    {
    //        //LoginGUI();
    //    }
    //    else if (CurrentMenu == "CreateAccount")
    //    {
    //        CreateAccountGUI();
    //    }

    //} //End onGUI

    public void LoginClick()
    {
        Email = GameObject.Find("EmailInput").GetComponent<InputField>().text;
        Password = GameObject.Find("PasswordInput").GetComponent<InputField>().text;
        if (Email != "" || Password != "")
        {
            StartCoroutine(LoginAccount());
        }
        else
        {
            statusText.GetComponent<Text>().text = "Username or password cannot be empty";
        }
    }

    public void ExitClick()
    {
        createAccountPanel.gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(true);
    }

    public void CreateAccountMenuClick()
    {
        createAccountPanel.gameObject.SetActive(true);
        loginPanel.gameObject.SetActive(false);
    }

    public void CreateAccountClick()
    {
        ConfirmPass = GameObject.Find("CPasswordInput").GetComponent<InputField>().text;
        CPassword = GameObject.Find("CPasswordInput2").GetComponent<InputField>().text;
        CEmail = GameObject.Find("CEmailInput").GetComponent<InputField>().text;
        ConfirmEmail = GameObject.Find("CEmailInput2").GetComponent<InputField>().text;
        if (ConfirmPass == "" || CPassword == "" || CEmail == "" || ConfirmEmail == "")
        {
            statusText.GetComponent<Text>().text = "All fields must be filled to create the account!";
        }
        else
        {
            if (ConfirmPass == CPassword && ConfirmEmail == CEmail)
            {
                StartCoroutine("CreateAccount");
            }
            else
            {
                statusText.GetComponent<Text>().text = "The Username / password confirmations do not match!";
            }
        }

    }

    #region coruoutine
    //Actually create account
    IEnumerator CreateAccount()
    {
        WWWForm Form = new WWWForm();
        Form.AddField("Email", CEmail);
        Form.AddField("Password", CPassword);
        WWW CreateAccount = new WWW(CreateAcountUrl, Form);
        //wait for php to send something back to unity
        yield return CreateAccount;
        if (CreateAccount.error != null)
        {
            statusText.GetComponent<Text>().text = "Cannot connect to Account Creation";
        }
        else
        {
            string CreateAccountReturn = CreateAccount.text;
            if (CreateAccountReturn == "Success")
            {
                statusText.GetComponent<Text>().text = "Success: Account created";
                createAccountPanel.gameObject.SetActive(false);
                loginPanel.gameObject.SetActive(true);
            }
            else
            {
                statusText.GetComponent<Text>().text = CreateAccountReturn;
            }
        }// End else


    } //End create account



    // Actual log in
    IEnumerator LoginAccount()
    {
        //Add out values that will go into the php script

        WWWForm Form = new WWWForm();
        //Make sure the email and password are spell the very same in the php script
        Form.AddField("Email", Email);
        Form.AddField("Password", Password);
        //Connect to the url
        WWW LoginAccountWWW = new WWW(LoginUrl, Form);
        yield return LoginAccountWWW;
        if (LoginAccountWWW.error != null)
        {
            statusText.GetComponent<Text>().text = "Can't connect to the log in";
        }
        else
        {
            string LogText = LoginAccountWWW.text;
            Debug.Log(LogText);
            statusText.GetComponent<Text>().text = LogText;
            if (LogText.Contains("success"))
            {
                PlayerPrefs.SetString("player", Email);
                SceneManager.LoadScene("ViewSwitch");
            }
            else
            {
                //if (LogTextSplit[1] == "Success")
                //{
                //    SceneManager.LoadScene("ViewSwitch");
                //}
            }

        }


    } // End login

    #endregion

}//End class
