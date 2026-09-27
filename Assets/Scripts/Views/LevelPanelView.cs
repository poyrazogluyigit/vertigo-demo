using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;

public class LevelPanelView : View
{
    [SerializeField] RectTransform _levelNumbers;
    [SerializeField] LevelPanelSetting _levelPanelSettings;
    [SerializeField] Image _currentZoneBg;
    private Vector3 _initialPosition;
    void Awake() => _initialPosition = _levelNumbers.anchoredPosition;

    public void ResetIndicator()
    {
        _levelNumbers.anchoredPosition = _initialPosition;
        int i = 0;
        foreach (var t in _levelNumbers.GetComponentsInChildren<TMP_Text>())
            t.text = (++i).ToString();
        SetNumberStyle(1);
    }

    // here we use a hack to keep reusing the same text objects
    // the numbers are increased by one and moved to the right before
    // moving to the left

    const int PinnedSlot = 15;

    public async Task ChangeLevelTo(int level, bool instant = false)
    {
    int slot = level - 1;
    if (slot > PinnedSlot)
    {
        IncreaseNumbers();                            // slot 15 now shows `level`, slot 14 shows level-1
        await MoveLevelIndicator(PinnedSlot - 1, true); // snap back: looks unchanged
        slot = PinnedSlot;
    }
    SetNumberStyle(level);                            // after the renumber, so colours match
    await MoveLevelIndicator(slot, instant);
    }

    async Task MoveLevelIndicator(int targetTextBoxIndex, bool cancelAnimation = false)
    {
        float xDelta = 135;
        Vector3 target = _initialPosition + new Vector3(-targetTextBoxIndex * xDelta, 0, 0);
        if (!cancelAnimation) await _levelNumbers.DOAnchorPos(target, 1f)
        .AsyncWaitForCompletion();
        else _levelNumbers.anchoredPosition = target;
    }

    int GetLevelType(int level)
    {
        if (level % 30 == 0) return 0;
        else if (level % 5 == 0 || level == 1) return 1;
        else return 2;
    }

    void IncreaseNumbers()
    {
        foreach (var t in _levelNumbers.GetComponentsInChildren<TMP_Text>())
        {
            int currentNumber = int.Parse(t.text);
            t.text = (currentNumber + 1).ToString();
        }
    }

    void SetNumberStyle(int level)
    {
        _currentZoneBg.sprite = _levelPanelSettings.backgrounds[GetLevelType(level)];
        foreach (var t in _levelNumbers.GetComponentsInChildren<TMP_Text>())
        {
            Color c = _levelPanelSettings.colors[GetLevelType(int.Parse(t.text))];
            c.a = int.Parse(t.text) < level ? 0.4f : 1f;
            t.color = c;
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        _levelNumbers = Child<RectTransform>("ui_group_level_track");
        _currentZoneBg = Child<Image>("ui_image_level_current_bg_value");
    }
#endif
}
