using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class RewardsManager : MonoBehaviour
{
    [SerializeField] private RewardPool rewardPool;
    private const int num_options = 8;

    public Reward[] PossibleRewards { get; private set; } = new Reward[0];
    private readonly Dictionary<int, int> _earnedRewards = new Dictionary<int, int>();
    public IReadOnlyDictionary<int, int> EarnedRewards => _earnedRewards;

    public static event System.Action<Reward[]> RewardsGenerated;

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
        int[] rewardIds = GetRandomRewards();
        var rewards = new Reward[rewardIds.Length];
        for (int i = 0; i < rewardIds.Length; i++)
        {
            int rewardId = rewardIds[i];
            rewards[i] = new Reward(rewardId, rewardPool.CalculateAmount(rewardId, level));
        }
        PossibleRewards = rewards;
        RewardsGenerated?.Invoke(PossibleRewards);
    }

    // Decides and commits the outcome immediately; presentation happens later.
    public SpinResult Pick()
    {
        int slot = Random.Range(0, PossibleRewards.Length);
        Reward reward = PossibleRewards[slot];
        bool isBomb = rewardPool.isBomb(reward.Id);

        _earnedRewards.TryGetValue(reward.Id, out int earned);
        if (!isBomb)
        {
            earned += reward.Amount;
            _earnedRewards[reward.Id] = earned;
        }
        return new SpinResult(slot, reward, isBomb, earned);
    }

    private int[] GetRandomRewards()
    {
        List<RewardBase> rewards = rewardPool.Rewards.Where(r => r != null).ToList();

        // Fisher-Yates shuffle over all reward pool
        for (int i = rewards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            // Swap
            RewardBase temp = rewards[i];
            rewards[i] = rewards[randomIndex];
            rewards[randomIndex] = temp;
        }
        return rewards.Take(num_options)
            .Select(r => r.id).ToArray();
    }
}
