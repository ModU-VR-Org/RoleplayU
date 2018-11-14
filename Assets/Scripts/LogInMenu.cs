using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogInMenu : MonoBehaviour
{
    public TMP_InputField logInUsernameField;
    public string logInUsername;
    public TMP_InputField logInPasswordField;
    public string logInPassword;

    public TMP_InputField newAccountEmailField;
    public string newAccountEmail;
    public TMP_InputField newAccountUsernameField;
    public string newAccountUsername;
    public TMP_InputField newAccountPasswordField;
    public string newAccountPassword;

    public GameObject status;
    public TextMeshProUGUI statusText;
    public GameObject registerStatus;
    public TextMeshProUGUI registerStatusText;
    public GameObject loggedInNetworkUI;

    //Database Stuff
    private string giantString;
    public string[] registeredUsers;


    public void Awake()
    {
        //Load prior login info from PlayerPrefs (Note: For more secure system Password shouldn't be stored like this)
        if (PlayerPrefs.HasKey("loginUsername") && PlayerPrefs.HasKey("loginPassword"))
        {
            string username = PlayerPrefs.GetString("loginUsername", "");
            string password = PlayerPrefs.GetString("loginPassword", "");
            ShowStoredUsernameAndPassword(username, password);
        }
    }


    void Start()
    {      
    }
    

    public void ShowStoredUsernameAndPassword(string username, string password)
    {
        logInUsernameField.text = username;
        logInPasswordField.text = password;
    }

    public void SignInButton()
    {
        logInUsername = logInUsernameField.text;
        logInPassword = logInPasswordField.text;

        TryLogin(logInUsername, logInPassword);
    }

    public void TryLogin(string username, string password)
    {
        StoreLoginInfo(username, password);

        status.SetActive(true);

        if (username == "" || password == "" || username == null || password == null)
        {
            statusText.text = "Username or Password can't be empty";
        }
        else
        {
            statusText.text = "Logging in...";
            StartCoroutine(EvaluateLoginInfo(username, password));
        }
    }

    private IEnumerator EvaluateLoginInfo(string username, string password)
    {       
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);

        WWW login = new WWW(DatabaseConstants.login, form);
        yield return login;

        //giantString = login.text;
        //Debug.Log(giantString);

        if (login.text == "failure")
        {
            statusText.text = "Incorrect username/password";
        }
        else if (login.text == "error!")
        {
            statusText.text = "An error occured on the server";
        }
        else
        {
            statusText.text = "Success!";

            loggedInNetworkUI.SetActive(true);

            status.SetActive(false);
            gameObject.SetActive(false);

            StoreLoginInfo(username, password);

            SessionToken.sessionToken = login.text;
        }
    }

    public void StoreLoginInfo(string username, string password)
    {
        PlayerPrefs.SetString("loginUsername", username);
        PlayerPrefs.SetString("loginPassword", password);
    }

    public void CreateAccountButton()
    {
        newAccountEmail = newAccountEmailField.text;
        newAccountUsername = newAccountUsernameField.text;
        newAccountPassword = newAccountPasswordField.text;

        TryRegistration(newAccountUsername, newAccountPassword, newAccountEmail);
    }

    public void TryRegistration(string username, string password, string email)
    {
        registerStatus.SetActive(true);
        if (username == "" || password == "" || email == "")
        {
            registerStatusText.text = "No empty fields allowed";
        }
        else
        {
            StartCoroutine(RegisterUser(username, password, email));
        }
    }

    public IEnumerator RegisterUser(string username, string password, string email)
    {
        if(!email.Contains("@"))
        {          
            registerStatusText.text = "Invalid email";
            yield break;
        }
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);
        form.AddField("emailPost", email);

        WWW register = new WWW(DatabaseConstants.insertUser, form);
        yield return register;

        //giantString = register.text;
        //Debug.Log(giantString);

        if(register.text == "usernameTaken")
        {
            registerStatusText.text = "Username is taken";
        }
        else if (register.text == "serverError")
        {
            registerStatusText.text = "An error occured on the server";
        }
        else if(register.text == "tableError")
        {
            registerStatusText.text = "An error occured adding your account";
        }
        else
        {
            StoreLoginInfo(username, password);

            registerStatusText.text = "Registration Successful";
            loggedInNetworkUI.SetActive(true);

            registerStatus.SetActive(false);
            gameObject.SetActive(false);

            SessionToken.sessionToken = register.text;
        }
    }
}
