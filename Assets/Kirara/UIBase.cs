using System;
using UnityEngine;

namespace Kirara
{
    [Serializable]
    public class UIBase : MonoBehaviour
    {
        public enum PanelType
        {
            Cover,
            Exclusive,
        }

        public PanelType panelType = PanelType.Cover;

        public virtual void Open()
        {
            UIManager.Instance.Add(this);
        }

        public virtual void Close()
        {
            UIManager.Instance.Remove(this);
            Destroy(gameObject);
        }

        public virtual void Show()
        {
            transform.position = transform.position - new Vector3(100000f, 100000f, 0);
        }

        public virtual void Hide()
        {
            transform.position = transform.position + new Vector3(100000f, 100000f, 0f);
        }
    }
}