using Kirara;
using Proto;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class GameObjectManager : Singleton<GameObjectManager>
    {
        public void CreateMonster(NMonster nMonster)
        {
            var monsterRoot = Object.Instantiate(Resources.Load<GameObject>("Prefabs/MonsterRoot"));

            int tid = nMonster.Tid;
            Object.Instantiate(
                Resources.Load<GameObject>(DefineManager.Instance.TIDToUnitDefine[tid].Resource),
                monsterRoot.transform);

            EntityManager.Instance.entityIdToGO.Add(nMonster.NEntity.EntityId, monsterRoot);

            monsterRoot.name = $"Monster {nMonster.Name} {nMonster.NEntity.EntityId}";
            monsterRoot.transform.position = nMonster.NEntity.Position.Native();

            Object.DontDestroyOnLoad(monsterRoot);
        }

        public void CreateCharacterObject(NCharacter nCharacter)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/diona");
            var other = Object.Instantiate(prefab);
            EntityManager.Instance.entityIdToGO.Add(nCharacter.NEntity.EntityId, other);

            other.name = $"Character Other EntityId={nCharacter.NEntity.EntityId}";
            other.layer = LayerMask.NameToLayer("Actor");

            var gameEntity = other.GetComponent<GameEntity>();
            if (gameEntity != null)
            {
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
            var playerRoot = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerRoot"));

            Object.Instantiate(Resources.Load<GameObject>("Prefabs/kirara"), playerRoot.transform);

            EntityManager.Instance.entityIdToGO.Add(nCharacter.NEntity.EntityId, playerRoot);

            playerRoot.name = $"Character Player EntityId={nCharacter.NEntity.EntityId}";
            playerRoot.layer = LayerMask.NameToLayer("Actor");

            var playerEntity = playerRoot.GetComponent<PlayerEntity>();
            if (playerEntity != null)
            {
                playerEntity.Set(nCharacter.NEntity);
                playerEntity.StartSendSyncRequestAsync().Forget();
            }

            var textPrefab = Resources.Load<Transform>("Prefabs/WorldHeadText");
            var text = Object.Instantiate(textPrefab, GameManager.Instance.worldCanvas.transform);
            text.GetComponent<WorldHeadText>().follow = playerRoot.transform;
            text.GetComponent<Text>().text = nCharacter.Name;

            Object.DontDestroyOnLoad(playerRoot);
        }
    }
}