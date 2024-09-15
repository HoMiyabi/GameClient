using System.Collections;
using System.Collections.Generic;
using Kirara;
using UnityEngine;

public class RoleListScene : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.NewUI<RoleSelectPanel>(UIPrefab.Prefix + "RoleSelectPanel").Open();
    }
}