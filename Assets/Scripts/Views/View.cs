using UnityEngine;

public class View : MonoBehaviour
{
#if UNITY_EDITOR
    // Finds a component on a descendant by GameObject name, for OnValidate wiring.
    // Searches only this view's hierarchy (inactive included), so it also works inside prefabs.
    protected T Child<T>(string childName) where T : Component
    {
        foreach (T c in GetComponentsInChildren<T>(true))
            if (c.name == childName) return c;
        Debug.LogError($"{GetType().Name}: '{childName}' not found under '{name}'", this);
        return null;
    }
#endif
}
