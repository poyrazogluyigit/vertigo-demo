using UnityEngine;

[CreateAssetMenu(fileName = "NewGoldReward", menuName = "WheelSpin/Rewards/Gold Reward")]
public class GoldRewardType : RewardType
{
    protected override float BaseMult => 0.5f;

    public override float CalculateReward(int amount)
    {
        // gold is a whole-number currency, so the fractional BaseMult must not produce fractional gold
        return Mathf.Floor(amount * BaseMult);
    }
}
