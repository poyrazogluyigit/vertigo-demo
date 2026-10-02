using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LevelPanelView : View
{
    const int PinnedSlot = 15;
    const float NumberSpacing = 135f;     // distance between two numbers on the track
    const float MoveDuration = 1f;
    const float PassedLevelAlpha = 0.4f;

    [SerializeField] private RectTransform _levelNumbers;
    [SerializeField] private LevelPanelSetting _levelPanelSettings;
    [SerializeField] private Image _currentZoneBg;

    private TMP_Text[] _labels;
    private Vector2 _initialPosition;
    private int _firstNumber = Zones.FirstLevel;   // the level shown by the leftmost label

    void Awake()
    {
        _labels = _levelNumbers.GetComponentsInChildren<TMP_Text>();
        _initialPosition = _levelNumbers.anchoredPosition;
    }

    protected override void Subscribe()
    {
        Bus.Subscribe<GameReset>(OnGameReset);
        Bus.Subscribe<LevelStarted>(OnLevelStarted);
    }

    protected override void Unsubscribe()
    {
        Bus.Unsubscribe<GameReset>(OnGameReset);
        Bus.Unsubscribe<LevelStarted>(OnLevelStarted);
    }

    void OnGameReset(GameReset _) => ResetIndicator();

    void OnLevelStarted(LevelStarted e) =>
        ChangeLevelTo(e.Level).OnComplete(() => Bus?.Publish(new LevelShown()));

    void ResetIndicator()
    {
        _firstNumber = Zones.FirstLevel;
        _levelNumbers.anchoredPosition = _initialPosition;
        RenderLevelsFrom(Zones.FirstLevel);
    }

    Tween ChangeLevelTo(int level)
    {
        int slot = level - _firstNumber;
        if (slot > PinnedSlot)
        {
            _firstNumber = level - PinnedSlot;
            RenderLevelsFrom(level);
            SnapIndicatorTo(PinnedSlot - 1);   // the previous level is back where the track already was
            slot = PinnedSlot;
        }
        else
        {
            RenderLevelsFrom(level);
        }
        return SlideIndicatorTo(slot);
    }

    void RenderLevelsFrom(int currentLevel)
    {
        _currentZoneBg.sprite = _levelPanelSettings.BackgroundFor(Zones.TypeOf(currentLevel));
        for (int i = 0; i < _labels.Length; i++)
        {
            int number = _firstNumber + i;
            Color color = _levelPanelSettings.NumberColorFor(Zones.TypeOf(number));
            color.a = number < currentLevel ? PassedLevelAlpha : 1f;
            _labels[i].text = number.ToString();
            _labels[i].color = color;
        }
    }

    Vector2 PositionOf(int slot) => _initialPosition + new Vector2(-slot * NumberSpacing, 0f);

    void SnapIndicatorTo(int slot) => _levelNumbers.anchoredPosition = PositionOf(slot);

    Tween SlideIndicatorTo(int slot) =>
        _levelNumbers.DOAnchorPos(PositionOf(slot), MoveDuration)
            .SetLink(gameObject);

#if UNITY_EDITOR
    void OnValidate()
    {
        _levelNumbers = Child<RectTransform>("ui_group_level_track");
        _currentZoneBg = Child<Image>("ui_image_level_current_bg_value");
    }
#endif
}
