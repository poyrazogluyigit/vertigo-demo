using UnityEngine;


[CreateAssetMenu(menuName = "WheelSpin/Reward Pool")]
public class RewardPool : ScriptableObject
{
    [SerializeField] private RewardDefinition bomb;
    [SerializeField] private AnimationCurve amountCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    [SerializeField] private int maxLevel = 30;
    public RewardDefinition Bomb => bomb;

    public int CalculateAmount(RewardDefinition rewardDefn, int level)
    {
        float t = maxLevel > 0 ? (float)level / maxLevel : 0f;
        return Mathf.Max(1, Mathf.RoundToInt(rewardDefn.baseAmount * amountCurve.Evaluate(t)));
    }
}
