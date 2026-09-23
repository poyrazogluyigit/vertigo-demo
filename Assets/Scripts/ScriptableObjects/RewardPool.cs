using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class RewardBase
{
    public int id;
    public string rewardName;
    public float baseAmount = 1f;
    public float weight = 1f;
    public bool isBomb = false;
}



[CreateAssetMenu(menuName = "Wheel/Reward Pool")]
public class RewardPool : ScriptableObject
{
    [SerializeField] private List<RewardBase> rewards  = new List<RewardBase>();
    [SerializeField] private AnimationCurve amountCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
    [SerializeField] private int maxLevel = 30;
    [SerializeField] public int BombId = 4;

    private Dictionary<int, RewardBase> _byId;
    public IReadOnlyList<RewardBase> Rewards => rewards;

    private Dictionary<int, RewardBase> ById => _byId ??= rewards.ToDictionary(r => r.id);

    public int CalculateAmount(int id, int level)
    {
        float t = maxLevel > 0 ? (float)level / maxLevel : 0f;
        RewardBase rBase = ById[id];
        return Mathf.Max(1, Mathf.RoundToInt(rBase.baseAmount * amountCurve.Evaluate(t)));
    }

    public bool isBomb(int id) => ById[id].isBomb;

#if UNITY_EDITOR
    void OnValidate()
    {
        _byId = null;
        foreach (var dupe in rewards.GroupBy(r => r.id).Where(g => g.Count() > 1))
            Debug.LogError($"RewardPool: duplicate reward id {dupe.Key}", this);
    }
#endif
}
