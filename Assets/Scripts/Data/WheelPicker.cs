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
            ValidateWheels(_normalWheels, ZoneType.Normal);
            ValidateWheels(_safeWheels, ZoneType.Safe);
            ValidateWheels(_superWheels, ZoneType.Super);
        }

        void ValidateWheels(WheelContent[] wheels, ZoneType zone)
        {
            if (wheels == null || wheels.Length == 0)
            {
                Debug.LogError($"{name}: needs at least one {zone} wheel", this);
                return;
            }
            foreach (WheelContent wheel in wheels)
            {
                if (wheel == null) Debug.LogError($"{name}: has an empty {zone} wheel entry", this);
                else wheel.ValidateBombs(zone);
            }
        }
    #endif
    }
}
