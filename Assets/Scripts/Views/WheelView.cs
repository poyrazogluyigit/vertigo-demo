using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.Assertions;
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

    void OnEnable()
    {
        GameManager.LevelChanged += DrawWheel;
        RewardsManager.RewardsGenerated += DrawRewards;
    }
    void OnDisable()
    {
        GameManager.LevelChanged -= DrawWheel;
        RewardsManager.RewardsGenerated -= DrawRewards;
    }

    void DrawWheel(int wheelType)
    {
        wheelBase.sprite = wheels[wheelType].WheelBase;
        indicator.sprite = wheels[wheelType].Indicator;
    }
    // TODO make sure rewards and slots length matches
    public void DrawRewards(Reward[] rewards)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;
            slots[i].SetReward(rewards[i].Data, rewards[i].Amount);
        }
    }
    public void Spin(int position)
    {
        Vector3 rot = new Vector3(0, 0, 360f / slots.Length * position + 360 * 3);
        transform.DORotate(rot, 4f, RotateMode.FastBeyond360);
    }
}
