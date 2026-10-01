using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private RewardsManager _rewardsManager;
    [SerializeField] private InputManager _im;
    [SerializeField] private ViewManager _vm;

    enum States { RESTARTING, SETUP, IDLE, ACTIVE, CLEARED, EXIT, GAMEOVER }
    States currentState;

    void Start()
    {
        currentState = States.SETUP;
        GameLoop(); 
    }

    async void GameLoop()
    {
        switch (currentState)
        {
            case States.RESTARTING:

                _rewardsManager.ClearRewards();
                _vm.Clear();
                _currentLevel = 1;
                SetState(States.SETUP);
                break;

            case States.SETUP:

                await _vm.UpdateLevelIndicator(_currentLevel);
                _rewardsManager.GenerateRewards(_currentLevel);
                _vm.DrawWheel(_rewardsManager.CurrentWheel, _rewardsManager.PossibleRewards);
                EnableButtons();
                SetState(States.IDLE);
                break;
                
            case States.IDLE:
                {
                    break;
                }
            case States.ACTIVE:

                DisableButtons();
                SpinResult result = _rewardsManager.Pick();
                await _vm.SpinWheel(result.Slot);
                if (result.IsBomb) SetState(States.GAMEOVER);
                else SetState(States.CLEARED);
                break;

            case States.CLEARED:
                
                _vm.DisplayEarnedRewards(_rewardsManager.EarnedRewards);
                _currentLevel++;
                SetState(States.SETUP);
                break;
                
            case States.EXIT:

                DisableButtons();
                _vm.DisplayEndgameScreen(_rewardsManager.EarnedRewards);
                break;

            case States.GAMEOVER:
                
                DisableButtons();
                _rewardsManager.ClearRewards();
                _vm.DisplayGameOverScreen();
                break;
                
            default: break;
        }
    }

    void SetState(States state)
    {
        currentState = state;
        GameLoop();
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

    void OnExitButtonPressed() => SetState(States.EXIT);
    void OnRestartButtonPressed() => SetState(States.RESTARTING);
    void OnSpinButtonPressed() => SetState(States.ACTIVE);

    void DisableButtons()
    {
        _im.SetSpinButtonInteraction(false);
        _im.SetExitButtonInteraction(false); 
    }

    void EnableButtons()
    {
        _im.SetSpinButtonInteraction(true);
        if (_currentLevel % 5 == 0) _im.SetExitButtonInteraction(true); 
    }
}
