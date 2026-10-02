using UnityEngine;
using UnityEngine.Serialization;

namespace WheelSpin
{
    // How the level track looks for each zone type.
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

        public Color NumberColorFor(ZoneType zone)
        {
            switch (zone)
            {
                case ZoneType.Super: return _superZoneColor;
                case ZoneType.Safe: return _safeZoneColor;
                default: return _normalZoneColor;
            }
        }

        // Shown behind the current level's number
        public Sprite BackgroundFor(ZoneType zone)
        {
            switch (zone)
            {
                case ZoneType.Super: return _currentSuperZoneBackground;
                case ZoneType.Safe: return _currentSafeZoneBackground;
                default: return _currentNormalZoneBackground;
            }
        }
    }
}
