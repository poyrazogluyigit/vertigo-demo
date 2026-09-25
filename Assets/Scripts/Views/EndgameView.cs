using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndgameView : MonoBehaviour
{
    [SerializeField] private Canvas _endgameCanvas;
    [SerializeField] private GameObject gameOverContainer;
    [SerializeField] private GameObject exitContainer;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Image RestartButtonImage;
    [SerializeField] private TMP_Text RestartButtonLabel;
    [SerializeField] private TMP_Text TitleText;
    [SerializeField] private TMP_Text DescriptionText;
    [SerializeField] private Sprite RestartButtonGreySprite;
    [SerializeField] private Sprite RestartButtonOrangeSprite;
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
        if (isLost)
        {
            gameOverContainer.SetActive(true);
            TitleText.text = "OH NO, A BOMB EXPLODED RIGHT IN YOUR HANDS!";
            DescriptionText.text = "You lost all your rewards.";
            RestartButtonLabel.text = "GIVE UP";
            RestartButtonImage.sprite = RestartButtonGreySprite;
        }
        else
        {
            exitContainer.SetActive(true);
            TitleText.text = "YOU LEFT THE TABLE";
            DescriptionText.text = "You collected the following rewards:";
            RestartButtonLabel.text = "PLAY AGAIN";
            RestartButtonImage.sprite = RestartButtonOrangeSprite;
        }
    }
}
