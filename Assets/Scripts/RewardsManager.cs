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
    public static event System.Action<IReadOnlyDictionary<int, int>> EarnedRewardsChanged;
    public static event System.Action<int> RewardPicked;
    public static event System.Action<bool> BombHit;

    void OnEnable()
    {
        GameManager.LevelChanged += GenerateRewards;
        WheelView.SpinButtonClicked += PickReward;
    }

    void OnDisable()
    {
        GameManager.LevelChanged -= GenerateRewards;
        WheelView.SpinButtonClicked -= PickReward;
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

    private void PickReward()
    {
        Reward result = PossibleRewards[Random.Range(0, PossibleRewards.Length)];
        _earnedRewards.TryGetValue(result.Id, out int earned);
        _earnedRewards[result.Id] = earned + result.Amount;
        EarnedRewardsChanged?.Invoke(_earnedRewards);
        RewardPicked?.Invoke(result.Id);
        BombHit?.Invoke(rewardPool.isBomb(result.Id));
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
