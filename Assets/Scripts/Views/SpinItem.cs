using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WheelSpin
{
    public class SpinItem : MonoBehaviour
    {
        [FormerlySerializedAs("icon")]
        [SerializeField] private Image _icon;
        [FormerlySerializedAs("amountText")]
        [SerializeField] private TextMeshProUGUI _amountText;

        [SerializeField] private float _iconSize = 72f;        
        [SerializeField] private float _iconMaxWidth = 110f;
        [SerializeField] private float _iconMaxHeight = 72f;

        public void Display(Sprite sprite, int amount)
        {
            _icon.sprite = sprite;
            FitIcon(sprite);
            _amountText.text = AmountText.Label(amount);
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
    }
}
