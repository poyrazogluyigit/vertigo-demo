namespace WheelSpin
{
    public interface IRewardScaler
    {
        int AmountFor(RewardDefinition reward, int level);
    }
}
