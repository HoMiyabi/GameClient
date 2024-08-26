using Summer.Network;
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using UnityEngine.UI;
using Proto;

public class NetStart : MonoBehaviour
{
    [Header("服务器信息")]
    public string host = "127.0.0.1";
    public int port = 32510;

    [Header("登录参数")]
    public InputField usernameInput;
    public InputField passwordInput;

    public Button playBtn;
    public Button connectBtn;
    public Text networkLatencyText;

    private GameObject hero; // 当前的角色

    private Dictionary<int, GameObject> entityIdToGO = new();

    private void Awake()
    {
        playBtn.onClick.AddListener(EnterGame);
        connectBtn.onClick.AddListener(Connect);
    }

    private void Connect()
    {
        NetClient.ConnectToServer(host, port);

        // 心跳包任务，每秒1次
        SendHeartBeatMessage().Forget();
    }

    void Start()
    {
        MessageRouter.Instance.Subscribe<GameEnterResponse>(OnGameEnterResponse);
        MessageRouter.Instance.Subscribe<SpaceCharactersEnterResponse>(OnSpaceCharactersEnterResponse);
        MessageRouter.Instance.Subscribe<SpaceEntitySyncResponse>(OnSpaceEntitySyncResponse);
        MessageRouter.Instance.Subscribe<HeartBeatResponse>(OnHeartBeatResponse);
    }


    // todo))
    // 万一response对应的是上次的request，发了两次request才回复，时间有问题
    // todo))
    // 第一次延迟好大
    private void OnHeartBeatResponse(Connection sender, HeartBeatResponse message)
    {
        var now = DateTime.UtcNow;
        var span = now - lastHeartBeatTime;
        int dt = (int)Math.Round(span.TotalMilliseconds);
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            networkLatencyText.text = dt + " ms";
            networkLatencyText.color = (dt < 100) ? Color.green : Color.red;
        });
    }

    private DateTime lastHeartBeatTime = DateTime.MinValue;

    private async UniTaskVoid SendHeartBeatMessage()
    {
        HeartBeatRequest request = new();

        while (true)
        {
            await UniTask.WaitForSeconds(1.0f);

            lastHeartBeatTime = DateTime.UtcNow;
            NetClient.Send(request);
        }
    }

    // 收到角色的同步信息
    private void OnSpaceEntitySyncResponse(Connection sender, Proto.SpaceEntitySyncResponse message)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var entity = message.EntitySync.Entity;
            // Debug.Log("收到同步信息 " + entity);

            if (entityIdToGO.TryGetValue(entity.Id, out var go))
            {
                var gameEntity = go.GetComponent<GameEntity>();
                gameEntity.SetFromProto(entity);
                gameEntity.SyncToTransform();
            }
            else
            {
                Debug.LogWarning("entityIdToGO.TryGetValue(entity.Id, out var go)");
            }
        });
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
                entityIdToGO.Add(entity.Id, hero);

                hero.name = $"Character Player {entity.Id}";

                var gameEntity = hero.GetComponent<GameEntity>();
                if (gameEntity != null)
                {
                    gameEntity.isMine = true;
                    gameEntity.SetFromProto(entity);
                    gameEntity.SyncToTransform();
                }

                hero.AddComponent<PlayerController>();
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
            foreach (var entity in entities)
            {
                var other = Instantiate(prefab);
                entityIdToGO.Add(entity.Id, other);

                other.name = $"Other Player {entity.Id}";
                var gameEntity = other.GetComponent<GameEntity>();
                if (gameEntity != null)
                {
                    gameEntity.isMine = false;
                    gameEntity.SetFromProto(entity);
                    gameEntity.SyncToTransform();
                }
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

        GameEnterRequest request = new()
        {
            CharacterId = 0,
        };
        NetClient.Send(request);
    }

    private void OnApplicationQuit()
    {
        NetClient.Close();
    }
}
