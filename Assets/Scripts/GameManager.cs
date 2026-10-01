using UnityEngine;
using UnityEngine.Serialization;

// Composition root: holds the scene references and wires them into GameFlow.
public class GameManager : MonoBehaviour
{
    [SerializeField] private WheelSchedule _wheelSchedule;
    [SerializeField] private RewardPricing _rewardPricing;
    [FormerlySerializedAs("_im")]
    [SerializeField] private InputManager _input;
    [SerializeField] private ViewManager _vm;

    private GameFlow _gameFlow;

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
