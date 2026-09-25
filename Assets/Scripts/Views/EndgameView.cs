using UnityEngine;
using UnityEngine.UI;

public class EndgameView : MonoBehaviour
{
    [SerializeField] private Canvas _endgameCanvas;
    [SerializeField] private GameObject gameOverContainer;
    [SerializeField] private GameObject exitContainer;
    [SerializeField] private Button RestartButton;
    public static event System.Action RestartButtonClicked;
    void OnEnable()
    {
        GameManager.GameEnded += DisplayEndScreen;
        RestartButton.onClick.AddListener(_rc);
    }

    void OnDisable()
    {
        GameManager.GameEnded -= DisplayEndScreen;
        RestartButton.onClick.RemoveListener(_rc);
    }

    private void _rc()
    {
        gameOverContainer.gameObject.SetActive(false);
        exitContainer.gameObject.SetActive(false);
        _endgameCanvas.gameObject.SetActive(false);
        RestartButtonClicked?.Invoke();
    }

    void DisplayEndScreen(bool isLost)
    {
        _endgameCanvas.gameObject.SetActive(true);
        if (isLost) gameOverContainer.SetActive(true);
        else exitContainer.SetActive(true);
    }
}
