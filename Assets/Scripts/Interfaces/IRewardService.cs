public interface IRewardService
{
    WheelSO CurrentWheel { get; }
    Reward[] PossibleRewards { get; }

    // A copy, one entry per reward type, in the order each was first won
    Reward[] Earned();

    void ClearRewards();
    void GenerateRewards(int level);
    SpinResult Pick();
}
