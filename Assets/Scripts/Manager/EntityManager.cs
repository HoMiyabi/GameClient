using System.Collections.Generic;
using Kirara;
using Proto;
using UnityEngine;

namespace Manager
{
    public class EntityManager : Singleton<EntityManager>
    {
        public readonly Dictionary<int, GameObject> entityIdToGO = new();

        public void DestroyEntity(int entityId)
        {
            if (entityIdToGO.Remove(entityId, out var go))
            {
                Object.Destroy(go);
            }
            else
            {
                Debug.LogWarning($"entityIdToGO找不到 EntityId={entityId}");
            }
        }

        public void SyncEntity(NEntity nEntity)
        {
            if (entityIdToGO.TryGetValue(nEntity.EntityId, out var go))
            {
                var gameEntity = go.GetComponent<GameEntity>();
                gameEntity.position.Set(nEntity.Position);
                gameEntity.direction.Set(nEntity.Direction);
            }
            else
            {
                Debug.LogWarning($"entityIdToGO.TryGetValue(entity.Id, out var go) EntityId={nEntity.EntityId}");
            }
        }
    }
}