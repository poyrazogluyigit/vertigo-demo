using System.Threading.Tasks;
using UnityEngine;

public readonly struct SpinResult
{
    public readonly RewardData Reward;
    public readonly int Amount;

    public SpinResult(RewardData reward, int amount)
    {
        Reward = reward;
        Amount = amount;
    }
}

[RequireComponent(typeof(Wheel))]
public class LevelManager : MonoBehaviour
{
    private const int MaxLevel = 30;
    private const int WheelSegmentCount = 8;

    [SerializeField] private WheelSO safeWheel;
    [SerializeField] private WheelSO normalWheel;
    [SerializeField] private WheelSO superWheel;
    [SerializeField] private Wheel _wheel;
    [SerializeField] private RewardPool _rewardPool;
    [SerializeField] private RewardData _hazardReward;

    private readonly RewardData[] _currentSlots = new RewardData[WheelSegmentCount];
    private int _currentLevel = 1;

    private static LevelType GetLevelType(int level)
    {
        if (level == MaxLevel) return LevelType.Super;
        if (level == 1 || level % 5 == 0) return LevelType.Safe;
        return LevelType.Normal;
    }

    private static RewardTier GetTierBand(int level)
    {
        if (level >= MaxLevel) return RewardTier.T3;
        if (level >= 16) return RewardTier.T2;
        return RewardTier.T1;
    }

    private void GenerateRewards(int level)
    {
        for (int i = 0; i < _currentSlots.Length; i++)
            _currentSlots[i] = null;

        if (_rewardPool == null)
            return;

        LevelType levelType = GetLevelType(level);
        RewardTier tierBand = GetTierBand(level);

        int slotIndex = 0;

        // Bomb is deterministic: always present on Normal levels, never on Safe/Super.
        if (levelType == LevelType.Normal && _hazardReward != null)
            _currentSlots[slotIndex++] = _hazardReward;

        // Super/T3 item is deterministic on the Super level, mirroring the bomb's forced presence.
        // Assumption: "super levels will have a super item, others will not" reads as a guarantee,
        // not just elevated odds, so it's forced in the same way the bomb is.
        if (levelType == LevelType.Super)
        {
            var superPick = _rewardPool.PickWeightedStrictTier(level, levelType, RewardTier.T3, 1);
            if (superPick.Count > 0)
                _currentSlots[slotIndex++] = superPick[0];
        }

        int remaining = WheelSegmentCount - slotIndex;
        if (remaining > 0)
        {
            var exclude = new System.Collections.Generic.List<RewardData>();
            for (int i = 0; i < slotIndex; i++)
                exclude.Add(_currentSlots[i]);

            var picks = _rewardPool.PickWeighted(level, levelType, tierBand, remaining, exclude);
            foreach (var pick in picks)
                _currentSlots[slotIndex++] = pick;
        }
        // If the pool doesn't have enough eligible rewards to fill every slot, the remainder
        // stays null — Wheel.Draw and Play() both tolerate that.
    }

    public async Task<SpinResult> Play()
    {
        int chosenIndex = _rewardPool != null
            ? _rewardPool.PickWinningIndex(_currentSlots, _currentLevel)
            : Random.Range(0, WheelSegmentCount);

        Debug.Log($"Chosen index: {chosenIndex}");
        await _wheel.Spin(chosenIndex);

        RewardData reward = _currentSlots[chosenIndex];
        int amount = _rewardPool != null ? _rewardPool.GetAmount(reward, _currentLevel) : 1;
        return new SpinResult(reward, amount);
    }

    public void SetLevel(int level)
    {
        _currentLevel = level;
        GenerateRewards(level);

        WheelSO wheelSkin = GetLevelType(level) switch
        {
            LevelType.Safe => safeWheel,
            LevelType.Super => superWheel,
            _ => normalWheel,
        };
        _wheel.Draw(wheelSkin, _currentSlots);
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        if (_wheel != null && normalWheel != null)
            _wheel.Draw(normalWheel, _currentSlots);
    }
#endif
}
