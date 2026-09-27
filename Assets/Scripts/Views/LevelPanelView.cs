using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;
public class LevelPanelView : View
{
    [SerializeField] RectTransform _levelNumbers;
    [SerializeField] Image _currentZoneTile;
    [SerializeField] Image _upcomingTilePrefab;
    private Vector3 _initialPosition;
    private readonly Dictionary<int, Image> _upcomingTiles = new Dictionary<int, Image>();
    void Awake() => _initialPosition = _levelNumbers.anchoredPosition;

    public async Task ChangeLevel(int level)
    {
        await MoveLevelIndicator(level);
        RefreshZones(level);
    }
    public void ResetIndicator()
    {
        _levelNumbers.anchoredPosition = _initialPosition;
        RefreshZones(1);
    }

    public void DrawCurrentZone(WheelSO wheel)
    {
        _currentZoneTile.sprite = wheel.ZoneTileSprite;
        _currentZoneTile.color = wheel.ZoneTileColor;
    }

    // Marks special zones ahead of the player. Each tile is a track child placed before
    // the numbers, so it draws behind its number and slides with the track.
    public void BuildUpcomingTiles(Func<int, WheelSO> wheelForZone)
    {
        foreach (TMP_Text label in _levelNumbers.GetComponentsInChildren<TMP_Text>(true))
        {
            if (!TryGetZone(label, out int zone)) continue;
            Color tint = wheelForZone(zone).UpcomingTileColor;
            if (tint.a <= 0f) continue;

            Image tile = Instantiate(_upcomingTilePrefab, _levelNumbers);
            tile.name = $"ui_image_level_upcoming_{zone:00}";
            tile.color = tint;
            tile.rectTransform.anchoredPosition = new Vector2(label.rectTransform.rect.width * (zone - 0.5f), 0f);
            tile.transform.SetAsFirstSibling();
            _upcomingTiles[zone] = tile;
        }
    }

    async Task MoveLevelIndicator(int level)
    {
        float xDelta = 135;
        Vector3 target = _initialPosition + new Vector3(-(level - 1) * xDelta, 0, 0);
        Debug.Log(target);
        await _levelNumbers.DOAnchorPos(target, 1f)
        .AsyncWaitForCompletion();
    }

    void RefreshZones(int level)
    {
        foreach (TMP_Text label in _levelNumbers.GetComponentsInChildren<TMP_Text>(true))
        {
            if (!TryGetZone(label, out int zone)) continue;
            Color c = label.color;
            c.a = zone < level ? 0.4f : 1f;
            label.color = c;
        }
        // Reached zones lose their tile; the current zone shows the current-zone tile instead
        foreach (KeyValuePair<int, Image> entry in _upcomingTiles)
            entry.Value.gameObject.SetActive(entry.Key > level);
    }

    static bool TryGetZone(TMP_Text label, out int zone)
    {
        string[] parts = label.name.Split('_');
        return int.TryParse(parts[parts.Length - 2], out zone);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        _levelNumbers    = Child<RectTransform>("ui_group_level_track");
        _currentZoneTile = Child<Image>("ui_image_level_current_bg");
    }
#endif
}
