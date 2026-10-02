using System;

public class GameFlow : IDisposable
{
    private int _currentLevel = Zones.FirstLevel;
    private readonly IEventBus _eventBus;
    private readonly IRewardService _rewards;
    private SpinResult _spinResult;

    // Setup and Spinning wait for a view to report its animation finished (LevelShown, SpinFinished).
    enum State { Restarting, Setup, Drawing, Idle, Spinning, Cleared, Exit, GameOver }
    State _currentState;

    public GameFlow(IEventBus eventBus, IRewardService rewards)
    {
        _eventBus = eventBus;
        _rewards = rewards;
    }

    public void Begin()
    {
        _eventBus.Subscribe<SpinPressed>(OnSpinButtonPressed);
        _eventBus.Subscribe<ExitPressed>(OnExitButtonPressed);
        _eventBus.Subscribe<RestartPressed>(OnRestartButtonPressed);
        _eventBus.Subscribe<LevelShown>(OnLevelShown);
        _eventBus.Subscribe<SpinFinished>(OnSpinFinished);
        SetState(State.Setup);
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<SpinPressed>(OnSpinButtonPressed);
        _eventBus.Unsubscribe<ExitPressed>(OnExitButtonPressed);
        _eventBus.Unsubscribe<RestartPressed>(OnRestartButtonPressed);
        _eventBus.Unsubscribe<LevelShown>(OnLevelShown);
        _eventBus.Unsubscribe<SpinFinished>(OnSpinFinished);
    }

    void OnExitButtonPressed(ExitPressed _)
    {
        if (CanExit())
            SetState(State.Exit);
    }
    void OnRestartButtonPressed(RestartPressed _)
    {
        if (_currentState == State.Exit || _currentState == State.GameOver)
            SetState(State.Restarting);
    }
    void OnSpinButtonPressed(SpinPressed _)
    {
        if (_currentState == State.Idle)
            SetState(State.Spinning);
    }
    void OnLevelShown(LevelShown _)
    {
        if (_currentState == State.Setup)
            SetState(State.Drawing);
    }
    void OnSpinFinished(SpinFinished _)
    {
        if (_currentState == State.Spinning)
            SetState(_spinResult.IsBomb ? State.GameOver : State.Cleared);
    }

    void EnableButtons() => _eventBus.Publish(new ActionsAllowed(true, CanExit()));
    void DisableButtons() => _eventBus.Publish(new ActionsAllowed(false, false));


    void GameLoop()
    {
        switch (_currentState)
        {
            case State.Restarting:

                _rewards.ClearRewards();
                _currentLevel = Zones.FirstLevel;
                _eventBus.Publish(new GameReset());
                SetState(State.Setup);
                break;

            case State.Setup:

                DisableButtons();
                _eventBus.Publish(new LevelStarted(_currentLevel));
                break;

            case State.Drawing:

                _rewards.GenerateRewards(_currentLevel);
                _eventBus.Publish(new WheelReady(_rewards.CurrentWheel, _rewards.PossibleRewards));
                SetState(State.Idle);
                break;

            case State.Idle:

                EnableButtons();
                break;

            case State.Spinning:

                DisableButtons();
                _spinResult = _rewards.Pick();
                _eventBus.Publish(new SpinStarted(_spinResult.Slot));
                break;

            case State.Cleared:

                _eventBus.Publish(new RewardsEarned(_rewards.EarnedRewards));
                _currentLevel++;
                SetState(State.Setup);
                break;

            case State.Exit:

                DisableButtons();
                _eventBus.Publish(new CashedOut(_rewards.EarnedRewards));
                break;

            case State.GameOver:

                _rewards.ClearRewards();
                _eventBus.Publish(new BombHit());
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
