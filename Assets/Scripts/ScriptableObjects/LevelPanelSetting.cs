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

#if UNITY_EDITOR
    void OnValidate()
    {
        backgrounds[0] = currentSuperZoneBackground;
        backgrounds[1] = currentSafeZoneBackground;
        backgrounds[2] = currentNormalZoneBackground;

        colors[0] = superZoneColor;
        colors[1] = safeZoneColor;
        colors[2] = normalZoneColor;
    }
#endif
}
