using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;

public class LevelPanelView : View
{
    [SerializeField] RectTransform _levelNumbers;
    [SerializeField] LevelPanelSetting _levelPanelSettings;
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

    public async Task ChangeLevel(int level, bool cancelAnimation = false)
    {
        if (level > 15) InfiniteSlideSetup(level);
        await MoveLevelIndicator(level, cancelAnimation);
    }

    async Task MoveLevelIndicator(int level, bool cancelAnimation = false)
    {
        float xDelta = 135;
        Vector3 target = _initialPosition + new Vector3(-(level - 1) * xDelta, 0, 0);
        Debug.Log(target);
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
        foreach (var t in _levelNumbers.GetComponentsInChildren<TMP_Text>())
        {
            Color c = _levelPanelSettings.colors[GetLevelType(int.Parse(t.text))];
            c.a = int.Parse(t.text) < level - 1 ? 0.4f : 1f;
            t.color = c;
        }
    }

    // a hack is used to keep reusing same components
    void InfiniteSlideSetup(int level)
    {
        IncreaseNumbers();
        SetNumberStyle(level);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        _levelNumbers    = Child<RectTransform>("ui_group_level_track");
    }
#endif
}
