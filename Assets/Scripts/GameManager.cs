using UnityEngine;

namespace WheelSpin
{
    // Composition root: holds the scene references and wires
    // everything to the event bus.
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private WheelSchedule _wheelSchedule;
        [SerializeField] private RewardPricing _rewardPricing;

        [SerializeField] private InputManager _inputManager;
        [SerializeField] private View[] _views;

        private GameFlow _gameFlow;
        private readonly IEventBus _eventBus = new EventBus();

    #if UNITY_EDITOR
        void OnValidate()
        {
            _inputManager = FindObjectOfType<InputManager>(true);
            _views = FindObjectsOfType<View>(true);

            if (_inputManager == null) Debug.LogError("Input manager cannot be found!", this);
            if (_views.Length == 0) Debug.LogError("No views can be found", this);
            if (_wheelSchedule == null) Debug.LogError("Wheel schedule is not assigned!", this);
            if (_rewardPricing == null) Debug.LogError("Reward pricing is not assigned!", this);
        }
    #endif

        // Everything subscribes in Awake; GameFlow starts publishing in Start.
        void Awake()
        {
            var rewards = new RewardService(_wheelSchedule, _rewardPricing, new UnityRandom());
            _gameFlow = new GameFlow(_eventBus, rewards);
            _inputManager.Bind(_eventBus);
            foreach (var view in _views)
            {
                view.Bind(_eventBus);
            }
        }

        void Start()
        {
            _gameFlow.Begin();
        }

        void OnDestroy()
        {
            _gameFlow?.Dispose();
            foreach (View view in _views)
                if (!ReferenceEquals(view, null)) view.Unbind();
        }
    }
}
