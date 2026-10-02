using System;

namespace WheelSpin
{
    public class GameFlow : IDisposable
    {
        private int _currentLevel = Zones.FirstLevel;
        private readonly IEventBus _eventBus;
        private readonly IRewardService _rewards;
        private SpinResult _spinResult;

        enum State { StartingLevel, Idle, Spinning, CashedOut, GameOver }
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
            _eventBus.Subscribe<SpinFinished>(OnSpinFinished);
            SetState(State.StartingLevel);
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<SpinPressed>(OnSpinButtonPressed);
            _eventBus.Unsubscribe<ExitPressed>(OnExitButtonPressed);
            _eventBus.Unsubscribe<RestartPressed>(OnRestartButtonPressed);
            _eventBus.Unsubscribe<SpinFinished>(OnSpinFinished);
        }

        void OnExitButtonPressed(ExitPressed _)
        {
            if (CanExit())
                SetState(State.CashedOut);
        }
        void OnRestartButtonPressed(RestartPressed _)
        {
            if (_currentState == State.CashedOut || _currentState == State.GameOver)
                Restart();
        }
        void OnSpinButtonPressed(SpinPressed _)
        {
            if (_currentState == State.Idle)
                SetState(State.Spinning);
        }

        void OnSpinFinished(SpinFinished _)
        {
            if (!(_currentState == State.Spinning)) return;
            if (_spinResult.IsBomb) SetState(State.GameOver);
            else AdvanceLevel();
        }

        void EnableButtons() => _eventBus.Publish(new ActionsAllowed(true, CanExit()));
        void DisableButtons() => _eventBus.Publish(new ActionsAllowed(false, false));


        void ClearRewards()
        {
            _rewards.ClearRewards();
            _eventBus.Publish(new RewardsChanged(_rewards.Earned()));
        }

        void Restart()
        {
            _eventBus.Publish(new GameReset());
            ClearRewards();
            _currentLevel = Zones.FirstLevel;
            SetState(State.StartingLevel);
        }

        void AdvanceLevel()
        {
            _eventBus.Publish(new RewardsChanged(_rewards.Earned()));
            _currentLevel++;
            SetState(State.StartingLevel);
        }


        void GameState()
        {
            switch (_currentState)
            {
                case State.StartingLevel:

                    DisableButtons();
                    _eventBus.Publish(new LevelChangedTo(_currentLevel));
                    _rewards.GenerateRewards(_currentLevel);
                    _eventBus.Publish(new RewardsReady(Zones.TypeOf(_currentLevel), _rewards.PossibleRewards()));
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

                case State.CashedOut:

                    _eventBus.Publish(new CashedOut(_rewards.Earned()));
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
            GameState();
        }
        bool CanExit()
        {
            return _currentState == State.Idle && Zones.AllowsExit(_currentLevel);
        }
    }
}
