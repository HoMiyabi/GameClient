using Kirara;
using Proto;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class GameObjectManager : Singleton<GameObjectManager>
    {
        public void CreateCharacterObject(NCharacter nCharacter)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/DogPBR");
            var other = Object.Instantiate(prefab);
            EntityManager.Instance.entityIdToGO.Add(nCharacter.NEntity.EntityId, other);

            other.name = $"Other Player EntityId={nCharacter.NEntity.EntityId}";
            other.layer = LayerMask.NameToLayer("Actor");

            var gameEntity = other.GetComponent<GameEntity>();
            if (gameEntity != null)
            {
                gameEntity.isMine = false;
                gameEntity.Set(nCharacter.NEntity);
                gameEntity.SyncToTransform();
            }

            var textPrefab = Resources.Load<Transform>("Prefabs/WorldHeadText");
            var text = Object.Instantiate(textPrefab, GameManager.Instance.worldCanvas.transform);
            text.GetComponent<WorldHeadText>().follow = other.transform;
            text.GetComponent<Text>().text = nCharacter.Name;

            Object.DontDestroyOnLoad(other);
        }

        public void CreatePlayer(NCharacter nCharacter)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/DogPBRPlayer");

            var hero = Object.Instantiate(prefab);
            EntityManager.Instance.entityIdToGO.Add(nCharacter.NEntity.EntityId, hero);

            hero.name = $"Character Player EntityId={nCharacter.NEntity.EntityId}";
            hero.layer = LayerMask.NameToLayer("Actor");

            var gameEntity = hero.GetComponent<GameEntity>();
            if (gameEntity != null)
            {
                gameEntity.isMine = true;
                gameEntity.Set(nCharacter.NEntity);
                gameEntity.SyncToTransform();
                gameEntity.SyncRequestAsync().Forget();
            }

            var textPrefab = Resources.Load<Transform>("Prefabs/WorldHeadText");
            var text = Object.Instantiate(textPrefab, GameManager.Instance.worldCanvas.transform);
            text.GetComponent<WorldHeadText>().follow = hero.transform;
            text.GetComponent<Text>().text = nCharacter.Name;

            Object.DontDestroyOnLoad(hero);
        }
    }
}