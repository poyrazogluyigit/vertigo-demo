using UnityEngine;

[CreateAssetMenu(fileName="NewWheelSO", menuName = "WheelSpin/WheelSO")]
public class WheelSO : ScriptableObject
{
    public const int SliceCount = 8;
    public Sprite WheelBase;
    public Sprite Indicator;
    // Tint of the sunburst behind the wheel; alpha 0 hides it
    public Color RaysColor = Color.clear;
    // Slice i is drawn on WheelView slot i
    public RewardDefinition[] Slices = new RewardDefinition[SliceCount];

#if UNITY_EDITOR
    void OnValidate()
    {
        if (Slices.Length != SliceCount)
            Debug.LogWarning($"{name}: has {Slices.Length} slices, the wheel shows {SliceCount}", this);
        for (int i = 0; i < Slices.Length; i++)
            if (Slices[i] == null)
                Debug.LogWarning($"{name}: slice {i} has no reward", this);
    }
#endif
}
