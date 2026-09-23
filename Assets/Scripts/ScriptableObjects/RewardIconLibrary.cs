using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(menuName = "Wheel/Reward Icon Library")]
public class RewardIconLibrary : ScriptableObject
{
    [System.Serializable] public struct RewardIcon { public int id; public Sprite sprite; }

    [SerializeField] private List<RewardIcon> icons = new List<RewardIcon>();
    [SerializeField] private Sprite fallback;
    private Dictionary<int, Sprite> _byId;

    public Sprite GetSprite(int id)
    {
        _byId ??= icons.ToDictionary(i => i.id, i => i.sprite);
        return _byId.TryGetValue(id, out var s) ? s : fallback;
    }

    void OnValidate() => _byId = null;
}