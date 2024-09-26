using Kirara;
using Proto;
using UnityEngine;

namespace Manager
{
    public class GameObjectManager : Singleton<GameObjectManager>
    {
        public void CreateCharacterObject(NCharacter nCharacter)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/DogPBR");
            var other = Object.Instantiate(prefab);
            EntityManager.Instance.entityIdToGO.Add(nCharacter.Id, other);

            other.name = $"Other Player {nCharacter.Id}";
            other.layer = LayerMask.NameToLayer("Actor");
            Object.DontDestroyOnLoad(other);
            var gameEntity = other.GetComponent<GameEntity>();
            if (gameEntity != null)
            {
                gameEntity.isMine = false;
                gameEntity.SetFromProto(nCharacter.Entity);
                gameEntity.SyncToTransform();
            }
        }
    }
}