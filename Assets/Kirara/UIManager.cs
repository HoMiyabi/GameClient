using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kirara
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public Transform canvas;

        protected override void Awake()
        {
            base.Awake();

            SceneManager.sceneLoaded += (scene, loadMode) =>
            {
                canvas = GameObject.Find("Canvas").transform;
            };
        }

        public T NewUI<T>(string prefabPath)
        {
            var prefab = Resources.Load<GameObject>(prefabPath);
            var go = Instantiate(prefab, canvas);
            return go.GetComponent<T>();
        }
    }
}