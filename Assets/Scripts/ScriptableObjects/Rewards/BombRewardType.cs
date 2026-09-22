using UnityEngine;

[CreateAssetMenu(fileName = "NewBombReward", menuName = "WheelSpin/Rewards/Bomb Reward")]
public class BombRewardType : RewardType
{
    protected override float BaseMult => 0f;

    public override bool IsBomb()
    {
        return true;
    }
}
