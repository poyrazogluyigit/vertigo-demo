using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
public class WheelView : MonoBehaviour
{
    public enum WheelType {BRONZE, SILVER, GOLD}
    [SerializeField] public WheelSO[] wheels;
    [SerializeField] private Image wheelBase;
    [SerializeField] private Image indicator;
    [SerializeField] private WheelRewardView[] slots = new WheelRewardView[8];
    [SerializeField] private int _radius = 42;

    [SerializeField] private Button _spinButton;
    public static event System.Action SpinButtonClicked;

    public void Draw(WheelType type, RewardData[] rewards, int[] amounts)
    {
        wheelBase.sprite = wheels[(int) type].WheelBase;
        indicator.sprite = wheels[(int) type].Indicator;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;
            RewardData reward = rewards != null && i < rewards.Length ? rewards[i] : null;
            int amount = amounts != null && i < amounts.Length ? amounts[i] : 0;
            slots[i].SetReward(reward, amount);
        }
    }
    public async Task Spin(int position)
    {
        Vector3 rot = new Vector3(0, 0, 360f / slots.Length * position + 360 * 3);
        await transform.DORotate(rot, 4f, RotateMode.FastBeyond360)
        .AsyncWaitForCompletion();
    }
}
