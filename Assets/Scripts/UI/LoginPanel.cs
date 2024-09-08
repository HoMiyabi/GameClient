using System;
using System.Collections;
using System.Collections.Generic;
using Proto;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;

    private void Awake()
    {
        loginButton.onClick.AddListener(LoginUI);
    }

    private void LoginUI()
    {
        Login(usernameInput.text, passwordInput.text);
    }

    private static void Login(string username, string password)
    {
        var request = new UserLoginRequest()
        {
            Username = username,
            Password = password,
        };
        NetClient.Send(request);
    }
}
