using System;
using System.Collections;
using System.Collections.Generic;
using Kirara;
using Proto;
using Summer.Network;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;

    private void Awake()
    {
        loginButton.onClick.AddListener(LoginUI);
        MessageRouter.Instance.Subscribe<UserLoginResponse>(OnUserLoginResponse);
    }

    private void OnUserLoginResponse(Connection sender, UserLoginResponse message)
    {
        MainThread.Instance.Enqueue(() =>
        {
            Debug.Log(message);

            if (message.Success)
            {
                SceneManager.LoadScene("ChooseCharacterScene");
            }
            else
            {
                var dialog = UIDialog.New("系统消息", message.Message);
                dialog.AddButton(UIButton.New("确定", () =>
                    {
                        print("确定");
                        dialog.Dismiss();
                    }).transform)
                    .Show();
            }
        });
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