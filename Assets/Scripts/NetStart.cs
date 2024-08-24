using Summer.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;
using Proto;
using UnityEngine;
using UnityEngine.UI;

public class NetStart : MonoBehaviour
{
    [Header("服务器信息")]
    public string host = "127.0.0.1";
    public int port = 32510;

    [Header("登录参数")]
    public InputField usernameInput;
    public InputField passwordInput;

    public Button playBtn;

    private GameObject hero; // 当前的角色

    private void Awake()
    {
        playBtn.onClick.AddListener(EnterGame);
    }

    void Start()
    {
        NetClient.ConnectToServer(host, port);

        MessageRouter.Instance.Subscribe<Proto.GameEnterResponse>(OnGameEnterResponse);
        MessageRouter.Instance.Subscribe<Proto.SpaceCharactersEnterResponse>(OnSpaceCharactersEnterResponse);
    }

    private async UniTaskVoid SyncRequestAsync()
    {
        while (true)
        {
            SpaceEntitySyncRequest request = new();
            await UniTask.WaitForSeconds(0.1f);
        }
    }

    // 加入游戏的响应结果 Entity肯定是自己
    private void OnGameEnterResponse(Connection sender, Proto.GameEnterResponse message)
    {
        Debug.Log("加入游戏的响应结果:" + message.Success);
        if (message.Success)
        {
            Debug.Log("角色信息:" + message.Entity);

            var entity = message.Entity;
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                playBtn.gameObject.SetActive(false);

                var prefab = Resources.Load<GameObject>("Prefabs/DogPBR");

                hero = Instantiate(prefab);
                var gameEntity = GetComponent<GameEntity>();
                if (gameEntity != null)
                {
                    gameEntity.SetData(entity);
                }

                SyncRequestAsync().Forget();
            });
        }
    }

    // 当有角色进入地图的通知 肯定不是自己
    private void OnSpaceCharactersEnterResponse(Connection conn, Proto.SpaceCharactersEnterResponse message)
    {
        Debug.Log("角色进入地图 " + message);
        var entities = message.EntityList;

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var prefab = Resources.Load<GameObject>("Prefabs/DogPBR");

            foreach (var e in entities)
            {
                Instantiate(
                    prefab,
                    new Vector3(e.Position.X, e.Position.Y, e.Position.Z),
                    Quaternion.Euler(e.Direction.X, e.Direction.Y, e.Direction.Z));
            }
        });
    }

    void Update()
    {
        
    }

    public void Login()
    {
        
    }

    public void EnterGame()
    {
        if (hero != null)
        {
            return;
        }

        Proto.GameEnterRequest request = new()
        {
            CharacterId = 0,
        };
        NetClient.Send(request);
    }
}
