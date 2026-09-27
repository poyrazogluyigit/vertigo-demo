using UnityEngine;


[CreateAssetMenu(menuName = "WheelSpin/Reward Pool")]
public class RewardPool : ScriptableObject
{
    [SerializeField] private RewardDefinition bomb;
    [SerializeField, Min(2)] private int maxLevel = 30;
    public RewardDefinition Bomb => bomb;

    public int CalculateAmount(RewardDefinition rewardDefn, int level)
    {
        RewardScaling scaling = rewardDefn.scaling;
        float multiplier = scaling != null ? scaling.Multiplier(level, maxLevel) : 1f;
        return RoundToSignificant(rewardDefn.baseAmount * multiplier, 2);
    }

    // Keeps labels tidy: 3,041 shows as 3,000
    static int RoundToSignificant(float value, int digits)
    {
        if (value < 1f) return 1;
        float step = Mathf.Max(1f, Mathf.Pow(10f, Mathf.Floor(Mathf.Log10(value)) + 1 - digits));
        return Mathf.RoundToInt(Mathf.Round(value / step) * step);
    }
}
