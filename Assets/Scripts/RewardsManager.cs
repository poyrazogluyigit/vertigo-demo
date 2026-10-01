using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class RewardsManager : MonoBehaviour, IRewardManager
{
    [FormerlySerializedAs("rewardPool")]
    [SerializeField] private RewardPool _rewardPool;
    [SerializeField] private WheelSO _bronzeWheel, _silverWheel, _goldWheel;
    public WheelSO CurrentWheel { get; private set; }
    public Reward[] PossibleRewards { get; private set; } = new Reward[0];
    private readonly Dictionary<RewardDefinition, int> _earnedRewards = new Dictionary<RewardDefinition, int>();
    public IReadOnlyDictionary<RewardDefinition, int> EarnedRewards => _earnedRewards;


    public void ClearRewards() => _earnedRewards.Clear();

    public WheelSO WheelForLevel(int level)
    {
        switch (Zones.TypeOf(level))
        {
            case ZoneType.Super: return _goldWheel;
            case ZoneType.Safe: return _silverWheel;
            default: return _bronzeWheel;
        }
    }

    public void GenerateRewards(int level)
    {
        CurrentWheel = WheelForLevel(level);
        RewardDefinition[] slices = CurrentWheel.Slices;
        var rewards = new Reward[slices.Length];
        for (int i = 0; i < slices.Length; i++)
        {
            RewardDefinition defn = slices[i];
            // Amount 0 hides the label, so the bomb shows no "x1"
            int amount = defn == _rewardPool.Bomb ? 0 : _rewardPool.CalculateAmount(defn, level);
            rewards[i] = new Reward(defn, amount);
        }
        PossibleRewards = rewards;
    }

    // Decides and commits the outcome immediately; presentation happens later.
    public SpinResult Pick()
    {
        int slot = Random.Range(0, PossibleRewards.Length);
        Reward reward = PossibleRewards[slot];
        bool isBomb = reward.RewardDefn == _rewardPool.Bomb;

        _earnedRewards.TryGetValue(reward.RewardDefn, out int earned);
        if (!isBomb)
        {
            earned += reward.Amount;
            _earnedRewards[reward.RewardDefn] = earned;
        }
        return new SpinResult(slot, reward, isBomb, earned);
    }
}
