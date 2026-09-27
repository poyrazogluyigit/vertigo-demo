using System.Threading.Tasks;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private RewardsManager _rewardsManager;
    [SerializeField] private InputManager _im;
    [SerializeField] private ViewManager _vm;

    void Start()
    {
        OnRestartButtonPressed();
    }

    void OnEnable()
    {
        InputManager.SpinButtonPressed += OnSpinButtonPressed;
        InputManager.ExitButtonPressed += OnExitButtonPressed;
        InputManager.RestartButtonPressed += OnRestartButtonPressed;
    }
    void OnDisable()
    {
        InputManager.SpinButtonPressed -= OnSpinButtonPressed;
        InputManager.ExitButtonPressed -= OnExitButtonPressed;
        InputManager.RestartButtonPressed -= OnRestartButtonPressed;
    }

    async void OnSpinButtonPressed()
    {
        DisableButtons();
        SpinResult result = _rewardsManager.Pick();
        await _vm.SpinWheel(result.Slot);
        await HandleLevelEnd(result.IsBomb);
        EnableButtons();
    }

    void OnExitButtonPressed()
    {
        DisableButtons();
        _vm.DisplayEndgameScreen(_rewardsManager.EarnedRewards);
    }

    void OnRestartButtonPressed()
    {
        _rewardsManager.ClearRewards();
        _vm.Clear();
        _currentLevel = 1;
        EnableButtons();
    }

    async Task HandleLevelEnd(bool isBomb)
    {
        if (isBomb)
        {
            HandleGameLost();
            return;
        }
        _vm.DisplayEarnedRewards(_rewardsManager.EarnedRewards);
        await SetLevel(++_currentLevel);
    }

    void HandleGameLost()
    {
        DisableButtons();
        _rewardsManager.ClearRewards();
        _vm.DisplayGameOverScreen();
    }

    void DisableButtons()
    {
        _im.SetSpinButtonInteraction(false);
        _im.SetExitButtonInteraction(false); 
    }

    void EnableButtons()
    {
        _im.SetSpinButtonInteraction(true);
        _im.SetExitButtonInteraction(true); 
    }

    async Task SetLevel(int level)
    {
        _currentLevel = level;
        await _vm.UpdateLevelIndicator(_currentLevel);
    }

}
