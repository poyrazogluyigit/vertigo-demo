using UnityEngine;
using UnityEngine.Serialization;

// What a reward is worth on a level: its base amount times its scaling curve, rounded to tidy numbers.
[CreateAssetMenu(menuName = "WheelSpin/Reward Pricing")]
public class RewardPricing : ScriptableObject, IRewardPricing
{
    [FormerlySerializedAs("maxLevel")]
    [SerializeField, Min(2)] private int _maxLevel = 30;

    const int LabelSignificantDigits = 2;

    public int AmountFor(RewardDefinition reward, int level)
    {
        RewardScaling scaling = reward.Scaling;
        float multiplier = scaling != null ? scaling.Multiplier(level, _maxLevel) : 1f;
        return RoundToSignificant(reward.BaseAmount * multiplier, LabelSignificantDigits);
    }

    // Keeps labels tidy: 3,041 shows as 3,000
    public static int RoundToSignificant(float value, int digits)
    {
        if (value < 1f) return 1;
        float step = Mathf.Max(1f, Mathf.Pow(10f, Mathf.Floor(Mathf.Log10(value)) + 1 - digits));
        return Mathf.RoundToInt(Mathf.Round(value / step) * step);
    }
}
