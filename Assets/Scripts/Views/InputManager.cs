using UnityEngine;
using UnityEngine.UI;

namespace WheelSpin
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Button _spinButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _restartButton;

        private IEventBus _eventBus;

        public void SetButtonInteractability(ActionsAllowed a)
        {
            _spinButton.interactable = a.Spin;
            _exitButton.interactable = a.Exit;
        }

        public void Bind(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<ActionsAllowed>(SetButtonInteractability);
        }

        void OnDestroy() => _eventBus?.Unsubscribe<ActionsAllowed>(SetButtonInteractability);

    #if UNITY_EDITOR
        void OnValidate()
        {
            // includeInactive: the restart button lives under the disabled endgame view
            Button[] buttons = FindObjectsOfType<Button>(true);

            _spinButton = System.Array.Find(buttons, b => b.name == "ui_button_wheel_spin");
            _exitButton = System.Array.Find(buttons, b => b.name == "ui_button_rewards_exit");
            _restartButton = System.Array.Find(buttons, b => b.name == "ui_button_gameend_restart_value");

            if (_spinButton == null) Debug.LogError("Spin button cannot be found!");
            if (_exitButton == null) Debug.LogError("Exit button cannot be found!");
            if (_restartButton == null) Debug.LogError("Restart button cannot be found!");
        }
    #endif

        void OnEnable()
        {
            _spinButton.onClick.AddListener(OnSpinClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
            _restartButton.onClick.AddListener(OnRestartClicked);
        }
        void OnDisable()
        {
            _spinButton.onClick.RemoveListener(OnSpinClicked);
            _exitButton.onClick.RemoveListener(OnExitClicked);
            _restartButton.onClick.RemoveListener(OnRestartClicked);
        }

        void OnSpinClicked() => _eventBus.Publish(new SpinPressed());
        void OnExitClicked() => _eventBus.Publish(new ExitPressed());
        void OnRestartClicked() => _eventBus.Publish(new RestartPressed());
    }
}
