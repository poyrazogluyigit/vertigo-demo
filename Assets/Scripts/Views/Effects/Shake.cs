using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WheelSpin
{
    [RequireComponent(typeof(RectTransform))]
    public class Shake : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.4f;
        [SerializeField] private float _strength = 20f;
        [SerializeField] private int _vibrato = 25;
        [SerializeField] private Color _flashColor = new Color(0.78f, 0.09f, 0.11f, 0.35f);   // alpha is the flash's peak

        Image _flash;

        public Tween PlayEffect()
        {
            Image flash = Flash();
            return DOTween.Sequence().SetLink(gameObject)
                .Insert(0f, ((RectTransform)transform).DOShakeAnchorPos(_duration, _strength, _vibrato))
                .Insert(0f, flash.DOFade(_flashColor.a, 0.08f))
                .Insert(0.08f, flash.DOFade(0f, 0.25f));
        }

        Image Flash()
        {
            if (_flash == null)
            {
                _flash = new GameObject("ui_image_fx_flash", typeof(RectTransform), typeof(Image)).GetComponent<Image>();
                _flash.transform.SetParent(GetComponentInParent<Canvas>().rootCanvas.transform, false);
                _flash.rectTransform.anchorMin = Vector2.zero;
                _flash.rectTransform.anchorMax = Vector2.one;
                _flash.rectTransform.sizeDelta = Vector2.zero;
                _flash.raycastTarget = false;
            }
            Color hidden = _flashColor;
            hidden.a = 0f;
            _flash.color = hidden;
            _flash.transform.SetAsLastSibling();
            return _flash;
        }
    }
}
