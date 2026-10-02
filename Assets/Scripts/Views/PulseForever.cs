using UnityEngine;
using UnityEngine.Serialization;

namespace WheelSpin
{
    [RequireComponent(typeof(RectTransform))]
    public class PulseForever : MonoBehaviour
    {
        [FormerlySerializedAs("growBy")]
        [SerializeField] private float _growBy = 8f;
        [FormerlySerializedAs("period")]
        [SerializeField] private float _period = 1.2f;

        RectTransform _rt;
        Vector2 _baseSize;

        void Awake()
        {
            _rt = (RectTransform)transform;
            _baseSize = _rt.sizeDelta;
        }

        void Update()
        {
            float t = 0.5f - 0.5f * Mathf.Cos(Time.time * 2f * Mathf.PI / _period);
            _rt.sizeDelta = _baseSize + Vector2.one * (_growBy * t);
        }

        void OnDisable() => _rt.sizeDelta = _baseSize;
    }
}
