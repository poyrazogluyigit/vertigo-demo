using UnityEngine;
using UnityEngine.Serialization;

namespace WheelSpin
{
    [CreateAssetMenu(menuName = "WheelSpin/Reward Scaling", fileName = "NewRewardScaling")]
    public class RewardScaling : ScriptableObject
    {
        [FormerlySerializedAs("multiplierAtMaxLevel")]
        [Min(1f)] public float MultiplierAtMaxLevel = 1f;

        // Geometric: 1x at level 1, MultiplierAtMaxLevel x at maxLevel, and it keeps growing past maxLevel
        public float Multiplier(int level, int maxLevel) =>
            Mathf.Pow(MultiplierAtMaxLevel, (level - 1f) / (maxLevel - 1f));
    }
}
