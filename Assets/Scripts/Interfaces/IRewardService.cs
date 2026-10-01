using System.Collections.Generic;

public interface IRewardService
{
    WheelSO CurrentWheel { get; }
    Reward[] PossibleRewards { get; }
    IReadOnlyDictionary<RewardDefinition, int> EarnedRewards { get; }

    void ClearRewards();
    void GenerateRewards(int level);
    SpinResult Pick();
}
