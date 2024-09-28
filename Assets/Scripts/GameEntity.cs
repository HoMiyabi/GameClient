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
                    EntityId = entityId,
                    Position = new NFloat3(),
                    Direction = new NFloat3(),
                },
            },
        };

        while (true)
        {
            await UniTask.WaitForSeconds(0.1f);

            if (transform.hasChanged)
            {
                request.EntitySync.Entity.Position.Set(position);
                request.EntitySync.Entity.Direction.Set(direction);
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

    public void Set(NEntity nEntity)
    {
        entityId = nEntity.EntityId;
        position.Set(nEntity.Position);
        direction.Set(nEntity.Direction);
    }
}
