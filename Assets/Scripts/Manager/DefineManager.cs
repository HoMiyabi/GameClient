using System.Collections.Generic;
using Kirara;
using Newtonsoft.Json;
using UnityEngine;

public class DefineManager : Singleton<DefineManager>
{
    public Dictionary<int, SpaceDefine> spaceDefineDict;
    public DefineManager()
    {
        string json = Resources.Load<TextAsset>("Data/SpaceDefine").text;
        spaceDefineDict = JsonConvert.DeserializeObject<Dictionary<int, SpaceDefine>>(json);
    }
}

public class SpaceDefine
{
    public int SID; // 场景编号
    public string Name; // 名称
    public string Resource; // 资源
    public string Kind; // 类型
    public int AllowPK; // 允许PK（1允许，0不允许）
}