public readonly struct Reward
{
    public readonly RewardDefinition RewardDefn;
    public readonly int Amount;

    public Reward(RewardDefinition rd, int amount)
    {
        RewardDefn = rd;
        Amount = amount;
    }
}

public readonly struct SpinResult
{
    public readonly int Slot;
    public readonly Reward Reward;
    public readonly bool IsBomb;
    public readonly int EarnedTotal;

    public SpinResult(int slot, Reward reward, bool isBomb, int earnedTotal)
    {
        Slot = slot;
        Reward = reward;
        IsBomb = isBomb;
        EarnedTotal = earnedTotal;
    }
}
