using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WheelSpin
{
    public class SpinItem : UIItem
    {
        [SerializeField] private Sprite _effectImage;
        [SerializeField] private Sprite _bombImage;   // flashed behind the bomb when it pops
        [SerializeField] private Color _rewardGlow = new Color(1f, 0.85f, 0.4f);   // replaced per zone by WheelView
        [SerializeField] private Color _bombGlow = new Color(1f, 0.35f, 0.15f);
        [SerializeField] private float _glowSize = 200f;

        public bool IsBomb;

        // Bomb fuse: each red blink (there and back) a little quicker than the last; the pop comes right after
        static readonly float[] BlinkDurations = { 0.5f, 0.36f, 0.26f, 0.2f };
        public static readonly float FuseDuration = System.Linq.Enumerable.Sum(BlinkDurations);

        Image _glow;

        public void SetGlowColor(Color color) => _rewardGlow = color;

        protected override Tween Effect() => IsBomb ? BombEffect() : RewardEffect();

        // Punch plus a warm glow burst
        Tween RewardEffect()
        {
            Image glow = ResetGlow(_effectImage, _rewardGlow);
            return DOTween.Sequence()
                .Insert(0f, transform.DOPunchScale(Vector3.one * 0.25f, 0.4f, 8, 0.6f))
                .Insert(0f, glow.DOFade(1f, 0.08f))
                .Insert(0f, glow.transform.DOScale(1.8f, 0.5f).SetEase(Ease.OutQuad))
                .Insert(0.08f, glow.DOFade(0f, 0.42f).SetEase(Ease.InQuad))
                .SetLink(gameObject);
        }

        Tween BombEffect()
        {
            Image glow = ResetGlow(_bombImage, _bombGlow);
            Transform icon = Icon.transform;
            Sequence seq = DOTween.Sequence();

            float fuse = 0f;
            foreach (float blink in BlinkDurations)
            {
                seq.Insert(fuse, Icon.DOColor(Color.red, blink / 2f).SetLoops(2, LoopType.Yoyo));
                fuse += blink;
            }

            return seq
                .Insert(0f, icon.DOScale(1.3f, fuse).SetEase(Ease.InQuad))
                .Insert(fuse, icon.DOScale(1f, 0.08f))
                .Insert(fuse, glow.DOFade(1f, 0.05f))
                .Insert(fuse, glow.transform.DOScale(2.5f, 0.45f).SetEase(Ease.OutCubic))
                .Insert(fuse + 0.05f, glow.DOFade(0f, 0.4f).SetEase(Ease.InQuad))
                .Insert(fuse, ((RectTransform)transform).DOShakeAnchorPos(0.4f, 12f, 30))
                .SetLink(gameObject);
        }

        Image ResetGlow(Sprite sprite, Color color)
        {
            if (_glow == null)
            {
                _glow = new GameObject("ui_image_slot_glow", typeof(RectTransform), typeof(Image), typeof(LayoutElement))
                    .GetComponent<Image>();
                _glow.GetComponent<LayoutElement>().ignoreLayout = true;
                _glow.transform.SetParent(transform, false);
                _glow.transform.SetAsFirstSibling();
                _glow.rectTransform.sizeDelta = Vector2.one * _glowSize;
                _glow.preserveAspect = true;
                _glow.raycastTarget = false;
            }

            _glow.sprite = sprite;

            color.a = 0f;
            _glow.color = color;
            _glow.transform.localScale = Vector3.one * 0.6f;
            return _glow;
        }

    #if UNITY_EDITOR
        void OnValidate()
        {
            if (_effectImage == null) Debug.LogError($"{name}: effect image is not assigned!", this);
            if (_bombImage == null) Debug.LogError($"{name}: bomb image is not assigned!", this);
        }
    #endif
    }
}
