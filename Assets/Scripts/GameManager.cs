using System;
using System.Collections.Generic;
using Kirara;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public Canvas worldCanvas;

    public List<GameObject> dontDestroyOnLoad = new();

    public GameInfo gameInfo;

    protected override void Awake()
    {
        base.Awake();
        foreach (var go in dontDestroyOnLoad)
        {
            DontDestroyOnLoad(go);
        }

        Application.targetFrameRate = 90;
    }

    private void Start()
    {
        SceneManager.LoadScene("LoginScene");
    }

    private void Update()
    {
        float fps = 1.0f / Time.unscaledDeltaTime;
        gameInfo.EnqueueFrameRate(fps);
    }
}
