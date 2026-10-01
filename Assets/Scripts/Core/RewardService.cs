using System.Collections.Generic;

public class RewardService : IRewardService
{
    private readonly IWheelSchedule _schedule;
    private readonly IRewardPricing _pricing;
    private readonly IRandom _random;
    private readonly Dictionary<RewardDefinition, int> _earnedRewards = new Dictionary<RewardDefinition, int>();

    public RewardService(IWheelSchedule schedule, IRewardPricing pricing, IRandom random)
    {
        _schedule = schedule;
        _pricing = pricing;
        _random = random;
    }

    public WheelSO CurrentWheel { get; private set; }
    public Reward[] PossibleRewards { get; private set; } = new Reward[0];
    public IReadOnlyDictionary<RewardDefinition, int> EarnedRewards => _earnedRewards;

    public void ClearRewards() => _earnedRewards.Clear();

    public void GenerateRewards(int level)
    {
        CurrentWheel = _schedule.WheelFor(level);
        RewardDefinition[] slices = CurrentWheel.Slices;
        var rewards = new Reward[slices.Length];
        for (int i = 0; i < slices.Length; i++)
        {
            RewardDefinition slice = slices[i];
            // Amount 0 hides the label, so the bomb shows no "x1"
            int amount = slice.IsBomb ? 0 : _pricing.AmountFor(slice, level);
            rewards[i] = new Reward(slice, amount);
        }
        PossibleRewards = rewards;
    }

    // Decides and commits the outcome immediately; presentation happens later.
    public SpinResult Pick()
    {
        int slot = _random.Range(0, PossibleRewards.Length);
        Reward reward = PossibleRewards[slot];
        bool isBomb = reward.RewardDefn.IsBomb;

        _earnedRewards.TryGetValue(reward.RewardDefn, out int earned);
        if (!isBomb)
        {
            earned += reward.Amount;
            _earnedRewards[reward.RewardDefn] = earned;
        }
        return new SpinResult(slot, reward, isBomb, earned);
    }
}
