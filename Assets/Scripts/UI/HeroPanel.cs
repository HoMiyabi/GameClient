using System.Collections;
using System.Collections.Generic;
using Kirara;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HeroPanel : UIBase
{
    [SerializeField] private Text jobText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text levelText;
    [SerializeField] private ChoiceItem choiceItem;
    [SerializeField] private Button button;

    public Image image;

    public int index;

    private HeroPanel Init(ChoiceGroup choiceGroup)
    {
        choiceGroup.Add(choiceItem);
        button.onClick.AddListener(choiceItem.Choose);

        return this;
    }

    public static HeroPanel New(ChoiceGroup choiceGroup)
    {
        var heroPanel = UIManager.Instance.NewUI<HeroPanel>("Prefabs/UI/HeroPanel");
        return heroPanel.Init(choiceGroup);
    }

    public void SetRole(string roleName, string jobName, int level)
    {
        nameText.text = roleName;
        jobText.text = jobName;
        levelText.text = $"Lv: {level}";
    }
}