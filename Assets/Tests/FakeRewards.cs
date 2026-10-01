using System.Collections.Generic;

// Pick() returns whatever outcome the test chose last; defaults to a reward.
public class FakeRewards : IRewardManager
{
    private bool _nextIsBomb;
    private readonly Dictionary<RewardDefinition, int> _earnedRewards = new Dictionary<RewardDefinition, int>();

    public WheelSO CurrentWheel => null;
    public Reward[] PossibleRewards { get; } = new Reward[0];
    public IReadOnlyDictionary<RewardDefinition, int> EarnedRewards => _earnedRewards;

    public void ReturnReward() => _nextIsBomb = false;
    public void ReturnBomb() => _nextIsBomb = true;

    public void ClearRewards() => _earnedRewards.Clear();
    public void GenerateRewards(int level) { }
    public SpinResult Pick() => new SpinResult(0, new Reward(null, 0), _nextIsBomb, 0);
}
