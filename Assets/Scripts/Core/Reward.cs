namespace WheelSpin
{
    public readonly struct Reward
    {
        public readonly RewardDefinition Definition;
        public readonly int Amount;

        public Reward(RewardDefinition definition, int amount)
        {
            Definition = definition;
            Amount = amount;
        }
    }

    public readonly struct SpinResult
    {
        public readonly int Slot;
        public readonly Reward Reward;

        public bool IsBomb => Reward.Definition.IsBomb;

        public SpinResult(int slot, Reward reward)
        {
            Slot = slot;
            Reward = reward;
        }
    }
}
