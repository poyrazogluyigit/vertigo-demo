using UnityEngine;
using UnityEngine.Serialization;

namespace WheelSpin
{
    // Spins a decorative UI element at a constant speed, e.g. the sunburst behind the wheel.
    public class RotateForever : MonoBehaviour
    {
        [FormerlySerializedAs("degreesPerSecond")]
        [SerializeField] private float _degreesPerSecond = -10f;

        void Update() => transform.Rotate(0f, 0f, _degreesPerSecond * Time.deltaTime);
    }
}
