using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "WheelSpin/Reward Defition", fileName = "NewRewardDefinition")]
public class RewardDefinition : ScriptableObject, IEquatable<RewardDefinition>
{
    [FormerlySerializedAs("id")]
    public int Id;
    [FormerlySerializedAs("image")]
    public Sprite Image;
    [FormerlySerializedAs("baseAmount")]
    public float BaseAmount = 1f;
    [FormerlySerializedAs("scaling")]
    public RewardScaling Scaling;   // null keeps BaseAmount on every level
    public bool Equals(RewardDefinition other)
    {
        if (other == null || GetType() != other.GetType())
        {
            return false;
        }
        return Id == other.Id;
    }
    public override bool Equals(object obj) => Equals(obj as RewardDefinition);
    public override int GetHashCode() => Id.GetHashCode();
    public static bool operator ==(RewardDefinition left, RewardDefinition right) => left?.Equals(right) ?? right is null;
    public static bool operator !=(RewardDefinition left, RewardDefinition right) => !(left == right);
}
