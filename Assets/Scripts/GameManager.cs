using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private RewardsManager _rewardsManager;
    public static event System.Action<int> LevelChanged;
    public static event System.Action<int> SpinStarted;
    public static event System.Action<SpinResult> RoundResolved;
    public static event System.Action<bool> GameEnded;
    public static event System.Action GameRestarted;

    private bool _spinning;
    private bool _gameEnded;
    private bool _isLost = false;
    private SpinResult _pending;

    void OnEnable()
    {
        WheelView.SpinButtonClicked += OnSpinClicked;
        WheelView.SpinAnimationComplete += OnSpinAnimationComplete;
        RewardPanelView.ExitButtonClicked += OnExitClicked;
        EndgameView.RestartButtonClicked += RestartGame;
    }

    void OnDisable()
    {
        WheelView.SpinButtonClicked -= OnSpinClicked;
        WheelView.SpinAnimationComplete -= OnSpinAnimationComplete;
        RewardPanelView.ExitButtonClicked -= OnExitClicked;
        EndgameView.RestartButtonClicked -= RestartGame;
    }

    void Start()
    {
        SetLevel(1);
    }

    void RestartGame()
    {
        _isLost = false;
        _gameEnded = false;
        GameRestarted?.Invoke();   // listeners drop whatever last run left behind
        SetLevel(1);
    }

    void OnSpinClicked()
    {
        if (_spinning) return;
        _spinning = true;
        _pending = _rewardsManager.Pick();      // data committed now
        SpinStarted?.Invoke(_pending.Slot);     // presentation starts
    }

    void OnSpinAnimationComplete()
    {
        if (!_spinning) return;
        _spinning = false;
        RoundResolved?.Invoke(_pending);
        if (_pending.IsBomb)
        {
           _isLost = true; EndGame(); 
        }
        else SetLevel(_currentLevel + 1);
    }

    // The spin's result is already picked once _spinning is set, so quitting
    // before the animation lands would drop a reward the player has won.
    void OnExitClicked()
    {
        if (_spinning || _gameEnded) return;
        EndGame();
    }

    private void EndGame()
    {
        _gameEnded = true;
        GameEnded?.Invoke(_isLost);
    }

    void SetLevel(int level)
    {
        _currentLevel = level;
        LevelChanged?.Invoke(_currentLevel);
    }

}
