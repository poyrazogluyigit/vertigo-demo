using UnityEngine;

// Hands the event bus to every view in the scene; the views subscribe to what they show.
public class ViewManager : MonoBehaviour
{
    [SerializeField] private View[] _views;

#if UNITY_EDITOR
    // includeInactive: the endgame view starts disabled
    void OnValidate()
    {
        _views = FindObjectsOfType<View>(true);
        if (_views.Length == 0) Debug.LogError("No views found in the scene!", this);
    }
#endif

    public void Bind(IEventBus bus)
    {
        foreach (View view in _views)
            view.Bind(bus);
    }

    // Views that never became active get no OnDestroy, so they are unbound from here.
    // ReferenceEquals: during scene unload a view may already be destroyed (Unity's == null
    // is true), but Unbind only touches the C# side, so it still has to run.
    void OnDestroy()
    {
        foreach (View view in _views)
            if (!ReferenceEquals(view, null)) view.Unbind();
    }
}
