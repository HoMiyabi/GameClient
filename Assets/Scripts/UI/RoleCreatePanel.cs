using System.Collections.Generic;
using Kirara;
using Proto;
using UnityEngine;

public partial class RoleCreatePanel : UIBase
{
    [SerializeField] private ChoiceGroup choiceGroup;

    private List<string> jobIdToName = new List<string>()
    {
        "战士", "法师", "仙术", "游侠"
    };

    private void btnConfirm_onClick()
    {
        if (choiceGroup.chosenIndex == -1)
        {
            return;
        }
        print($"name={inputRoleName.text}, job={jobIdToName[choiceGroup.chosenIndex]}");

        CharacterCreateRequest request = new()
        {
            Name = inputRoleName.text,
            JobType = choiceGroup.chosenIndex + 1,
        };
        NetClient.Send(request);
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

        MessageRouter.Instance.Subscribe<CharacterCreateResponse>(OnCharacterCreateResponse);
    }

    private void OnDestroy()
    {
        MessageRouter.Instance.Unsubscribe<CharacterCreateResponse>(OnCharacterCreateResponse);
    }

    private void OnCharacterCreateResponse(Connection sender, CharacterCreateResponse message)
    {
        MainThread.Instance.Enqueue(() =>
        {
            var dialog = UIDialog.New("系统消息", message.Message);
            dialog.Open();
            dialog.AddButton(UIButton.New("哦", () => dialog.Close()).transform).Show();
            Close();
        });

        if (message.Success)
        {
            var request = new CharacterListRequest();
            NetClient.Send(request);
        }
    }
}