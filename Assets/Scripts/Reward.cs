public readonly struct Reward
{
    public readonly int Id;
    public readonly int Amount;

    public Reward(int id, int amount)
    {
        Id = id;
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
