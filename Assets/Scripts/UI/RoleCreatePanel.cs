using System;
using System.Collections;
using System.Collections.Generic;
using Kirara;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public partial class RoleCreatePanel : UIBase
{
    [SerializeField] private ChoiceGroup choiceGroup;

    private List<string> jobIdToName = new List<string>()
    {
        "战士", "法师", "仙术", "游侠"
    };

    private void btnConfirm_onClick()
    {
        print($"name={inputRoleName.text}, job={jobIdToName[choiceGroup.chosenIndex]}");
    }

    private void Start()
    {
        traChooseJobBtnRoot.DestroyAllChildren();
        btnBack.onClick.AddListener(Close);
        for (int i = 0; i < 4; i++)
        {
            var btn = ChooseJobBtn.New(choiceGroup, jobIdToName[i]);
            btn.transform.SetParent(traChooseJobBtnRoot);
        }
        choiceGroup.onChoiceChange.AddListener((newIndex, oldIndex) =>
        {
            textChosenJob.text = jobIdToName[newIndex];
        });
        btnConfirm.onClick.AddListener(btnConfirm_onClick);
    }
}