using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Threading.Tasks;
public class Wheel : MonoBehaviour
{
    [SerializeField] private Image wheelBase;
    [SerializeField] private Image indicator;
    [SerializeField] private Image[] sliceRenderers = new Image[8];
    [SerializeField] private int _radius = 42;

    void Start()
    {
        ArrangeSlices();
    }
    public void Draw(WheelSO wheelType, RewardData[] rewards)
    {
        wheelBase.sprite = wheelType.WheelBase;
        indicator.sprite = wheelType.Indicator;
        for (int i = 0; i < sliceRenderers.Length; i++)
            sliceRenderers[i].sprite = i < rewards.Length ? rewards[i]?.sprite : null;
    }
    public async Task Spin(int position)
    {
        Vector3 rot = new Vector3(0, 0, 360f / sliceRenderers.Length * position + 360 * 3);
        await transform.DORotate(rot, 4f, RotateMode.FastBeyond360)
        .AsyncWaitForCompletion();
    }

    private void ArrangeSlices()
    {
        float step = 360f / sliceRenderers.Length;
        // TODO radius should be calculated
        for (int i = 0; i < sliceRenderers.Length; i++)
        {
            float angle = i * step; // + whatever offset aligns slot 0 with your indicator
            float rad = angle * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)) * _radius;
            if (sliceRenderers[i] == null)
        {
            Debug.LogError($"sliceRenderers[{i}] is null on {name}", this);
            continue;
        }
            var rt = sliceRenderers[i].rectTransform;
            rt.anchoredPosition = pos;
            rt.localRotation = Quaternion.Euler(0, 0, -angle); // only if icons should rotate radially — optional
        }
    }
}
