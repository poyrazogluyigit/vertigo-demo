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
    public Reward[] generatedRewards {get; private set;}
    private readonly Dictionary<RewardData, int> _earnedRewards = new Dictionary<RewardData, int>();
    public static event System.Action<Reward[]> RewardsGenerated;
    public static event System.Action<bool> RewardPicked;

    void OnEnable()
    {
        GameManager.LevelChanged += GenerateRewards;
    }

    void OnDisable()
    {
        GameManager.LevelChanged -= GenerateRewards;
    }

    public void GenerateRewards(int level)
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
        generatedRewards = rewards;
        RewardsGenerated.Invoke(generatedRewards);
    }

    private void PickReward()
    {
        Reward result = generatedRewards[Random.Range(0, 8)];
        bool isBomb = true;
        if (!result.Data.isBomb)
        {
            isBomb = false;
            int prev = _earnedRewards.TryGetValue(result.Data, out int amount) ? amount : 0;
            _earnedRewards[result.Data] = prev + result.Amount;
        }
        RewardPicked.Invoke(isBomb);
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
