using System.Collections.Generic;
using Kirara;
using Newtonsoft.Json;
using UnityEngine;

public class DefineManager : Singleton<DefineManager>
{
    public Dictionary<int, SpaceDefine> spaceDefineDict;
    public Dictionary<int, UnitDefine> unitDefineDict;

    public DefineManager()
    {
        spaceDefineDict = Load<SpaceDefine>("Define/SpaceDefine.json");
        unitDefineDict = Load<UnitDefine>("Define/UnitDefine.json");
    }

    private static string LoadFile(string filePath)
    {
        string text = Resources.Load<TextAsset>(filePath).text;
        return text;
    }

    private static Dictionary<int, T> Load<T>(string filePath)
    {
        string json = LoadFile(filePath);
        return JsonConvert.DeserializeObject<Dictionary<int, T>>(json);
    }
}