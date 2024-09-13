using System;
using System.Collections;
using System.Collections.Generic;
using Kirara;
using UnityEngine;
using UnityEngine.UI;

public class ChooseJobBtn : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Text uiText;
    [SerializeField] private ChoiceItem choiceItem;

    private void Awake()
    {
        button.onClick.AddListener(choiceItem.Choose);
    }

    public static ChooseJobBtn New(ChoiceGroup choiceGroup, string text)
    {
        var self = UIManager.Instance.NewUI<ChooseJobBtn>("Prefabs/UI/ChooseJobBtn");
        choiceGroup.Add(self.choiceItem);
        self.uiText.text = text;
        return self;
    }
}