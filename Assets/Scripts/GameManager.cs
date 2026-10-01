using UnityEngine;
using UnityEngine.Serialization;

// Composition root: holds the scene references and wires them into GameFlow.
public class GameManager : MonoBehaviour
{
    [SerializeField] private WheelSchedule _wheelSchedule;
    [SerializeField] private RewardPricing _rewardPricing;

    [SerializeField] private InputManager _inputManager;
    [SerializeField] private ViewManager _vm;

    private GameFlow _gameFlow;
    private EventBus _eventBus = new EventBus();

#if UNITY_EDITOR
    void OnValidate()
    {
        _inputManager = FindObjectOfType<InputManager>(true);
        _vm = FindObjectOfType<ViewManager>(true);

        if (_inputManager == null) Debug.LogError("Input manager cannot be found!", this);
        if (_vm == null) Debug.LogError("View manager cannot be found!", this);
        if (_wheelSchedule == null) Debug.LogError("Wheel schedule is not assigned!", this);
        if (_rewardPricing == null) Debug.LogError("Reward pricing is not assigned!", this);
    }
#endif

    void Awake()
    {
        var rewards = new RewardService(_wheelSchedule, _rewardPricing, new UnityRandom());
        _gameFlow = new GameFlow(_eventBus, rewards, _vm);
        _inputManager.Bind(_eventBus);
    }

    void Start()
    {
        _gameFlow.Begin();
    }

    void OnDestroy()
    {
        _gameFlow?.Dispose();
    }
}
