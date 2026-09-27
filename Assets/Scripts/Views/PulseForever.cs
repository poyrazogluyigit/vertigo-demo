using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class PulseForever : MonoBehaviour
{
    [SerializeField] private float growBy = 8f;
    [SerializeField] private float period = 1.2f;

    RectTransform _rt;
    Vector2 _baseSize;

    void Awake()
    {
        _rt = (RectTransform)transform;
        _baseSize = _rt.sizeDelta;
    }

    void Update()
    {
        float t = 0.5f - 0.5f * Mathf.Cos(Time.time * 2f * Mathf.PI / period);
        _rt.sizeDelta = _baseSize + Vector2.one * (growBy * t);
    }

    void OnDisable() => _rt.sizeDelta = _baseSize;
}
