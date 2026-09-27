using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="NewLevelPanelSetting", menuName = "WheelSpin/LevelPanelSetting")]
public class LevelPanelSetting : ScriptableObject
{
    [SerializeField] private Color normalZoneColor;
    [SerializeField] private Sprite currentNormalZoneBackground;
    [SerializeField] private Color safeZoneColor;
    [SerializeField] private Sprite currentSafeZoneBackground;
    [SerializeField] private Color superZoneColor;
    [SerializeField] private Sprite currentSuperZoneBackground;
    public List<Sprite> backgrounds;
    public List<Color> colors;

    void Awake()
    {
        backgrounds = new List<Sprite>
        {
            currentSuperZoneBackground,
            currentSafeZoneBackground,
            currentNormalZoneBackground
        };
        colors = new List<Color>
        {
            superZoneColor,
            safeZoneColor,
            normalZoneColor
        };
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        Awake();
    }
#endif
}
