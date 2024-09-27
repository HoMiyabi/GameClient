using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kirara;
using Manager;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Proto;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class NetStart : MonoSingleton<NetStart>
{
    public List<GameObject> keepAlive;

    [Header("服务器信息")]
    public string host = "127.0.0.1";
    public int port = 32510;

    [Header("登录参数")]
    public InputField usernameInput;
    public InputField passwordInput;

    public Button playBtn;
    public Button connectBtn;
    public Text networkLatencyText;

    public Transform canvas;

    protected override void Awake()
    {
        base.Awake();


        // playBtn.onClick.AddListener(EnterGame);
        connectBtn.onClick.AddListener(Connect);

        foreach (GameObject go in keepAlive)
        {
            DontDestroyOnLoad(go);
        }
    }

    private void Connect()
    {
        NetClient.ConnectToServer(host, port);

        // 心跳包任务，每秒1次
        SendHeartBeatMessage().Forget();
    }

    void Start()
    {
        // Debug.Log($"MainThreadId={Thread.CurrentThread.ManagedThreadId}");
        Application.targetFrameRate = 90;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Actor"), LayerMask.NameToLayer("Actor"), true);

        MessageRouter.Instance.Subscribe<GameEnterResponse>(OnGameEnterResponse);
        MessageRouter.Instance.Subscribe<SpaceCharactersEnterResponse>(OnSpaceCharactersEnterResponse);
        MessageRouter.Instance.Subscribe<SpaceEntitySyncResponse>(OnSpaceEntitySyncResponse);
        MessageRouter.Instance.Subscribe<HeartBeatResponse>(OnHeartBeatResponse);
        MessageRouter.Instance.Subscribe<SpaceCharacterLeaveResponse>(OnSpaceCharacterLeaveResponse);

        SceneManager.LoadScene("LoginScene");

        Connect();
    }

    /// <summary>
    /// 有角色离开地图
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnSpaceCharacterLeaveResponse(Connection sender, SpaceCharacterLeaveResponse message)
    {
        int entityId = message.EntityId;
        MainThread.Instance.Enqueue(() =>
        {
            if (EntityManager.Instance.entityIdToGO.Remove(entityId, out var go))
            {
                Destroy(go);
            }
            else
            {
                Debug.LogWarning("entityIdToGO.Remove(entityId, out var go)");
            }
        });
    }


    // todo))
    // 万一response对应的是上次的request，发了两次request才回复，时间有问题
    // todo))
    // 第一次延迟好大
    private void OnHeartBeatResponse(Connection sender, HeartBeatResponse message)
    {
        long ms = heartBeatStopwatch.ElapsedMilliseconds;
        MainThread.Instance.Enqueue(() =>
        {
            networkLatencyText.text = $"{ms} ms";
            networkLatencyText.color = (ms < 100) ? Color.green : Color.red;
        });
    }

    private Stopwatch heartBeatStopwatch = new();

    private async UniTaskVoid SendHeartBeatMessage()
    {
        HeartBeatRequest request = new();

        while (true)
        {
            await UniTask.WaitForSeconds(1.0f);
            heartBeatStopwatch.Restart();
            NetClient.Send(request);
        }
    }

    // 收到角色的同步信息
    private void OnSpaceEntitySyncResponse(Connection sender, SpaceEntitySyncResponse message)
    {
        var entity = message.EntitySync.Entity;
        MainThread.Instance.Enqueue(() =>
        {
            if (EntityManager.Instance.entityIdToGO.TryGetValue(entity.EntityId, out var go))
            {
                var gameEntity = go.GetComponent<GameEntity>();
                gameEntity.SetFromProto(entity);
            }
            else
            {
                Debug.LogWarning($"entityIdToGO.TryGetValue(entity.Id, out var go) id {entity.EntityId}");
            }
        });
    }

    // 加入游戏的响应结果 Entity肯定是自己
    private void OnGameEnterResponse(Connection sender, GameEnterResponse message)
    {
        Debug.Log("加入游戏的响应结果:" + message.Success);
        if (!message.Success)
        {
            Debug.LogWarning("进入失败");
        }

        Debug.Log("角色信息:" + message);

        var character = message.Character;
        var entity = message.Character.NEntity;
        MainThread.Instance.Enqueue(() =>
        {
            var prefab = Resources.Load<GameObject>("Prefabs/DogPBR");

            var hero = Instantiate(prefab);
            EntityManager.Instance.entityIdToGO.Add(entity.EntityId, hero);

            hero.name = $"Character Player EntityId={entity.EntityId}";
            hero.layer = LayerMask.NameToLayer("Actor");

            var gameEntity = hero.GetComponent<GameEntity>();
            if (gameEntity != null)
            {
                gameEntity.isMine = true;
                gameEntity.SetFromProto(entity);
                gameEntity.SyncToTransform();
                gameEntity.SyncRequestAsync().Forget();
            }

            var tpCameraController = hero.AddComponent<TPCameraController>();
            tpCameraController.offset = new Vector3(0, 1, 0);
            hero.AddComponent<PlayerController>();

            DontDestroyOnLoad(hero);

            SceneManager.LoadScene(DefineManager.Instance.spaceDefineDict[character.SpaceId].Resource);
        });
    }

    // 当有角色进入地图的通知 肯定不是自己
    private void OnSpaceCharactersEnterResponse(Connection conn, SpaceCharactersEnterResponse message)
    {
        Debug.Log("角色进入地图 " + message);
        var characters = message.Characters;

        MainThread.Instance.Enqueue(() =>
        {
            foreach (var nCharacter in characters)
            {
                GameObjectManager.Instance.CreateCharacterObject(nCharacter);
            }
        });
    }

    private void OnApplicationQuit()
    {
        NetClient.Close();
    }
}
