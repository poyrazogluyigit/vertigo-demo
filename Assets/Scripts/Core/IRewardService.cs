namespace WheelSpin
{
    public interface IRewardService
    {
        // A copy of the current wheel's rewards, one per slot
        Reward[] PossibleRewards();

        // A copy, one entry per reward type, in the order each was first won
        Reward[] Earned();

        void ClearRewards();
        void GenerateRewards(int level);
        SpinResult Pick();
    }
}
