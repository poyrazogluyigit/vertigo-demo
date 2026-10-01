using System;

public class GameFlow : IDisposable
{
    private int _currentLevel = Zones.FirstLevel;
    private readonly IRewardService _rewards;
    private readonly IGameInput _input;
    private readonly IGameView _gameView;

    enum State { Restarting, Setup, Idle, Active, Cleared, Exit, GameOver }
    State _currentState;

    public GameFlow(IRewardService rewards, IGameInput input, IGameView gameView)
    {
        _rewards = rewards;
        _input = input;
        _gameView = gameView;

        _input.SpinPressed += OnSpinButtonPressed;
        _input.ExitPressed += OnExitButtonPressed;
        _input.RestartPressed += OnRestartButtonPressed;
    }

    public void Dispose()
    {
        _input.SpinPressed -= OnSpinButtonPressed;
        _input.ExitPressed -= OnExitButtonPressed;
        _input.RestartPressed -= OnRestartButtonPressed;
    }

    void OnExitButtonPressed()
    {
        if (CanExit())
            SetState(State.Exit);
    }
    void OnRestartButtonPressed() => SetState(State.Restarting);
    void OnSpinButtonPressed() => SetState(State.Active);

    public void Begin()
    {
        SetState(State.Setup);
    }

    async void GameLoop()
    {
        switch (_currentState)
        {
            case State.Restarting:

                _rewards.ClearRewards();
                _gameView.Clear();
                _currentLevel = Zones.FirstLevel;
                SetState(State.Setup);
                break;

            case State.Setup:

                await _gameView.UpdateLevelIndicator(_currentLevel);
                _rewards.GenerateRewards(_currentLevel);
                _gameView.DrawWheel(_rewards.CurrentWheel, _rewards.PossibleRewards);
                SetState(State.Idle);
                break;

            case State.Idle:

                EnableButtons();
                break;

            case State.Active:

                DisableButtons();
                SpinResult result = _rewards.Pick();
                await _gameView.SpinWheel(result.Slot);
                if (result.IsBomb) SetState(State.GameOver);
                else SetState(State.Cleared);
                break;

            case State.Cleared:

                _gameView.DisplayEarnedRewards(_rewards.EarnedRewards);
                _currentLevel++;
                SetState(State.Setup);
                break;

            case State.Exit:

                DisableButtons();
                _gameView.DisplayExitScreen(_rewards.EarnedRewards);
                break;

            case State.GameOver:

                DisableButtons();
                _rewards.ClearRewards();
                _gameView.DisplayGameOverScreen();
                break;

            default: break;
        }
    }

    void SetState(State state)
    {
        _currentState = state;
        GameLoop();
    }

    void DisableButtons()
    {
        _input.SetSpinEnabled(false);
        _input.SetExitEnabled(false);
    }

    bool CanExit()
    {
        return _currentState == State.Idle && Zones.AllowsExit(_currentLevel);
    }

    void EnableButtons()
    {
        _input.SetSpinEnabled(true);
        if (CanExit()) _input.SetExitEnabled(true);
    }
}
