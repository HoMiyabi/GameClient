using Cysharp.Threading.Tasks;
using Proto;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public int entityId;
    public Vector3 position;
    public Vector3 direction;
    public bool isMine; // 是否自己控制的角色

    public async UniTaskVoid SyncRequestAsync()
    {
        var request = new SpaceEntitySyncRequest
        {
            EntitySync = new NEntitySync
            {
                Entity = new NEntity
                {
                    Position = new NInt3(),
                    Direction = new NInt3(),
                },
            },
        };

        while (true)
        {
            await UniTask.WaitForSeconds(0.1f);

            if (transform.hasChanged)
            {
                request.EntitySync.Entity.SetFromNative(this);
                // print(request);
                NetClient.Send(request);
                transform.hasChanged = false;
            }
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
            SyncToTransformLerp();
        }
    }

    public void SyncToTransformLerp()
    {
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 5f);

        Quaternion targetRotation = Quaternion.Euler(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
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
