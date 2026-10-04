using UnityEngine;
using UnityEngine.Serialization;

namespace WheelSpin
{
    [RequireComponent(typeof(RectTransform))]
    public class Pulse : MonoBehaviour
    {
        // Size suits compact elements; Alpha suits images that look the same at any scale, like the sunburst rays
        enum Mode { Size, Alpha }

        [SerializeField] private Mode _mode = Mode.Size;
        [SerializeField] private float _growBy = 8f;
        [SerializeField] private float _period = 1.2f;
        [SerializeField] private float _fadeBy = 0.3f;         // Alpha mode: how far the alpha dips at the bottom of a pulse
        [SerializeField] private float _extraStrength = 2f;   // at intensity 1: growBy x (1 + this)
        [SerializeField] private float _extraSpeed = 1.5f;    // at intensity 1: pulses (1 + this) times faster

        RectTransform _rt;
        CanvasRenderer _renderer;
        Vector2 _baseSize;
        float _intensity = 1;   // 0 = idle pulse, 1 = hardest
        float _phase;       // accumulated, so a changing speed never makes the pulse jump

        void Awake()
        {
            _rt = (RectTransform)transform;
            _baseSize = _rt.sizeDelta;
            _renderer = GetComponent<CanvasRenderer>();
        }

        public void SetIntensity(float intensity) => _intensity = Mathf.Clamp01(intensity);

        void Update()
        {
            _phase += Time.deltaTime * (1f + _intensity * _extraSpeed) * 2f * Mathf.PI / _period;
            float t = 0.5f - 0.5f * Mathf.Cos(_phase);
            float strength = 1f + _intensity * _extraStrength;

            if (_mode == Mode.Alpha)
                _renderer.SetAlpha(1f - Mathf.Clamp01(_fadeBy * strength) * t);   // multiplies the Graphic's own colour
            else
                _rt.sizeDelta = _baseSize + Vector2.one * (_growBy * strength * t);
        }

        void OnDisable()
        {
            _rt.sizeDelta = _baseSize;
            if (_renderer != null) _renderer.SetAlpha(1f);
        }
    }
}
