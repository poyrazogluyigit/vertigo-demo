using System.Collections.Generic;
using UnityEngine;

public enum RewardType { WEAPON, COSMETIC, CHEST, GOLD, CASH, POINT }

[CreateAssetMenu(menuName = "Wheel/Reward Pool")]
public class RewardPool : ScriptableObject
{
    [SerializeField] public List<RewardData> rewards {get; private set;} = new List<RewardData>();
    [SerializeField] private AnimationCurve amountCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    [SerializeField] private int maxLevel = 30;

    public int CalculateAmount(RewardData reward, int level)
    {
        float t = maxLevel > 0 ? (float)level / maxLevel : 0f;
        return Mathf.RoundToInt(reward.baseAmount * amountCurve.Evaluate(t));
    }
}
