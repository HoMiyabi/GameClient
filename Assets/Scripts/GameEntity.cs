using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Proto;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public int entityId;
    public Vector3 position;
    public Vector3 direction;
    public bool isMine; // 是否自己控制的角色

    private void Start()
    {
        SyncRequestAsync().Forget();
    }

    private async UniTaskVoid SyncRequestAsync()
    {
        var request = new Proto.SpaceEntitySyncRequest()
        {
            EntitySync = new Proto.NEntitySync()
            {
                Entity = new Proto.NEntity()
                {
                    Position = new NVector3(),
                    Direction = new NVector3(),
                }
            }
        };

        while (true)
        {
            await UniTask.WaitForSeconds(0.1f);

            request.EntitySync.Entity.SetFromNative(this);
            NetClient.Send(request);
        }
    }

    private void Update()
    {
        if (isMine)
        {
            // 玩家
            SyncToSelf();
        }
        else
        {
            // 其他角色
            SyncToTransform();
        }
    }

    public void SyncToTransform()
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(direction);
    }

    public void SyncToSelf()
    {
        position = transform.position;
        direction = transform.rotation.eulerAngles;
    }
}
