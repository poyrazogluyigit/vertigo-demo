using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

namespace WheelSpin
{
    public abstract class UIItem : MonoBehaviour
    {
        [FormerlySerializedAs("icon")]
        [SerializeField] Image _icon;
        [FormerlySerializedAs("amountText")]
        [SerializeField] TextMeshProUGUI _amountText;

        [SerializeField] private float _iconSize = 72f;
        [SerializeField] private float _iconMaxWidth = 110f;
        [SerializeField] private float _iconMaxHeight = 72f;

        protected Image Icon => _icon;
        protected int Amount { get; private set; }
        protected int PreviousAmount { get; private set; }
        public bool AmountChanged => Amount != PreviousAmount;

        public void Display(Sprite sprite, int amount)
        {
            _icon.sprite = sprite;
            FitIcon(sprite);
            PreviousAmount = gameObject.activeSelf ? Amount : 0;
            Amount = amount;
            ShowAmount(amount);
        }

        protected virtual void ShowAmount(int amount)
        {
            _amountText.text = AmountText.Label(amount);
            _amountText.gameObject.SetActive(amount != 0);
        }

        // Sizes the icon's rect to the sprite's own shape, so wide sprites aren't letterboxed
        // inside a square box.
        void FitIcon(Sprite sprite)
        {
            if (sprite == null) return;

            float aspect = sprite.rect.width / sprite.rect.height;
            float width = _iconSize * Mathf.Sqrt(aspect);
            float height = _iconSize / Mathf.Sqrt(aspect);
            float shrink = Mathf.Min(1f, _iconMaxWidth / width, _iconMaxHeight / height);

            _icon.rectTransform.sizeDelta = new Vector2(width, height) * shrink;
        }

        protected abstract Tween Effect();

        public Tween PlayEffect() => Effect();
    }
}
