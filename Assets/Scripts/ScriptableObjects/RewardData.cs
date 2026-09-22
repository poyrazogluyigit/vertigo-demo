using UnityEngine;

public enum LevelType { Safe, Normal, Super }
public enum RewardTier { T1, T2, T3 }

[System.Flags]
public enum LevelTypeMask
{
    None = 0,
    Safe = 1 << (int)LevelType.Safe,
    Normal = 1 << (int)LevelType.Normal,
    Super = 1 << (int)LevelType.Super,
    All = Safe | Normal | Super,
}

[System.Flags]
public enum RewardTierMask
{
    None = 0,
    T1 = 1 << (int)RewardTier.T1,
    T2 = 1 << (int)RewardTier.T2,
    T3 = 1 << (int)RewardTier.T3,
    All = T1 | T2 | T3,
}

[CreateAssetMenu(menuName = "Wheel/Reward Data")]
public class RewardData : ScriptableObject
{
    public RewardType category;
    public Sprite sprite;

    [Tooltip("Currency rewards (Gold/Cash/Point) scale this by the pool's shared amount curve. Item rewards always grant quantity 1.")]
    public bool isCurrency;
    public float baseValue = 1f;

    [Tooltip("Static pick weight. Ignored for hazard rewards, whose effective weight instead scales with level via the pool's bomb weight curve.")]
    public float weight = 1f;

    [Tooltip("Marks this as the bomb/hazard reward: deterministically forced into Normal-type levels instead of being weighted-picked.")]
    public bool isHazard;

    [Tooltip("Which level types (Safe/Normal/Super) this reward is allowed to appear on.")]
    public LevelTypeMask eligibleLevelTypes = LevelTypeMask.All;

    [Tooltip("Which reward tier(s) this reward belongs to. Eligibility at a given level is cumulative up through the current tier band (T2 band also allows T1 rewards, etc).")]
    public RewardTierMask eligibleTiers = RewardTierMask.T1;

    public bool IsEligibleForLevelType(LevelType levelType)
    {
        return (eligibleLevelTypes & (LevelTypeMask)(1 << (int)levelType)) != 0;
    }
}
