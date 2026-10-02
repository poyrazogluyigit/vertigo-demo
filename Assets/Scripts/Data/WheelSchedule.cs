using UnityEngine;

namespace WheelSpin
{
    // Decides which wheel each level uses. Normal zones rotate through _normalWheels,
    // so adding a wheel asset there adds variety without any code change.
    [CreateAssetMenu(fileName = "NewWheelSchedule", menuName = "WheelSpin/Wheel Schedule")]
    public class WheelSchedule : ScriptableObject, IWheelSchedule
    {
        [SerializeField] private WheelSO _superWheel;
        [SerializeField] private WheelSO _safeWheel;
        [SerializeField] private WheelSO[] _normalWheels = new WheelSO[1];

        public WheelSO WheelFor(int level)
        {
            switch (Zones.TypeOf(level))
            {
                case ZoneType.Super: return _superWheel;
                case ZoneType.Safe: return _safeWheel;
                default: return _normalWheels[(level - Zones.FirstLevel) % _normalWheels.Length];
            }
        }

        // Why a wheel can't serve a zone type, or null if it can.
        public static string CheckWheel(WheelSO wheel, ZoneType zone)
        {
            if (wheel == null) return $"a {zone} wheel is not assigned";
            int expected = Zones.BombsPerWheel(zone);
            if (wheel.BombCount != expected)
                return $"{wheel.name} has {wheel.BombCount} bombs, a {zone} wheel needs {expected}";
            return null;
        }

    #if UNITY_EDITOR
        // Runs when this asset is edited; changing a wheel's slices alone won't re-run it.
        void OnValidate()
        {
            if (_normalWheels == null || _normalWheels.Length == 0)
            {
                Debug.LogError($"{name}: needs at least one normal wheel", this);
                return;
            }
            Report(CheckWheel(_superWheel, ZoneType.Super));
            Report(CheckWheel(_safeWheel, ZoneType.Safe));
            foreach (WheelSO wheel in _normalWheels)
                Report(CheckWheel(wheel, ZoneType.Normal));
        }

        void Report(string problem)
        {
            if (problem != null) Debug.LogError($"{name}: {problem}", this);
        }
    #endif
    }
}
