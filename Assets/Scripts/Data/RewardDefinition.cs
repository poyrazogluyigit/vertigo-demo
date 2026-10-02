using UnityEngine;
using UnityEngine.Serialization;

namespace WheelSpin
{
    [CreateAssetMenu(menuName = "WheelSpin/Reward Definition", fileName = "NewRewardDefinition")]
    public class RewardDefinition : ScriptableObject
    {
        [FormerlySerializedAs("image")]
        public Sprite Image;
        [FormerlySerializedAs("baseAmount")]
        public float BaseAmount = 1f;
        [FormerlySerializedAs("scaling")]
        public RewardScaling Scaling;   // null keeps BaseAmount on every level
        public bool IsBomb;
    }
}
