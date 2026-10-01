using System.Collections.Generic;

public interface IRewardManager
{
    WheelSO CurrentWheel { get; }
    Reward[] PossibleRewards { get; }
    IReadOnlyDictionary<RewardDefinition, int> EarnedRewards { get; }

    void ClearRewards();
    void GenerateRewards(int level);
    SpinResult Pick();
}
