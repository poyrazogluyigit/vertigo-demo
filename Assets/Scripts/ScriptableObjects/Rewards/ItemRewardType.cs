using UnityEngine;

[CreateAssetMenu(fileName = "NewItemReward", menuName = "WheelSpin/Rewards/Item Reward")]
public class ItemRewardType : RewardType
{
    protected override float BaseMult => 0f;

    public override float CalculateReward(int amount)
    {
        // items are granted as discrete units, not scaled by a value multiplier
        return amount;
    }
}
