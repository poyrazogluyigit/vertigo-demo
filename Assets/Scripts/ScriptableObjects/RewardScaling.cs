using UnityEngine;

// How a reward's amount grows with the zone. Rewards without one keep their base amount.
[CreateAssetMenu(menuName = "WheelSpin/Reward Scaling", fileName = "NewRewardScaling")]
public class RewardScaling : ScriptableObject
{
    [Min(1f)] public float multiplierAtMaxLevel = 1f;

    // Geometric: 1x at level 1, multiplierAtMaxLevel x at maxLevel, and it keeps growing past maxLevel
    public float Multiplier(int level, int maxLevel) =>
        Mathf.Pow(multiplierAtMaxLevel, (level - 1f) / (maxLevel - 1f));
}
