using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    /// <summary>
    /// NOTE: AS OF RIGHT NOW, I don't think this does anything. This was replaced by LogInMenu.cs
    /// </summary>

    public Text status;
    public InputField inputUser, inputPass, regUsername, regPassword, regEmail;
    int currentID;
    bool takenUsername;

    string giantString;
    public string[] registeredUsers;

    public string[] userNames = new string[100]; //change this number to how many users you expect
    public string[] passwords = new string[100]; // change this number to how many users you expect
    IEnumerator Start()
    {
        //WWW users = new WWW("http://skygnite.000webhostapp.com/read.php");
        WWW users = new WWW("http://ec2-52-53-224-203.us-west-1.compute.amazonaws.com/read.php");
        //http://ec2-52-53-224-203.us-west-1.compute.amazonaws.com/filename.php
        yield return users;
        giantString = users.text;

        registeredUsers = giantString.Split(';');

        for (int i = 0; i < registeredUsers.Length - 1; i++)
        {
            userNames[i] = registeredUsers[i].Substring(registeredUsers[i].IndexOf('U') + 9);
            userNames[i] = userNames[i].Remove(userNames[i].IndexOf("|"));

            passwords[i] = registeredUsers[i].Substring(registeredUsers[i].IndexOf("Password") + 9);
        }
    }
    public void TryToLogin()
    {
        currentID = -1;

        if (inputUser.text == "" || inputPass.text == "")
        {
            status.text = "Username or Password can't be empty";
        }
        else
        {
            //Note: from my own guessing, this is where you would make a SQL call (passing username and password). First you would use WWW to call a PHP script, which would run a sql command to search the database for the username, then check that the input password matches the database password, and return true or false back to Unity along with the userID + any other auth stuff like session cookie etc
            for (int i = 0; i < registeredUsers.Length - 1; i++)
            {
                if (inputUser.text == userNames[i])
                {
                    currentID = i;
                }
            }

            if (currentID == -1)
            {
                status.text = "User not found"; //note: this seems dangerous security-wise
            }
            else
            {
                if (inputPass.text == passwords[currentID])
                {
                    status.text = "Success!";
                }
                else
                {
                    status.text = "Password incorrect";
                }
            }
        }
    }
    public void TryToRegister()
    {
        takenUsername = false;
        if (regUsername.text == "" || regPassword.text == "" || regEmail.text == "")
        {
            status.text = "No empty fields allowed";
        }
        else
        {
            for (int i = 0; i < registeredUsers.Length - 1; i++)
            {
                if (regUsername.text == userNames[i])
                {
                    takenUsername = true;
                }
            }
            if (takenUsername == false && regUsername.text != "Password" && !regUsername.text.Contains("Password")) //Note: replace = Password with .Contains Password since better and redundant
            {
                status.text = "Registration Successful";
                RegisterUser(regUsername.text, regPassword.text, regEmail.text);
            }
            else
            {
                status.text = "Username is already taken";
            }
        }
    }
    public void RegisterUser(string username, string password, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);
        form.AddField("emailPost", email);

        WWW register = new WWW("http://ec2-52-53-224-203.us-west-1.compute.amazonaws.com/insertUser.php", form);
    }

}
