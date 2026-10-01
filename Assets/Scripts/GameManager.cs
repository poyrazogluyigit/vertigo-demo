using UnityEngine;
using UnityEngine.Serialization;

// Composition root: holds the scene references and wires them into GameFlow.
public class GameManager : MonoBehaviour
{
    [SerializeField] private WheelSchedule _wheelSchedule;
    [SerializeField] private RewardPricing _rewardPricing;

    [SerializeField] private InputManager _input;
    [SerializeField] private ViewManager _vm;

    private GameFlow _gameFlow;

#if UNITY_EDITOR
    void OnValidate()
    {
        _input = FindObjectOfType<InputManager>(true);
        _vm = FindObjectOfType<ViewManager>(true);

        if (_input == null) Debug.LogError("Input manager cannot be found!", this);
        if (_vm == null) Debug.LogError("View manager cannot be found!", this);
        if (_wheelSchedule == null) Debug.LogError("Wheel schedule is not assigned!", this);
        if (_rewardPricing == null) Debug.LogError("Reward pricing is not assigned!", this);
    }
#endif

    void Awake()
    {
        var rewards = new RewardService(_wheelSchedule, _rewardPricing, new UnityRandom());
        _gameFlow = new GameFlow(rewards, _input, _vm);
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
