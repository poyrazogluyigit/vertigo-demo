using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private RewardsManager _rewardsManager;
    public static event System.Action<int> LevelChanged;
    public static event System.Action<int> SpinStarted;
    public static event System.Action<SpinResult> RoundResolved;

    private bool _spinning;
    private SpinResult _pending;

    void OnEnable()
    {
        WheelView.SpinButtonClicked += OnSpinClicked;
        WheelView.SpinAnimationComplete += OnSpinAnimationComplete;
    }

    void OnDisable()
    {
        WheelView.SpinButtonClicked -= OnSpinClicked;
        WheelView.SpinAnimationComplete -= OnSpinAnimationComplete;
    }

    void Start()
    {
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
        if (_pending.IsBomb) GameOver();
        else SetLevel(_currentLevel + 1);
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
    }

    void SetLevel(int level)
    {
        _currentLevel = level;
        LevelChanged?.Invoke(_currentLevel);
    }

}
