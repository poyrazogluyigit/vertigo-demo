using System.Collections.Generic;
using UnityEngine;

public readonly struct Reward
{
    public readonly RewardData Data;
    public readonly int Amount;

    public Reward(RewardData reward, int amount)
    {
        Data = reward;
        Amount = amount;
    }
}
public class RewardsManager : MonoBehaviour
{
    [SerializeField] private RewardPool rewardPool;

    public Reward[] GenerateRewards(int level)
    {
        const int numRewards = 8;
        Reward[] rewards = new Reward[numRewards];
        RewardData[] rewardDatas = GetRandomRewards(numRewards);
        for (int i = 0; i < numRewards; i++)
        {
            var rd = rewardDatas[i];
            int amount = rewardPool.CalculateAmount(rd, level);
            rewards[i] = new Reward(rd, amount);
        }
        return rewards;
    }

    private RewardData[] GetRandomRewards(int numRewards)
    {
        List<RewardData> rewards = new List<RewardData>(rewardPool.rewards);

        // Fisher-Yates shuffle over all reward pool
        for (int i = rewards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            // Swap
            RewardData temp = rewards[i];
            rewards[i] = rewards[randomIndex];
            rewards[randomIndex] = temp;
        }
        return rewards.GetRange(0, numRewards).ToArray();    
    }
}
