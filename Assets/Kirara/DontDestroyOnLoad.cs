using UnityEngine;

namespace Kirara
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(transform.root);
        }
    }
}