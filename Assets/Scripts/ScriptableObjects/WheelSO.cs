using UnityEngine;

[CreateAssetMenu(fileName="NewWheelSO", menuName = "WheelSpin/WheelSO")]
public class WheelSO : ScriptableObject
{
    public const int SliceCount = 8;
    public Sprite WheelBase;
    public Sprite Indicator;
    public Color RaysColor = Color.clear;
    public RewardDefinition[] Slices = new RewardDefinition[SliceCount];

    public int BombCount
    {
        get
        {
            int count = 0;
            foreach (RewardDefinition slice in Slices)
                if (slice != null && slice.IsBomb) count++;
            return count;
        }
    }

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
