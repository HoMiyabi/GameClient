using System;
using System.Collections;
using System.Collections.Generic;
using Kirara;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public Dropdown dropdown;

    public Canvas worldCanvas;

    private List<string> resolutionOptionTexts = new()
    {
        "1280 x 720",
        "800 x 600",
        "全屏",
    };

    private List<Vector2Int> resolutionOptions = new()
    {
        new Vector2Int(1280, 720),
        new Vector2Int(800, 600),
        new Vector2Int(-1, -1),
    };

    protected override void Awake()
    {
        base.Awake();

        dropdown.ClearOptions();
        dropdown.AddOptions(resolutionOptionTexts);
        dropdown.onValueChanged.AddListener(value =>
        {
            var resolution = resolutionOptions[value];
            if (resolution == new Vector2Int(-1, -1))
            {
                var resolutions = Screen.resolutions;
                int width = resolutions[^1].width;
                int height = resolutions[^1].height;
                Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
            }
            else
            {
                Screen.SetResolution(resolution.x, resolution.y, FullScreenMode.FullScreenWindow);
            }
        });
    }
}
