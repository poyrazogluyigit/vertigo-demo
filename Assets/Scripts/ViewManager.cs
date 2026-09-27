using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class ViewManager : MonoBehaviour
{
    [SerializeField] private EndgameView _endgameView;
    [SerializeField] private LevelPanelView _levelPanelView;
    [SerializeField] private WheelView _wheelView;
    [SerializeField] private RewardPanelView _rewardPanelView;
    [SerializeField] private EndScreen _gameOverScreen, _cashOutScreen, _winScreen;


#if UNITY_EDITOR
    // includeInactive: the endgame view starts disabled
    void OnValidate()
    {
        _endgameView = FindObjectOfType<EndgameView>(true);
        _levelPanelView = FindObjectOfType<LevelPanelView>(true);
        _wheelView = FindObjectOfType<WheelView>(true);
        _rewardPanelView = FindObjectOfType<RewardPanelView>(true);

        if (_endgameView == null) Debug.LogError("Endgame view cannot be found!");
        if (_levelPanelView == null) Debug.LogError("Level panel view cannot be found!");
        if (_wheelView == null) Debug.LogError("Wheel View cannot be found!");
        if (_rewardPanelView == null) Debug.LogError("Reward panel view cannot be found!");
    }
#endif

    public void DrawWheel(WheelSO wheel, Reward[] rewards)
    {
        _wheelView.DrawWheel(wheel);
        _wheelView.DrawRewards(rewards);
    }

    public async Task SpinWheel(int slot)
    {
        await _wheelView.Spin(slot);
    }
    public async Task UpdateLevelIndicator(int level)
    {
        await _levelPanelView.ChangeLevel(level);
    }

    public void DisplayEarnedRewards(IReadOnlyDictionary<RewardDefinition, int> rewards)
    {
        foreach (var k in rewards.Keys)
        {
            _rewardPanelView.DisplayInGame(new Reward(k, rewards[k]));
        }
    }
    public void DisplayEndgameScreen(bool isWon, IReadOnlyDictionary<RewardDefinition, int> rewards)
    {
        if (isWon) _endgameView.DisplayScreen(_winScreen);
        else _endgameView.DisplayScreen(_cashOutScreen);
        foreach (var k in rewards.Keys)
        {
            _rewardPanelView.DisplayEndgame(new Reward(k, rewards[k]));
        }
    }

    public void DisplayGameOverScreen()
    {
        _endgameView.DisplayScreen(_gameOverScreen);
    }

    public void Clear()
    {
        _endgameView.HideEndScreen();
        _rewardPanelView.ClearInGameRewards();
        _levelPanelView.ResetIndicator();
    }

}
