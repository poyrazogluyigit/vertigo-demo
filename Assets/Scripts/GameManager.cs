using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private RewardsManager _rewardsManager;
    public static event System.Action<int> LevelChanged;

    void OnEnable()
    {
        WheelView.SpinButtonClicked += Play;
        RewardsManager.RewardPicked += EndLevel;
    }

    void OnDisable()
    {
        WheelView.SpinButtonClicked -= Play;
        RewardsManager.RewardPicked -= EndLevel;
    }

    void Start()
    {
        SetLevel(1);
    }
    void Play()
    {
        SetLevel(_currentLevel + 1);
    }

    void EndLevel(bool isBomb)
    {
        if (isBomb) GameOver();
        else SetLevel(_currentLevel + 1);
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
    }

    void SetLevel(int level)
    {
        _currentLevel = level;
        LevelChanged.Invoke(_currentLevel);
    }

}
