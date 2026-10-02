// Pick() returns whatever outcome the test chose last; defaults to a reward.
public class FakeRewards : IRewardService
{
    private bool _nextIsBomb;

    public WheelSO CurrentWheel => null;
    public Reward[] PossibleRewards { get; } = new Reward[0];
    public Reward[] Earned() => new Reward[0];

    public void ReturnReward() => _nextIsBomb = false;
    public void ReturnBomb() => _nextIsBomb = true;

    public void ClearRewards() { }
    public void GenerateRewards(int level) { }
    public SpinResult Pick() => new SpinResult(0, new Reward(null, 0), _nextIsBomb, 0);
}
