using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WheelSpin
{
    public class EndgameView : View
    {
        [SerializeField] private EndScreen _gameOverScreen, _cashOutScreen;
        [SerializeField] private GameObject _rewardsContainer, _bombContainer;
        [SerializeField] private TMP_Text _title, _description, _buttonLabel;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private RewardList _rewards;
        [SerializeField] private ScrollRect _rewardsScroll;

        protected override void Subscribe()
        {
            Bus.Subscribe<GameReset>(OnGameReset);
            Bus.Subscribe<CashedOut>(OnCashedOut);
            Bus.Subscribe<BombHit>(OnBombHit);
        }

        protected override void Unsubscribe()
        {
            Bus.Unsubscribe<GameReset>(OnGameReset);
            Bus.Unsubscribe<CashedOut>(OnCashedOut);
            Bus.Unsubscribe<BombHit>(OnBombHit);
        }

        void OnGameReset(GameReset _) => gameObject.SetActive(false);
        void OnCashedOut(CashedOut e)
        {
            _rewards.Draw(e.Earned);
            DisplayScreen(_cashOutScreen);
        }

        void OnBombHit(BombHit _) => DisplayScreen(_gameOverScreen);

        void DisplayScreen(EndScreen screen)
        {
            _rewardsScroll.verticalNormalizedPosition = 0;
            _title.text = screen.Title;
            _description.text = screen.Description;
            _buttonLabel.text = screen.ButtonLabel;
            _buttonImage.sprite = screen.ButtonSprite;
            _rewardsContainer.SetActive(screen.ShowsRewards);
            _bombContainer.SetActive(!screen.ShowsRewards);
            gameObject.SetActive(true);
        }

        #if UNITY_EDITOR
        void OnValidate()
        {
            _rewardsContainer = Child<Transform>("ui_container_exit")?.gameObject;
            _bombContainer    = Child<Transform>("ui_container_gameover")?.gameObject;
            _title            = Child<TMP_Text>("ui_text_gameend_title_value");
            _description      = Child<TMP_Text>("ui_text_gameend_desc_value");
            _buttonLabel      = Child<TMP_Text>("ui_text_button_gameend_restart_value");
            _buttonImage      = Child<Image>("ui_button_gameend_restart_value");
            _rewards          = Child<RewardList>("ui_content_exit_rewards");
            _rewardsScroll    = Child<ScrollRect>("ui_scrollview_exit_rewards");

            if (_gameOverScreen == null) Debug.LogError("Game over screen is not assigned!", this);
            if (_cashOutScreen == null) Debug.LogError("Cash out screen is not assigned!", this);
        }
    #endif
    }
}
