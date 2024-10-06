using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public partial class GameInfo : MonoBehaviour
{
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

    private void Start()
    {
        ddResolution.ClearOptions();
        ddResolution.ClearOptions();
        ddResolution.AddOptions(resolutionOptionTexts);
        ddResolution.onValueChanged.AddListener(value =>
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
                Screen.SetResolution(resolution.x, resolution.y, FullScreenMode.Windowed);
            }
        });

        SetFrameRateAsync().Forget();
    }

    public void SetNetworkLatency(long ms)
    {
        textNetworkLatency.text = $"{ms} ms";
        textNetworkLatency.color = (ms < 100) ? Color.green : Color.red;
    }


    private List<float> frameRates = new(60);
    private int frameRatesFront = 0;

    public void EnqueueFrameRate(float fps)
    {
        if (frameRates.Count < 60)
        {
            frameRates.Add(fps);
        }
        else
        {
            frameRates[frameRatesFront] = fps;
        }
        frameRatesFront = (frameRatesFront + 1) % 60;
    }

    private async UniTaskVoid SetFrameRateAsync()
    {
        while (true)
        {
            await UniTask.WaitForSeconds(2);

            float maFps = frameRates.Average();
            textFrameRate.text = $"{maFps:F1} fps";
            textFrameRate.color = Color.green;
        }
    }

    private void Update()
    {
        float fps = 1.0f / Time.unscaledDeltaTime;
        EnqueueFrameRate(fps);
    }
}