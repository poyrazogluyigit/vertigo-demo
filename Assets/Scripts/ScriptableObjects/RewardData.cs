using UnityEngine;

[CreateAssetMenu(menuName = "Wheel/Reward Data")]
public class RewardData : ScriptableObject
{
    public RewardType category;
    public Sprite sprite;
    public float baseAmount = 1f;
    public float weight = 1f;
    public bool isBomb {get; private set;} = false;
}
