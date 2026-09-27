using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class RewardsManager : MonoBehaviour
{
    [SerializeField] private RewardPool rewardPool;
    private const int num_options = 8;
    public Reward[] PossibleRewards { get; private set; } = new Reward[0];
    private readonly Dictionary<RewardDefinition, int> _earnedRewards = new Dictionary<RewardDefinition, int>();
    public IReadOnlyDictionary<RewardDefinition, int> EarnedRewards => _earnedRewards;


    public void ClearRewards() => _earnedRewards.Clear();

    public void GenerateRewards(int level)
    {
        RewardDefinition[] rewardDefns = GetRandomRewards();
        var rewards = new Reward[rewardDefns.Length];
        int i = 0;
        if (level == 30) rewards[i++] = new Reward(rewardPool.Super, 1);    
        else if (level % 5 != 0 && level != 1) rewards[i++] = new Reward(rewardPool.Bomb, 1);
        while (i < num_options)
        {
            rewards[i] = new Reward(rewardDefns[i], rewardPool.CalculateAmount(rewardDefns[i], level));
            i++;
        }
        PossibleRewards = rewards;
    }

    // Decides and commits the outcome immediately; presentation happens later.
    public SpinResult Pick()
    {
        int slot = Random.Range(0, PossibleRewards.Length);
        Reward reward = PossibleRewards[slot];
        bool isBomb = reward.RewardDefn == rewardPool.Bomb;

        _earnedRewards.TryGetValue(reward.RewardDefn, out int earned);
        if (!isBomb)
        {
            earned += reward.Amount;
            _earnedRewards[reward.RewardDefn] = earned;
        }
        return new SpinResult(slot, reward, isBomb, earned);
    }

    private RewardDefinition[] GetRandomRewards()
    {
        List<RewardDefinition> rewards = rewardPool.Rewards.Where(r => r != null).ToList();

        // Fisher-Yates shuffle over all reward pool
        for (int i = rewards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            // Swap
            RewardDefinition temp = rewards[i];
            rewards[i] = rewards[randomIndex];
            rewards[randomIndex] = temp;
        }
        return rewards.Take(num_options)
            .ToArray();
    }
}
