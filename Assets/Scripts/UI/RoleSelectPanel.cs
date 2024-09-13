using System;
using System.Collections;
using System.Collections.Generic;
using Kirara;
using UnityEngine;
using UnityEngine.UI;

public partial class RoleSelectPanel : UIBase
{
    [SerializeField] private ChoiceGroup choiceGroup;

    public class RoleInfo
    {
        public string name;
        public int job; // 1战士，2法师，3仙术，4游侠
        public int level;
    }

    private List<RoleInfo> roles = new List<RoleInfo>();

    private List<HeroPanel> heroPanels = new List<HeroPanel>();

    private string[] jobToName = new string[] {"", "战士", "法师", "仙术", "游侠"};

    /// <summary>
    /// 获取角色列表
    /// </summary>
    public void LoadRoles()
    {
        roles.Add(new RoleInfo()
        {
            name = "绮良良1",
            job = 1,
            level = 25,
        });
        roles.Add(new RoleInfo()
        {
            name = "绮良良2",
            job = 2,
            level = 32,
        });
        roles.Add(new RoleInfo()
        {
            name = "绮良良3",
            job = 3,
            level = 19,
        });
        roles.Add(new RoleInfo()
        {
            name = "绮良良4",
            job = 4,
            level = 66,
        });

        for (int i = 0; i < roles.Count; i++)
        {
            var panel = HeroPanel.New(choiceGroup);
            panel.transform.SetParent(traAllRole);
            panel.SetRole(roles[i].name, jobToName[roles[i].job], roles[i].level);
            heroPanels.Add(panel);
        }

        choiceGroup.onChoiceChange.AddListener((newIndex, oldIndex) =>
        {
            if (oldIndex != -1)
            {
                var oldPanel = heroPanels[oldIndex];
                oldPanel.image.gameObject.SetActive(false);
            }

            var newPanel = heroPanels[newIndex];
            newPanel.image.gameObject.SetActive(true);

            textNameContent.text = roles[newIndex].name;
            textJobContent.text = jobToName[roles[newIndex].job];
            textLavelContent.text = roles[newIndex].level.ToString();
        });
    }

    private void Start()
    {
        btnCreateCharacter.onClick.AddListener(() =>
        {
            UIManager.Instance.NewUI<RoleCreatePanel>("Prefabs/UI/RoleCreatePanel").Open();
        });

        btnRemoveCharacter.onClick.AddListener(() =>
        {
            print(roles[choiceGroup.chosenIndex].name);
        });

        traAllRole.DestroyAllChildren();

        LoadRoles();
    }
}