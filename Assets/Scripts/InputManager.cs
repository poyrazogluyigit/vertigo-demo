using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Button _spinButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _restartButton;
    public static event System.Action SpinButtonPressed,
    ExitButtonPressed, RestartButtonPressed;

    public void SetSpinButtonInteraction(bool interaction) => _spinButton.interactable = interaction;
    public void SetExitButtonInteraction(bool interaction) => _exitButton.interactable = interaction;
    public void SetRestartButtonInteraction(bool interaction) => _restartButton.interactable = interaction;

#if UNITY_EDITOR
    void OnValidate()
    {
        // includeInactive: the restart button lives under the disabled endgame view
        Button[] buttons = FindObjectsOfType<Button>(true);

        _spinButton = System.Array.Find(buttons, b => b.name == "ui_button_wheel_spin");
        _exitButton = System.Array.Find(buttons, b => b.name == "ui_button_rewards_exit");
        _restartButton = System.Array.Find(buttons, b => b.name == "ui_button_gameend_restart");

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

    void OnSpinClicked() => SpinButtonPressed?.Invoke();
    void OnExitClicked() => ExitButtonPressed?.Invoke();
    void OnRestartClicked() => RestartButtonPressed?.Invoke();
    


}
