using System;

public class GameFlow : IDisposable
{
    private int _currentLevel = Zones.FirstLevel;
    private EventBus _eventBus;
    private readonly IRewardService _rewards;
    private readonly IGameView _gameView;

    enum State { Restarting, Setup, Idle, Active, Cleared, Exit, GameOver }
    State _currentState;

    public GameFlow(EventBus eventBus, IRewardService rewards, IGameView gameView)
    {
        _eventBus = eventBus;
        _rewards = rewards;
        _gameView = gameView;
    }

    public void Begin()
    {
        _eventBus.Subscribe<SpinPressed>(OnSpinButtonPressed);
        _eventBus.Subscribe<ExitPressed>(OnExitButtonPressed);
        _eventBus.Subscribe<RestartPressed>(OnRestartButtonPressed);
        SetState(State.Setup);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<SpinPressed>(OnSpinButtonPressed);
        _eventBus.Unsubscribe<ExitPressed>(OnExitButtonPressed);
        _eventBus.Unsubscribe<RestartPressed>(OnRestartButtonPressed);
    }

    void OnExitButtonPressed(ExitPressed _)
    {
        if (CanExit())
            SetState(State.Exit);
    }
    void OnRestartButtonPressed(RestartPressed _) => SetState(State.Restarting);
    void OnSpinButtonPressed(SpinPressed _) => SetState(State.Active);

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

                _eventBus.Publish(new ActionsAllowed(false, false));
                await _gameView.UpdateLevelIndicator(_currentLevel);
                _rewards.GenerateRewards(_currentLevel);
                _gameView.DrawWheel(_rewards.CurrentWheel, _rewards.PossibleRewards);
                SetState(State.Idle);
                break;

            case State.Idle:

                _eventBus.Publish(new ActionsAllowed(true, CanExit()));
                break;

            case State.Active:

                _eventBus.Publish(new ActionsAllowed(false, false));
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

                _gameView.DisplayExitScreen(_rewards.EarnedRewards);
                break;

            case State.GameOver:

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
    bool CanExit()
    {
        return _currentState == State.Idle && Zones.AllowsExit(_currentLevel);
    }
}
