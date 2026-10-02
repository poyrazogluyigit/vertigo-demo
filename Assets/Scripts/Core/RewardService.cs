using System.Collections.Generic;

public class RewardService : IRewardService
{
    private readonly IWheelSchedule _schedule;
    private readonly IRewardPricing _pricing;
    private readonly IRandom _random;

    private readonly List<Reward> _earned = new List<Reward>();

    public RewardService(IWheelSchedule schedule, IRewardPricing pricing, IRandom random)
    {
        _schedule = schedule;
        _pricing = pricing;
        _random = random;
    }

    public WheelSO CurrentWheel { get; private set; }
    public Reward[] PossibleRewards { get; private set; } = new Reward[0];

    public Reward[] Earned() => _earned.ToArray();

    public void ClearRewards()
    {
        _earned.Clear();
    }

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

        int earned = isBomb ? 0 : Earn(reward);
        return new SpinResult(slot, reward, isBomb, earned);
    }

    // Adds to the reward's running total and returns the new total
    int Earn(Reward reward)
    {
        for (int i = 0; i < _earned.Count; i++)
        {
            if (_earned[i].RewardDefn == reward.RewardDefn)
            {
                _earned[i] = new Reward(_earned[i].RewardDefn, _earned[i].Amount + reward.Amount);
                return _earned[i].Amount;
            }
        }
        _earned.Add(reward);
        return reward.Amount;
    }
}
