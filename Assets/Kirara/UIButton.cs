using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kirara
{
    public class UIButton : UIBase
    {
        public Button button;
        public Text uiText;

        public static UIButton New(string text, UnityAction listener = null)
        {
            var self = UIManager.Instance.NewUI<UIButton>(UIPrefab.UIButton);

            self.uiText.text = text;
            if (listener != null)
            {
                self.button.onClick.AddListener(listener);
            }

            return self;
        }
    }
}