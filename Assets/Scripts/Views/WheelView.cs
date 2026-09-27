using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
public class WheelView : View
{
    [SerializeField] private Image wheelBase;
    [SerializeField] private Image indicator;
    [SerializeField] private Transform SpinningPart;
    [SerializeField] private RewardView[] slots = new RewardView[WheelSO.SliceCount];


    public void DrawWheel(WheelSO wheel)
    {
        wheelBase.sprite = wheel.WheelBase;
        indicator.sprite = wheel.Indicator;
    }

    public void DrawRewards(Reward[] rewards)
    {
        if (rewards.Length != slots.Length)
            Debug.LogWarning($"WheelView: {rewards.Length} rewards for {slots.Length} slots", this);

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;
            bool hasReward = i < rewards.Length;
            slots[i].gameObject.SetActive(hasReward);
            if (hasReward)
                slots[i].Display(rewards[i].RewardDefn.image, rewards[i].Amount);
        }
    }
    public async Task Spin(int position)
    {
        Vector3 rot = new Vector3(0, 0, 360f / slots.Length * position + 360 * 3);
        await SpinningPart.DORotate(rot, 4f, RotateMode.FastBeyond360)
        .AsyncWaitForCompletion();
    }

}
