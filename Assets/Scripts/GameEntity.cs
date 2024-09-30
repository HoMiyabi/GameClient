using Cysharp.Threading.Tasks;
using Proto;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public int entityId;
    public Vector3 position;
    public Vector3 direction;

    private void Update()
    {
        SyncToTransformLerp();
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

    public void Set(NEntity nEntity)
    {
        entityId = nEntity.EntityId;
        position.Set(nEntity.Position);
        direction.Set(nEntity.Direction);
    }
}
