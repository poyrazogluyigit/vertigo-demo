using UnityEngine;
using DG.Tweening;
using TMPro;
public class LevelPanelView : MonoBehaviour
{
    [SerializeField] RectTransform _levelNumbers;
    private Vector3 _initialPosition;
    void Awake() => _initialPosition = _levelNumbers.anchoredPosition;
    void OnEnable() => GameManager.LevelChanged += OnLevelChanged;
    void OnDisable() => GameManager.LevelChanged -= OnLevelChanged;


    void OnLevelChanged(int level)
    {
        MoveLevelIndicator(level);
        ColorZoneNumbers(level);
    }
    void MoveLevelIndicator(int level)
    {
        float xDelta = 135;
        Vector3 target = _initialPosition + new Vector3(-(level - 1) * xDelta, 0, 0);
        Debug.Log(target);
        _levelNumbers.DOAnchorPos(target, 1f);
    }

    void ColorZoneNumbers(int level)
    {
        foreach (TMP_Text label in _levelNumbers.GetComponentsInChildren<TMP_Text>(true))
        {
            string[] parts = label.name.Split('_');
            if (!int.TryParse(parts[parts.Length - 2], out int zone)) continue;
            Color c = label.color;
            c.a = zone < level ? 0.4f : 1f;
            label.color = c;
        }
    }
}