using UnityEngine;

// Spins a decorative UI element at a constant speed, e.g. the sunburst behind the wheel.
public class RotateForever : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = -10f;

    void Update() => transform.Rotate(0f, 0f, degreesPerSecond * Time.deltaTime);
}
