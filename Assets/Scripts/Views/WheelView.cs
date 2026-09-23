using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WheelView : MonoBehaviour
{
    public enum WheelType {BRONZE, SILVER, GOLD}
    [SerializeField] public WheelSO[] wheels;
    [SerializeField] private Image wheelBase;
    [SerializeField] private Image indicator;
    [SerializeField] private Transform SpinningPart;
    [SerializeField] private RewardView[] slots = new RewardView[8];
    [SerializeField] private RewardIconLibrary iconLibrary;
    [SerializeField] private int _radius = 42;

    [SerializeField] private Button _spinButton;
    public static event System.Action SpinButtonClicked;
    public static event System.Action SpinAnimationComplete;

    void Awake()
    {
        _spinButton.onClick.AddListener(() => SpinButtonClicked?.Invoke());
    }

    void OnEnable()
    {
        GameManager.LevelChanged += DrawWheel;
        RewardsManager.RewardsGenerated += DrawRewards;
        GameManager.SpinStarted += Spin;
    }
    void OnDisable()
    {
        GameManager.LevelChanged -= DrawWheel;
        RewardsManager.RewardsGenerated -= DrawRewards;
        GameManager.SpinStarted -= Spin;
    }

    // Every 30th level is a gold wheel, every 5th a silver one.
    static WheelType TypeForLevel(int level)
    {
        if (level % 30 == 0) return WheelType.GOLD;
        if (level % 5 == 0 || level == 1) return WheelType.SILVER;
        return WheelType.BRONZE;
    }

    void DrawWheel(int level)
    {
        WheelSO wheel = wheels[(int)TypeForLevel(level)];
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
                slots[i].Display(iconLibrary.GetSprite(rewards[i].Id), rewards[i].Amount);
        }
    }
    public void Spin(int position)
    {
        Vector3 rot = new Vector3(0, 0, 360f / slots.Length * position + 360 * 3);
        _spinButton.interactable = false;
        SpinningPart.DORotate(rot, 4f, RotateMode.FastBeyond360)
            .OnComplete(() =>
            {
                _spinButton.interactable = true;
                SpinAnimationComplete?.Invoke();
            });
    }
}
