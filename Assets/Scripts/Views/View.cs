using UnityEngine;

public class View : MonoBehaviour
{
    protected IEventBus Bus { get; private set; }
    public void Bind(IEventBus bus)
    {
        Bus = bus;
        Subscribe();
    }

    public void Unbind()
    {
        if (Bus == null) return;
        Unsubscribe();
        Bus = null;
    }

    protected virtual void Subscribe() { }
    protected virtual void Unsubscribe() { }

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
