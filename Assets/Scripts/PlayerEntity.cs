using Cysharp.Threading.Tasks;
using Proto;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public int entityId;
    // public Vector3 position;
    // public Vector3 direction;

    public async UniTaskVoid StartSendSyncRequestAsync()
    {
        var request = new SpaceEntitySyncRequest
        {
            EntitySync = new NEntitySync
            {
                NEntity = new NEntity
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
                request.EntitySync.NEntity.Position.Set(transform.position);
                request.EntitySync.NEntity.Direction.Set(transform.rotation.eulerAngles);
                NetClient.conn.Send(request);
                transform.hasChanged = false;
            }
        }
    }

    public void Set(NEntity nEntity)
    {
        entityId = nEntity.EntityId;
        transform.position = nEntity.Position.Native();
        transform.rotation = Quaternion.Euler(nEntity.Direction.Native());
        Physics.SyncTransforms();
    }
}