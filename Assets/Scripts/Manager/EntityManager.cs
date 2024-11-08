﻿using System.Collections.Generic;
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

        public void SyncEntity(NEntitySync nEntitySync)
        {
            var nEntity = nEntitySync.NEntity;
            if (entityIdToGO.TryGetValue(nEntity.EntityId, out var go))
            {
                var gameEntity = go.GetComponent<GameEntity>();
                if (gameEntity != null)
                {
                    gameEntity.position.Set(nEntity.Position);
                    gameEntity.direction.Set(nEntity.Direction);
                }
                var playerEntity = go.GetComponent<PlayerEntity>();
                if (playerEntity != null)
                {
                    playerEntity.Set(nEntity);
                }
            }
            else
            {
                Debug.LogWarning($"entityIdToGO.TryGetValue(entity.Id, out var go) EntityId={nEntity.EntityId}");
            }
        }
    }
}