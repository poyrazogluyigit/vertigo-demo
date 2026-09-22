using UnityEngine;

[CreateAssetMenu(fileName="NewReward", menuName = "WheelSpin/New Reward")]
public class RewardType : ScriptableObject
{
    public Sprite sprite;

    protected virtual float BaseMult => 0f;

    public virtual float CalculateReward(int amount)
    {
        return amount * BaseMult;
    }

    public virtual bool IsBomb()
    {
        return false;
    }
}
