using System;
using System.Collections;
using System.Collections.Generic;
using Kirara;
using UnityEngine;

public class LoginScene : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.NewUI<LoginPanel>(UIPrefab.Prefix + "LoginPanel").Open();
    }
}
