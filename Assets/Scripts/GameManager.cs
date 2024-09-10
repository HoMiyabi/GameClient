using System;
using System.Collections;
using System.Collections.Generic;
using Kirara;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public Dropdown dropdown;

    protected override void Awake()
    {
        base.Awake();

        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>() {"1280 x 720", "800 x 600", "全屏"});
        dropdown.onValueChanged.AddListener(value =>
        {
            if (value == 0)
            {
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            }
            else if (value == 1)
            {
                Screen.SetResolution(800, 600, FullScreenMode.Windowed);
            }
            else if (value == 2)
            {
                var resolutions = Screen.resolutions;
                int width = resolutions[^1].width;
                int height = resolutions[^1].height;
                Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
            }
        });
    }
}
