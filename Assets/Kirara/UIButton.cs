using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kirara
{
    public class UIButton : MonoBehaviour
    {
        public Button button;
        public Text uiText;

        public static UIButton New(string text, UnityAction listener)
        {
            var self = UIManager.Instance.NewUI<UIButton>(UIPrefab.UIButton);

            self.uiText.text = text;
            self.button.onClick.AddListener(listener);

            return self;
        }
    }
}