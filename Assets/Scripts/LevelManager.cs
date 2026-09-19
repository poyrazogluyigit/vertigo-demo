using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Wheel))]
public class LevelManager : MonoBehaviour
{
    [SerializeField] private WheelSO[] wheelTypes;
    [SerializeField] private Wheel _wheel;
    [SerializeField] private Button _button;
    public int level {get; private set;}
    [SerializeField] private RewardSO[] rewards;
    private int _currentReward;

    void Start()
    {
        SetLevel(1);
        _button.onClick.AddListener(Play);
    }

    void generateRewards(bool isSafe, bool isSuper)
    {
        // to be implemented later
    }

    void Play()
    {
        _currentReward = Random.Range(0, rewards.Length);
        _wheel.Spin(_currentReward);
    }

    public void SetLevel(int level)
    {
        if (level == 30)
        {
            generateRewards(true, true);
            _wheel.Draw(wheelTypes[2], rewards);
            

        } // super level;
        else if (this.level == 1 || this.level % 5 == 0)
        {
            generateRewards(true, false);
            _wheel.Draw(wheelTypes[1], rewards);
        } // safe level;
        else
        {
            generateRewards(false, false);
            _wheel.Draw(wheelTypes[0], rewards);
        }
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        _wheel.Draw(wheelTypes[1], rewards);
    }
#endif
}
