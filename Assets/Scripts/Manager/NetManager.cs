using Kirara;
using Proto;
using UnityEngine;

namespace Manager
{
    public class NetManager : MonoSingleton<NetManager>
    {
        private void Start()
        {
            MessageRouter.Instance.Subscribe<MonstersEnterSpaceResponse>(OnMonstersEnterSpaceResponse);
        }

        private void OnMonstersEnterSpaceResponse(Connection conn, MonstersEnterSpaceResponse message)
        {
            Debug.Log("野怪进入地图 " + message);
            var nMonsters = message.NMonsters;

            MainThread.Instance.Enqueue(() =>
            {
                foreach (var nMonster in nMonsters)
                {
                    GameObjectManager.Instance.CreateMonster(nMonster);
                }
            });
        }
    }
}