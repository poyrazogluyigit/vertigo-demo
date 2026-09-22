using System.Collections.Generic;
using UnityEngine;

public enum RewardType { WEAPON, COSMETIC, CHEST, GOLD, CASH, POINT }

[CreateAssetMenu(menuName = "Wheel/Reward Pool")]
public class RewardPool : ScriptableObject
{
    [SerializeField] private List<RewardData> rewards = new List<RewardData>();

    [Tooltip("Game-wide amount scaling for currency rewards, evaluated at level / maxLevel.")]
    [SerializeField] private AnimationCurve amountCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

    [Tooltip("Game-wide weight scaling for the hazard (bomb) reward, evaluated at level / maxLevel. Only applies to rewards with isHazard set.")]
    [SerializeField] private AnimationCurve hazardWeightCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

    [SerializeField] private int maxLevel = 30;

    public int GetAmount(RewardData reward, int level)
    {
        if (reward == null || !reward.isCurrency)
            return 1;

        float t = maxLevel > 0 ? (float)level / maxLevel : 0f;
        return Mathf.RoundToInt(reward.baseValue * amountCurve.Evaluate(t));
    }

    /// <summary>
    /// Weighted pick without replacement, filtered to rewards eligible for the given level type
    /// and cumulatively eligible up through the given tier band. Hazard rewards are never picked
    /// here — they're forced in deterministically by the caller instead.
    /// </summary>
    public List<RewardData> PickWeighted(int level, LevelType levelType, RewardTier tierBand, int count, IEnumerable<RewardData> exclude = null)
    {
        return PickWeightedInternal(level, levelType, requiredTier: null, tierBand, count, exclude);
    }

    /// <summary>
    /// Weighted pick without replacement restricted to rewards that explicitly belong to
    /// requiredTier (not the cumulative band) — used to force a guaranteed tier-3 pick on Super levels.
    /// </summary>
    public List<RewardData> PickWeightedStrictTier(int level, LevelType levelType, RewardTier requiredTier, int count, IEnumerable<RewardData> exclude = null)
    {
        return PickWeightedInternal(level, levelType, requiredTier, requiredTier, count, exclude);
    }

    private List<RewardData> PickWeightedInternal(int level, LevelType levelType, RewardTier? requiredTier, RewardTier tierBand, int count, IEnumerable<RewardData> exclude)
    {
        var excludeSet = exclude != null ? new HashSet<RewardData>(exclude) : null;

        var eligible = new List<RewardData>();
        foreach (var reward in rewards)
        {
            if (reward == null || reward.isHazard)
                continue;
            if (excludeSet != null && excludeSet.Contains(reward))
                continue;
            if (!reward.IsEligibleForLevelType(levelType))
                continue;
            if (requiredTier.HasValue)
            {
                if ((reward.eligibleTiers & (RewardTierMask)(1 << (int)requiredTier.Value)) == 0)
                    continue;
            }
            else if (!IsTierUsable(reward.eligibleTiers, tierBand))
            {
                continue;
            }
            eligible.Add(reward);
        }

        var result = new List<RewardData>(Mathf.Min(count, eligible.Count));
        for (int picks = 0; picks < count && eligible.Count > 0; picks++)
        {
            float totalWeight = 0f;
            foreach (var reward in eligible)
                totalWeight += Mathf.Max(0f, reward.weight);

            int chosenIndex;
            if (totalWeight <= 0f)
            {
                chosenIndex = Random.Range(0, eligible.Count);
            }
            else
            {
                float roll = Random.Range(0f, totalWeight);
                float cumulative = 0f;
                chosenIndex = eligible.Count - 1;
                for (int i = 0; i < eligible.Count; i++)
                {
                    cumulative += Mathf.Max(0f, eligible[i].weight);
                    if (roll <= cumulative)
                    {
                        chosenIndex = i;
                        break;
                    }
                }
            }

            result.Add(eligible[chosenIndex]);
            eligible.RemoveAt(chosenIndex);
        }

        return result;
    }

    private float GetHazardWeight(RewardData hazard, int level)
    {
        float t = maxLevel > 0 ? (float)level / maxLevel : 0f;
        return Mathf.Max(0f, hazard.weight * hazardWeightCurve.Evaluate(t));
    }

    /// <summary>
    /// Weighted pick of which already-drawn wheel slot the spin lands on. This is where the
    /// hazard's growing weight actually takes effect: it always occupies exactly one of the
    /// slots, but its odds of being the winning slot increase with level.
    /// </summary>
    public int PickWinningIndex(IReadOnlyList<RewardData> slots, int level)
    {
        var weights = new float[slots.Count];
        float totalWeight = 0f;
        for (int i = 0; i < slots.Count; i++)
        {
            var reward = slots[i];
            float w = reward == null ? 0f : reward.isHazard ? GetHazardWeight(reward, level) : Mathf.Max(0f, reward.weight);
            weights[i] = w;
            totalWeight += w;
        }

        if (totalWeight <= 0f)
            return Random.Range(0, slots.Count);

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        for (int i = 0; i < slots.Count; i++)
        {
            cumulative += weights[i];
            if (roll <= cumulative)
                return i;
        }
        return slots.Count - 1;
    }

    /// <summary>
    /// Cumulative tier usability: a tier band makes every tier at or below it usable
    /// (e.g. the T2 band still allows T1 rewards), matching how rarity is meant to layer
    /// on top of earlier tiers rather than replace them.
    /// </summary>
    private static bool IsTierUsable(RewardTierMask itemTiers, RewardTier currentBand)
    {
        RewardTierMask usable = RewardTierMask.T1;
        if (currentBand >= RewardTier.T2) usable |= RewardTierMask.T2;
        if (currentBand >= RewardTier.T3) usable |= RewardTierMask.T3;
        return (itemTiers & usable) != 0;
    }
}
