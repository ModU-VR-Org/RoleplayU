using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DatabaseConstants 
{
    public static string serverURL = "http://ec2-18-144-41-17.us-west-1.compute.amazonaws.com"; 
    public static string selectCharacter = serverURL + "/selectCharacter.php"; 
    public static string initializePlayer = serverURL + "/initializePlayer.php";
    public static string readUsers = serverURL + "/read.php";
    public static string insertUser = serverURL + "/insertUser.php";
    public static string login = serverURL + "/login.php";
}

//If Hosting a custom server, change the serverURL to your server's URL. 
//The other variables corespond to PHP webpages that control game logic
//Your webserver must have webpages matching these names 
//with PHP scripts of the same functionality

//TODO: Provide documentation of PHP scripts
