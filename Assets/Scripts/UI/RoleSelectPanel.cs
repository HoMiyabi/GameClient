using System;
using System.Collections;
using System.Collections.Generic;
using Kirara;
using Proto;
using Sirenix.OdinInspector;
using Summer.Network;
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
        public int id;
    }

    private List<RoleInfo> roles = new List<RoleInfo>();

    private List<HeroPanel> heroPanels = new List<HeroPanel>();

    private string[] jobToName = new string[] {"", "战士", "法师", "仙术", "游侠"};

    private void btnDeleteRole_onClick()
    {
        if (choiceGroup.chosenIndex == -1)
        {
            return;
        }

        var dialog = UIDialog.New("系统提示", "确定删除此角色吗？删除后无法恢复。");
        dialog.Open();
        dialog
            .AddButton(UIButton.New("取消", () => dialog.Close()).transform)
            .AddButton(UIButton.New("确定", () =>
            {
                var role = roles[choiceGroup.chosenIndex];
                print($"删除角色, id={role.id}, name={role.name}");
                var request = new CharacterDeleteRequest()
                {
                    CharacterId = role.id,
                };
                NetClient.Send(request);
                dialog.Close();
            }).transform)
            .Show();
    }

    private void btnStart_onClick()
    {
        if (choiceGroup.chosenIndex == -1)
        {
            return;
        }
        print("进入游戏" + roles[choiceGroup.chosenIndex].name);
    }

    private void Start()
    {
        btnCreateRole.onClick.AddListener(() =>
        {
            UIManager.Instance.NewUI<RoleCreatePanel>("Prefabs/UI/RoleCreatePanel").Open();
        });

        btnDeleteRole.onClick.AddListener(btnDeleteRole_onClick);

        btnStart.onClick.AddListener(btnStart_onClick);

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
            textLevelContent.text = roles[newIndex].level.ToString();
        });

        MessageRouter.Instance.Subscribe<CharacterListResponse>(OnCharacterListResponse);
        MessageRouter.Instance.Subscribe<CharacterDeleteResponse>(OnCharacterDeleteResponse);

        var request = new CharacterListRequest();
        NetClient.Send(request);
    }

    private void OnCharacterDeleteResponse(Connection sender, CharacterDeleteResponse message)
    {
        var request = new CharacterListRequest();
        NetClient.Send(request);
    }

    private void OnCharacterListResponse(Connection sender, CharacterListResponse message)
    {
        roles.Clear();
        foreach (var c in message.CharacterList)
        {
            roles.Add(new RoleInfo() {name = c.Name, job = c.TypeId, level = c.Level, id = c.Id});
        }

        MainThread.Instance.Enqueue(() =>
        {
            choiceGroup.Clear();
            heroPanels.Clear();
            traAllRole.DestroyAllChildren();
            for (int i = 0; i < roles.Count; i++)
            {
                var panel = HeroPanel.New(choiceGroup);
                panel.transform.SetParent(traAllRole);
                panel.SetRole(roles[i].name, jobToName[roles[i].job], roles[i].level);
                heroPanels.Add(panel);
            }
        });
    }
}