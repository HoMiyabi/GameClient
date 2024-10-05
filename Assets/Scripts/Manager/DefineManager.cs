using System.Collections.Generic;
using Kirara;
using Newtonsoft.Json;
using UnityEngine;

public class DefineManager : MonoSingleton<DefineManager>
{
    public Dictionary<int, SpaceDefine> SIDToSpaceDefine;
    public Dictionary<int, UnitDefine> TIDToUnitDefine;

    protected override void Awake()
    {
        base.Awake();

        Load();
    }

    private void Load()
    {
        SIDToSpaceDefine = LoadJson<SpaceDefine>("DefineJson/SpaceDefine");
        TIDToUnitDefine = LoadJson<UnitDefine>("DefineJson/UnitDefine");
    }

    private static string LoadText(string filePath)
    {
        string text = Resources.Load<TextAsset>(filePath).text;
        return text;
    }

    private static Dictionary<int, T> LoadJson<T>(string filePath)
    {
        string json = LoadText(filePath);
        return JsonConvert.DeserializeObject<Dictionary<int, T>>(json);
    }
}