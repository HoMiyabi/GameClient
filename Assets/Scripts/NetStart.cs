using System;
using Cysharp.Threading.Tasks;
using Kirara;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Proto;
using UnityEngine.SceneManagement;

public class NetStart : MonoSingleton<NetStart>
{
    [Header("服务器信息")]
    public string host = "127.0.0.1";
    public int port = 32510;

    public Text networkLatencyText;

    public Transform canvas;

    public GameInfo gameInfo;

    private void Connect()
    {
        NetClient.ConnectToServer(host, port);

        // 心跳包任务，每秒1次
        SendHeartBeatMessage().Forget();
    }

    void Start()
    {
        // Debug.Log($"MainThreadId={Thread.CurrentThread.ManagedThreadId}");
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Actor"), LayerMask.NameToLayer("Actor"), true);

        MessageRouter.Instance.Subscribe<GameEnterResponse>(OnGameEnterResponse);
        MessageRouter.Instance.Subscribe<SpaceCharactersEnterResponse>(OnSpaceCharactersEnterResponse);
        MessageRouter.Instance.Subscribe<SpaceEntitySyncResponse>(OnSpaceEntitySyncResponse);
        MessageRouter.Instance.Subscribe<HeartBeatResponse>(OnHeartBeatResponse);
        MessageRouter.Instance.Subscribe<SpaceCharacterLeaveResponse>(OnSpaceCharacterLeaveResponse);

        Connect();
    }

    // todo))
    // 万一response对应的是上次的request，发了两次request才回复，时间有问题
    // todo))
    // 第一次延迟好大
    private void OnHeartBeatResponse(Connection sender, HeartBeatResponse message)
    {
        long ms = heartBeatStopwatch.ElapsedMilliseconds;
        // Debug.Log($"接收 {DateTime.UtcNow:HH:mm:ss.fff} {ms}ms");
        MainThread.Instance.Enqueue(() =>
        {
            gameInfo.SetNetworkLatency(ms);
        });
    }

    private System.Diagnostics.Stopwatch heartBeatStopwatch = new();

    private async UniTaskVoid SendHeartBeatMessage()
    {
        byte[] requestBytes = Connection.GetSendBytes(new HeartBeatRequest());

        // todo)) 断开后就不要发了
        while (true)
        {
            await UniTask.WaitForSeconds(1.0f);
            heartBeatStopwatch.Restart();
            // Debug.Log($"发送 {DateTime.UtcNow:HH:mm:ss.fff}");
            NetClient.conn.SendBytes(requestBytes);
        }
    }

    // 收到角色的同步信息
    private void OnSpaceEntitySyncResponse(Connection sender, SpaceEntitySyncResponse message)
    {
        var entitySync = message.EntitySync;
        MainThread.Instance.Enqueue(() => EntityManager.Instance.SyncEntity(entitySync));
    }

    // 加入游戏的响应结果 Entity肯定是自己
    private void OnGameEnterResponse(Connection sender, GameEnterResponse message)
    {
        Debug.Log("加入游戏的响应结果:" + message.Success);
        if (!message.Success)
        {
            Debug.LogWarning("进入失败");
        }

        Debug.Log("自己进入地图 " + message);

        var character = message.NCharacter;
        MainThread.Instance.Enqueue(() =>
        {
            GameObjectManager.Instance.CreatePlayer(character);
            SceneManager.LoadScene(DefineManager.Instance.SIDToSpaceDefine[character.SpaceId].Resource);
        });
    }

    // 当有角色进入地图的通知 肯定不是自己
    private void OnSpaceCharactersEnterResponse(Connection conn, SpaceCharactersEnterResponse message)
    {
        Debug.Log("其他人进入地图 " + message);
        var nCharacters = message.NCharacters;

        MainThread.Instance.Enqueue(() =>
        {
            foreach (var nCharacter in nCharacters)
            {
                GameObjectManager.Instance.CreateCharacterObject(nCharacter);
            }
        });
    }

    /// <summary>
    /// 有角色离开地图
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="message"></param>
    private void OnSpaceCharacterLeaveResponse(Connection sender, SpaceCharacterLeaveResponse message)
    {
        int entityId = message.EntityId;
        MainThread.Instance.Enqueue(() => EntityManager.Instance.DestroyEntity(entityId));
    }

    private void OnApplicationQuit()
    {
        NetClient.Close();
    }
}
