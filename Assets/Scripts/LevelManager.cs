using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Wheel))]
public class LevelManager : MonoBehaviour
{
    [SerializeField] private WheelSO[] wheelTypes;
    [SerializeField] private Wheel _wheel;
    [SerializeField] private RewardType[] _possibleRewards;

    void generateRewards(bool isSafe, bool isSuper)
    {
        // to be implemented later
    }

    public async Task<RewardType> Play()
    {
       int  _currentReward = Random.Range(0, _possibleRewards.Length);
        Debug.Log($"Chosen index: {_currentReward}");
        await _wheel.Spin(_currentReward);
        return _possibleRewards[_currentReward];
    }

    public void SetLevel(int level)
    {
        if (level == 30)
        {
            generateRewards(true, true);
            _wheel.Draw(wheelTypes[2], _possibleRewards);
            

        } // super level;
        else if (level == 1 || level % 5 == 0)
        {
            generateRewards(true, false);
            _wheel.Draw(wheelTypes[1], _possibleRewards);
        } // safe level;
        else
        {
            generateRewards(false, false);
            _wheel.Draw(wheelTypes[0], _possibleRewards);
        }
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        _wheel.Draw(wheelTypes[1], _possibleRewards);
    }
#endif
}
