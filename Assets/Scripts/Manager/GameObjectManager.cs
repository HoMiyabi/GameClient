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

            var textPrefab = Resources.Load<Transform>("Prefabs/WorldHeadText");
            var text = Object.Instantiate(textPrefab, GameManager.Instance.worldCanvas.transform);
            text.GetComponent<WorldHeadText>().follow = monsterRoot.transform;
            text.GetComponent<Text>().text = nMonster.Name;

            Object.DontDestroyOnLoad(monsterRoot);
        }

        public void CreateCharacterOther(NCharacter nCharacter)
        {
            var otherRoot = Object.Instantiate(Resources.Load<GameObject>("Prefabs/OtherRoot"));

            var otherPrefabPath = "Prefabs/diona";

            Object.Instantiate(Resources.Load<GameObject>(otherPrefabPath), otherRoot.transform);
            EntityManager.Instance.entityIdToGO.Add(nCharacter.NEntity.EntityId, otherRoot);

            otherRoot.name = $"Character Other EntityId={nCharacter.NEntity.EntityId}";
            otherRoot.layer = LayerMask.NameToLayer("Actor");

            if (otherRoot.TryGetComponent<GameEntity>(out var gameEntity))
            {
                gameEntity.Set(nCharacter.NEntity);
                gameEntity.SyncToTransform();
            }

            var textPrefab = Resources.Load<Transform>("Prefabs/WorldHeadText");
            var text = Object.Instantiate(textPrefab, GameManager.Instance.worldCanvas.transform);
            text.GetComponent<WorldHeadText>().follow = otherRoot.transform;
            text.GetComponent<Text>().text = nCharacter.Name;

            Object.DontDestroyOnLoad(otherRoot);
        }

        public void CreateCharacterPlayer(NCharacter nCharacter)
        {
            var playerRoot = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerRoot"));

            string playerPrefabPath = "Prefabs/kirara";
            Object.Instantiate(Resources.Load<GameObject>(playerPrefabPath), playerRoot.transform);

            EntityManager.Instance.entityIdToGO.Add(nCharacter.NEntity.EntityId, playerRoot);

            playerRoot.name = $"Character Player EntityId={nCharacter.NEntity.EntityId}";
            playerRoot.layer = LayerMask.NameToLayer("Actor");

            if (playerRoot.TryGetComponent<PlayerEntity>(out var playerEntity))
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