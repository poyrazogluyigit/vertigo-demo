using System.Diagnostics.Contracts;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private WheelSO[] wheelTypes;
    [SerializeField] private Wheel wheel;
    private int _level;
    [SerializeField] private RewardSO[] rewards;

    void Start()
    {
        SetLevel(1);
    }

    void generateRewards(bool isSafe, bool isSuper)
    {
        // to be implemented later
    }

    void SetLevel(int level)
    {
        if (level == 30)
        {
            generateRewards(true, true);
            wheel.Draw(wheelTypes[2], rewards);
            

        } // super level;
        else if (_level == 1 || _level % 5 == 0)
        {
            generateRewards(true, false);
            wheel.Draw(wheelTypes[1], rewards);
        } // safe level;
        else
        {
            generateRewards(false, false);
            wheel.Draw(wheelTypes[0], rewards);
        }
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        
    }
#endif
}
