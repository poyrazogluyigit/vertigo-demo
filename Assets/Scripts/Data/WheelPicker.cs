using UnityEngine;

namespace WheelSpin
{
    [CreateAssetMenu(fileName = "New Wheel Picker", menuName = "WheelSpin/Wheel Picker")]
    public class WheelPicker : ScriptableObject, IWheelSchedule
    {
        [SerializeField] private WheelContent[] _superWheels = new WheelContent[1];
        [SerializeField] private WheelContent[] _safeWheels = new WheelContent[1];
        [SerializeField] private WheelContent[] _normalWheels = new WheelContent[1];


        public WheelContent WheelFor(int level)
        {
            switch (Zones.TypeOf(level))
            {
                case ZoneType.Super: return RandomSelection(_superWheels);
                case ZoneType.Safe: return RandomSelection(_safeWheels);
                default: return RandomSelection(_normalWheels);
            }
        }
        static WheelContent RandomSelection(WheelContent[] wheels) => wheels[Random.Range(0, wheels.Length)];


    #if UNITY_EDITOR
        void OnValidate()
        {
            if (!CheckWheelsNotNull()) return;
            foreach (var wheel in _normalWheels) 
                IsWheelValid(wheel, ZoneType.Normal);
            foreach (var wheel in _safeWheels)
                IsWheelValid(wheel, ZoneType.Safe);
            foreach (var wheel in _superWheels) 
                IsWheelValid(wheel, ZoneType.Super);
        }
        public static void IsWheelValid(WheelContent wheel, ZoneType zone)
        {
            int expected = Zones.BombsPerWheel(zone);
            if (wheel.BombCount != expected)
                Debug.LogError($"{wheel.name} has {wheel.BombCount} bombs, a {zone} wheel needs {expected}");
        }

        bool CheckWheelsNotNull()
        {
            if (_normalWheels == null) Debug.LogError($"{name}: needs at least one normal wheel", this);
            if (_safeWheels == null) Debug.LogError($"{name}: needs at least one safe wheel", this);
            if (_superWheels == null) Debug.LogError($"{name}: needs at least one super wheel", this);
            return !(_normalWheels == null || _safeWheels == null || _superWheels == null);
        }
    #endif
    }
}
