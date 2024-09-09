using System;
using System.Collections;
using System.Collections.Generic;
using Kirara;
using PimDeWitte.UnityMainThreadDispatcher;
using Proto;
using Summer.Network;
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
        MessageRouter.Instance.Subscribe<UserLoginResponse>(OnUserLoginResponse);
    }

    private void OnUserLoginResponse(Connection sender, UserLoginResponse message)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Debug.Log(message);

            var dialog = UIDialog.New("登录结果", message.Message);
            dialog.AddButton(UIButton.New("取消", () =>
                {
                    print("取消");
                    dialog.Dismiss();
                }).transform)
                .AddButton(UIButton.New("确定", () =>
                {
                    print("确定");
                    dialog.Dismiss();
                }).transform)
                .Show();
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
