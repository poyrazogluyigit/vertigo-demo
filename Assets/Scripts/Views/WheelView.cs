using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
public class WheelView : View
{
    [FormerlySerializedAs("wheelBase")]
    [SerializeField] private Image _wheelBase;
    [FormerlySerializedAs("indicator")]
    [SerializeField] private Image _indicator;
    [FormerlySerializedAs("rays")]
    [SerializeField] private Image _rays;
    [FormerlySerializedAs("SpinningPart")]
    [SerializeField] private Transform _spinningPart;
    [FormerlySerializedAs("slots")]
    [SerializeField] private RewardView[] _slots = new RewardView[WheelSO.SliceCount];

    const int ExtraTurns = 3;          // full turns before landing
    const float SpinDuration = 4f;

    public void DrawWheel(WheelSO wheel)
    {
        _wheelBase.sprite = wheel.WheelBase;
        _indicator.sprite = wheel.Indicator;
        _rays.color = wheel.RaysColor;
        _rays.gameObject.SetActive(wheel.RaysColor.a > 0f);
    }

    public void DrawRewards(Reward[] rewards)
    {
        if (rewards.Length != _slots.Length)
            Debug.LogWarning($"WheelView: {rewards.Length} rewards for {_slots.Length} slots", this);

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] == null)
                continue;
            bool hasReward = i < rewards.Length;
            _slots[i].gameObject.SetActive(hasReward);
            if (hasReward)
                _slots[i].Display(rewards[i].RewardDefn.Image, rewards[i].Amount);
        }
    }
    public async Task Spin(int position)
    {
        Vector3 rot = new Vector3(0, 0, 360f / _slots.Length * position + 360f * ExtraTurns);
        await _spinningPart.DORotate(rot, SpinDuration, RotateMode.FastBeyond360)
        .AsyncWaitForCompletion();
    }

}
