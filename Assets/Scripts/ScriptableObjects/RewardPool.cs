using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(menuName = "WheelSpin/Reward Pool")]
public class RewardPool : ScriptableObject
{
    [FormerlySerializedAs("bomb")]
    [SerializeField] private RewardDefinition _bomb;
    [FormerlySerializedAs("maxLevel")]
    [SerializeField, Min(2)] private int _maxLevel = 30;
    public RewardDefinition Bomb => _bomb;

    const int LabelSignificantDigits = 2;

    public int CalculateAmount(RewardDefinition rewardDefn, int level)
    {
        RewardScaling scaling = rewardDefn.Scaling;
        float multiplier = scaling != null ? scaling.Multiplier(level, _maxLevel) : 1f;
        return RoundToSignificant(rewardDefn.BaseAmount * multiplier, LabelSignificantDigits);
    }

    // Keeps labels tidy: 3,041 shows as 3,000
    static int RoundToSignificant(float value, int digits)
    {
        if (value < 1f) return 1;
        float step = Mathf.Max(1f, Mathf.Pow(10f, Mathf.Floor(Mathf.Log10(value)) + 1 - digits));
        return Mathf.RoundToInt(Mathf.Round(value / step) * step);
    }
}
