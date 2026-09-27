using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndgameView : View
{
    [SerializeField] private GameObject _rewardsContainer, _bombContainer;
    [SerializeField] private TMP_Text _title, _description, _buttonLabel;
    [SerializeField] private Image _buttonImage;

    public void DisplayScreen(EndScreen screen)
    {
        _title.text = screen.Title;
        _description.text = screen.Description;
        _buttonLabel.text = screen.ButtonLabel;
        _buttonImage.sprite = screen.ButtonSprite;
        _rewardsContainer.SetActive(screen.ShowsRewards);
        _bombContainer.SetActive(!screen.ShowsRewards);
        gameObject.SetActive(true);
    }

    public void HideEndScreen() => gameObject.SetActive(false);

    #if UNITY_EDITOR
    void OnValidate()
    {
        _rewardsContainer = Child<Transform>("ui_container_exit")?.gameObject;
        _bombContainer    = Child<Transform>("ui_container_gameover")?.gameObject;
        _title            = Child<TMP_Text>("ui_text_gameend_title_value");
        _description      = Child<TMP_Text>("ui_text_gameend_desc_value");
        _buttonLabel      = Child<TMP_Text>("ui_text_button_gameend_restart_value");
        _buttonImage      = Child<Image>("ui_button_gameend_restart_value");
    }
#endif
}
