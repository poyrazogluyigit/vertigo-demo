using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Wheel/Reward Pool")]
public class RewardPool : ScriptableObject
{
    [SerializeField] private List<RewardDefinition> rewards  = new List<RewardDefinition>();
    [SerializeField] private RewardDefinition bomb;
    [SerializeField] private RewardDefinition superReward;
    [SerializeField] private AnimationCurve amountCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    [SerializeField] private int maxLevel = 30;
    public IReadOnlyList<RewardDefinition> Rewards => rewards;
    public RewardDefinition Bomb => bomb;
    public RewardDefinition Super => superReward;

    public int CalculateAmount(RewardDefinition rewardDefn, int level)
    {
        float t = maxLevel > 0 ? (float)level / maxLevel : 0f;
        return Mathf.Max(1, Mathf.RoundToInt(rewardDefn.baseAmount * amountCurve.Evaluate(t)));
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        foreach (var dupe in rewards.GroupBy(r => r.id).Where(g => g.Count() > 1))
            Debug.LogError($"RewardPool: duplicate reward id {dupe.Key}", this);
    }
#endif
}
