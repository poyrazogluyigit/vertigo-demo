using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;

namespace WheelSpin
{
    public class WheelView : View
    {
        [Serializable]
        private struct Skin
        {
            public Sprite Base;
            public Sprite Indicator;
            public Color Rays;   // alpha 0 hides the rays
        }

        [FormerlySerializedAs("wheelBase")]
        [SerializeField] private Image _wheelBase;
        [FormerlySerializedAs("indicator")]
        [SerializeField] private Image _indicator;
        [FormerlySerializedAs("rays")]
        [SerializeField] private Image _rays;
        [SerializeField] private ByZone<Skin> _skins;
        [FormerlySerializedAs("slots")]
        [SerializeField] private SpinItem[] _slots = new SpinItem[WheelContent.SliceCount];

        [SerializeField] private WheelAnimation _wheelAnimation;


        protected override void Subscribe()
        {
            Bus.Subscribe<RewardsReady>(OnWheelReady);
            Bus.Subscribe<SpinStarted>(OnSpinStarted);
        }

        protected override void Unsubscribe()
        {
            Bus.Unsubscribe<RewardsReady>(OnWheelReady);
            Bus.Unsubscribe<SpinStarted>(OnSpinStarted);
        }

        void OnWheelReady(RewardsReady e)
        {
            DrawSkin(_skins.For(e.Zone));
            DrawRewards(e.Rewards);
        }

        void OnSpinStarted(SpinStarted e) => Spin(e.Slot);

        void DrawSkin(Skin skin)
        {
            _wheelBase.sprite = skin.Base;
            _indicator.sprite = skin.Indicator;
            _rays.color = skin.Rays;
            _rays.gameObject.SetActive(skin.Rays.a > 0f);

            // The landing glow shares the rays' tint, so it follows the zone (its alpha is animated anyway)
            foreach (SpinItem slot in _slots)
                if (slot != null) slot.SetGlowColor(skin.Rays);
        }

        void DrawRewards(Reward[] rewards)
        {
            if (rewards.Length != _slots.Length)
                Debug.LogWarning($"WheelView: {rewards.Length} rewards for {_slots.Length} slots", this);

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i] == null)
                    continue;
                bool hasReward = i < rewards.Length;
                _slots[i].gameObject.SetActive(hasReward);
                if (!hasReward)
                    continue;
                _slots[i].IsBomb = rewards[i].Definition.IsBomb;
                _slots[i].Display(rewards[i].Definition.Image, rewards[i].Amount);
            }
        }

        void Spin(int position)
        {
            float landing = 360f / _slots.Length * position;
            _wheelAnimation.GetTween(landing, 360f / _slots.Length).SetLink(gameObject)
                // SpinFinished waits for the landing effect: GameFlow opens the game-over screen on it
                .OnComplete(() => _slots[position].PlayEffect()
                    .OnComplete(() => Bus?.Publish(new SpinFinished())));
        }
    }
}
