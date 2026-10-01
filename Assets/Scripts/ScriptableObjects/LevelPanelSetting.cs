using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName="NewLevelPanelSetting", menuName = "WheelSpin/LevelPanelSetting")]
public class LevelPanelSetting : ScriptableObject
{
    [FormerlySerializedAs("normalZoneColor")]
    [SerializeField] private Color _normalZoneColor;
    [FormerlySerializedAs("currentNormalZoneBackground")]
    [SerializeField] private Sprite _currentNormalZoneBackground;
    [FormerlySerializedAs("safeZoneColor")]
    [SerializeField] private Color _safeZoneColor;
    [FormerlySerializedAs("currentSafeZoneBackground")]
    [SerializeField] private Sprite _currentSafeZoneBackground;
    [FormerlySerializedAs("superZoneColor")]
    [SerializeField] private Color _superZoneColor;
    [FormerlySerializedAs("currentSuperZoneBackground")]
    [SerializeField] private Sprite _currentSuperZoneBackground;
    [FormerlySerializedAs("backgrounds")]
    public List<Sprite> Backgrounds;
    [FormerlySerializedAs("colors")]
    public List<Color> Colors;

    void OnEnable()
    {
        Backgrounds = new List<Sprite>
        {
            _currentSuperZoneBackground,
            _currentSafeZoneBackground,
            _currentNormalZoneBackground
        };
        Colors = new List<Color>
        {
            _superZoneColor,
            _safeZoneColor,
            _normalZoneColor
        };
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        OnEnable();
    }
#endif
}
