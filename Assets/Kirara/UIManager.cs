using UnityEngine;

namespace Kirara
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public Transform canvas;

        public T NewUI<T>(string prefabPath)
        {
            var prefab = Resources.Load<GameObject>(prefabPath);
            var go = Instantiate(prefab, canvas);
            return go.GetComponent<T>();
        }
    }
}