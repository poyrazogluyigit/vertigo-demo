using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
public class LevelPanelView : View
{
    [SerializeField] RectTransform _levelNumbers;
    private Vector3 _initialPosition;
    void Awake() => _initialPosition = _levelNumbers.anchoredPosition;

    public async Task ChangeLevel(int level)
    {
        await MoveLevelIndicator(level);
        ColorZoneNumbers(level);
    }
    public void ResetIndicator()
    {
        _levelNumbers.anchoredPosition = _initialPosition;
        ColorZoneNumbers(1);
    }
    async Task MoveLevelIndicator(int level)
    {
        float xDelta = 135;
        Vector3 target = _initialPosition + new Vector3(-(level - 1) * xDelta, 0, 0);
        Debug.Log(target);
        await _levelNumbers.DOAnchorPos(target, 1f)
        .AsyncWaitForCompletion();
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