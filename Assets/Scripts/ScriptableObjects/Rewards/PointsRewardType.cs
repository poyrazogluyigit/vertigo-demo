using UnityEngine;

[CreateAssetMenu(fileName = "NewPointsReward", menuName = "WheelSpin/Rewards/Points Reward")]
public class PointsRewardType : RewardType
{
    protected override float BaseMult => 5f;
}
