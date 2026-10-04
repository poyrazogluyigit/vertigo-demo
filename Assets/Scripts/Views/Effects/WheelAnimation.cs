using DG.Tweening;
using UnityEngine;

namespace WheelSpin
{
    public class WheelAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _spinningPart;
        [SerializeField] private Transform _indicator;
        [SerializeField] private Pulse _rays;   

        const float TickAngle = -18f;       // deflection when a peg slips past the indicator
        const float PushZone = 0.35f;       // share of a slice, just before a boundary, in which the peg pushes it
        const float ReleaseDuration = 0.5f;  // snap-back from full deflection

        float _sliceAngle;
        float _indicatorDeflection;
        float _lastAngle;
        float _indicatorDirection = 1f;              

        public Tween GetTween(float degrees, float sliceAngle)
        {
            _sliceAngle = sliceAngle;
            _lastAngle = _spinningPart.eulerAngles.z;
            const int ExtraTurns = 5;
            const float SpinDuration = 4.0f;
            const float overshoot = 18f;   // degrees, wind-up and overrun

            float sweep = Mathf.Repeat(degrees - _spinningPart.eulerAngles.z, 360f) + 360f * ExtraTurns;

            Sequence seq = DOTween.Sequence();
            return seq.Append(Rotate(-overshoot, 0.5f, Ease.OutQuad))
                .Append(Rotate(sweep + 2 * overshoot, SpinDuration, Ease.OutCubic))
                .Append(Rotate(-overshoot, 0.5f, Ease.InOutQuad))
                .OnUpdate(() =>
                {
                    UpdateIndicator();
                    float progress = seq.ElapsedPercentage();
                    _rays.SetIntensity(progress * progress);   // quiet early, hard near the end
                })
                .OnKill(() =>
                {
                    ResetIndicator();
                    _rays.SetIntensity(0f);
                });
        }

        void UpdateIndicator()
        {
            float angle = _spinningPart.eulerAngles.z;
            float moved = Mathf.DeltaAngle(_lastAngle, angle);
            _lastAngle = angle;
            if (Mathf.Abs(moved) > 0.0001f) _indicatorDirection = Mathf.Sign(moved);

            float phase = Mathf.Repeat(angle - _sliceAngle * 0.5f, _sliceAngle) / _sliceAngle;
            float pushed = _indicatorDirection > 0f
                ? Mathf.Clamp01((phase - (1f - PushZone)) / PushZone)
                : -Mathf.Clamp01((PushZone - phase) / PushZone);

            // Being pushed is immediate; letting go is a fast spring-back
            _indicatorDeflection = Mathf.Abs(pushed) > Mathf.Abs(_indicatorDeflection)
                ? pushed
                : Mathf.MoveTowards(_indicatorDeflection, pushed, Time.deltaTime / ReleaseDuration);
            _indicator.localRotation = Quaternion.Euler(0f, 0f, TickAngle * _indicatorDeflection);
        }

        void ResetIndicator()
        {
            _indicatorDeflection = 0f;
            _indicatorDirection = 1f;
            _indicator.localRotation = Quaternion.identity;
        }

        Tween Rotate(float degrees, float duration, Ease ease) =>
            _spinningPart.DORotate(new Vector3(0, 0, degrees), duration, RotateMode.FastBeyond360)
                .SetRelative().SetEase(ease);
    }
}
