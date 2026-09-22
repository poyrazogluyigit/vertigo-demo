using UnityEngine;

[CreateAssetMenu(fileName = "NewConsumableReward", menuName = "WheelSpin/Rewards/Consumable Reward")]
public class ConsumableRewardType : RewardType
{
    protected override float BaseMult => 1f;

    public override float CalculateReward(int amount)
    {
        // consumables are granted as discrete units, not scaled by a value multiplier
        return amount;
    }
}
