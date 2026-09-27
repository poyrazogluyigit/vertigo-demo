using System;
using UnityEngine;

[CreateAssetMenu(menuName = "WheelSpin/Reward Defition", fileName = "NewRewardDefinition")]
public class RewardDefinition : ScriptableObject, IEquatable<RewardDefinition>
{
    public int id;
    public Sprite image;
    public float baseAmount = 1f;
    public RewardScaling scaling;   // null keeps baseAmount on every level
    public bool Equals(RewardDefinition other)
    {
        if (other == null || GetType() != other.GetType())
        {
            return false;
        }
        return id == other.id;
    }
    public override bool Equals(object obj) => Equals(obj as RewardDefinition);
    public override int GetHashCode() => id.GetHashCode();
    public static bool operator ==(RewardDefinition left, RewardDefinition right) => left?.Equals(right) ?? right is null;
    public static bool operator !=(RewardDefinition left, RewardDefinition right) => !(left == right);
}
