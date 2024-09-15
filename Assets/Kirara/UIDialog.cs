using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Kirara
{
    public class UIDialog : UIBase
    {
        [SerializeField] private Image bgMask;
        [SerializeField] private Transform box;
        [SerializeField] private Text titleText;
        [SerializeField] private Text messageText;
        [SerializeField] private Transform buttonRoot;

        public string Title
        {
            get => titleText.text;
            set => titleText.text = value;
        }

        public string Message
        {
            get => messageText.text;
            set => messageText.text = value;
        }

        public enum ETorchBgMaskBehavior
        {
            None,
            Close,
            PassThrough,
        }

        private ETorchBgMaskBehavior _torchBgMaskBehavior;

        public ETorchBgMaskBehavior TorchBgMaskBehavior
        {
            get => _torchBgMaskBehavior;
            set
            {
                _torchBgMaskBehavior = value;
                switch (value)
                {
                    case ETorchBgMaskBehavior.None:
                    {
                        bgMask.raycastTarget = true;
                        var bgMaskButton = bgMask.GetComponent<Button>();
                        bgMaskButton.onClick.RemoveAllListeners();
                        break;
                    }
                    case ETorchBgMaskBehavior.Close:
                    {
                        bgMask.raycastTarget = true;
                        var bgMaskButton = bgMask.GetComponent<Button>();
                        bgMaskButton.onClick.RemoveAllListeners();

                        bgMaskButton.onClick.AddListener(Close);
                        break;
                    }
                    case ETorchBgMaskBehavior.PassThrough:
                    {
                        bgMask.raycastTarget = false;
                        var bgMaskButton = bgMask.GetComponent<Button>();
                        bgMaskButton.onClick.RemoveAllListeners();
                        break;
                    }
                }
            }
        }

        public static UIDialog New(string title, string message)
        {
            var self = UIManager.Instance.NewUI<UIDialog>(UIPrefab.UIDialog);

            self.Title = title;
            self.Message = message;
            self.TorchBgMaskBehavior = ETorchBgMaskBehavior.None;

            return self;
        }

        public UIDialog AddButton(Transform button)
        {
            button.SetParent(buttonRoot);
            return this;
        }

        public override void Show()
        {
            box.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            box.DOScale(1f, 0.1f);

            var canvasGroup = box.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.DOFade(1f, 0.1f);

            float curAlpha = bgMask.color.a;
            bgMask.color = new Color(bgMask.color.r, bgMask.color.g, bgMask.color.b, 0f);
            bgMask.DOFade(curAlpha, 0.1f);
        }

        public override void Hide() => gameObject.SetActive(false);

        public override void Close()
        {
            var canvasGroup = box.GetComponent<CanvasGroup>();

            var s = DOTween.Sequence();
            s.Insert(0, box.DOScale(0.7f, 0.1f));
            s.Insert(0, canvasGroup.DOFade(0f, 0.1f));
            s.Insert(0, bgMask.DOFade(0f, 0.1f));
            s.onComplete += () =>
            {
                base.Close();
            };
        }
    }
}