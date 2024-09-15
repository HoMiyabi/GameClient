using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kirara
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public Transform canvas;

        [SerializeField] private List<UIBase> stk;

        public void Add(UIBase ui)
        {
            if (ui.panelType == UIBase.PanelType.Exclusive)
            {
                for (int i = stk.Count - 1; i >= 0; i--)
                {
                    if (stk[i].panelType == UIBase.PanelType.Exclusive)
                    {
                        stk[i].Hide();
                        break;
                    }
                    stk[i].Hide();
                }
            }
            stk.Add(ui);
        }

        public void Remove(UIBase ui)
        {
            int uiIdx = stk.Count - 1;
            while (uiIdx != -1 && stk[uiIdx] != ui)
            {
                uiIdx--;
            }
            if (uiIdx == -1)
            {
                Debug.LogWarning("ui不在stk中");
                return;
            }
            if (ui.panelType == UIBase.PanelType.Exclusive)
            {
                for (int i = uiIdx - 1; i >= 0; i--)
                {
                    if (stk[i].panelType == UIBase.PanelType.Exclusive)
                    {
                        stk[i].Show();
                        break;
                    }
                    stk[i].Show();
                }
            }
            stk.RemoveAt(uiIdx);
        }

        protected override void Awake()
        {
            base.Awake();

            stk ??= new();

            SceneManager.sceneLoaded += (scene, loadMode) =>
            {
                stk.Clear();
                canvas = GameObject.Find("Canvas").transform;
            };
        }

        public T NewUI<T>(string prefabPath) where T : UIBase
        {
            if (canvas == null)
            {
                canvas = GameObject.Find("Canvas").transform;
            }

            var prefab = Resources.Load<GameObject>(prefabPath);
            var go = Instantiate(prefab, canvas);
            return go.GetComponent<T>();
        }

        public UIBase NewUI(string prefabPath)
        {
            if (canvas == null)
            {
                canvas = GameObject.Find("Canvas").transform;
            }

            var prefab = Resources.Load<GameObject>(prefabPath);
            var go = Instantiate(prefab, canvas);
            if (go.TryGetComponent(out UIBase ui))
            {
                return ui;
            }
            else
            {
                Debug.LogWarning("找不到UIBase 自动添加");
                return go.AddComponent<UIBase>();
            }
        }
    }
}