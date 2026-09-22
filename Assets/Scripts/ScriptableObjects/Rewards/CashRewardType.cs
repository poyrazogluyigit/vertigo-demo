using UnityEngine;

[CreateAssetMenu(fileName = "NewCashReward", menuName = "WheelSpin/Rewards/Cash Reward")]
public class CashRewardType : RewardType
{
    protected override float BaseMult => 5f;
}
