using UnityEngine;
using System.Collections.Generic;

public class RewardPanelView : View
{
    [SerializeField] private RewardView _rcPrefab;
    [SerializeField] private RewardView _rcCardPrefab;
    [SerializeField] private Transform _inGameRewardsContent;
    [SerializeField] private Transform _endgameRewardsContent;

    // One view cache per container: the same reward shows up in both the in-game
    // list and the endgame list, and each list needs its own RewardView instance.
    private readonly Dictionary<Transform, Dictionary<RewardDefinition, RewardView>> _entries =
        new Dictionary<Transform, Dictionary<RewardDefinition, RewardView>>();

    public void DisplayInGame(Reward reward) => Display(reward, _inGameRewardsContent);
    public void DisplayEndgame(Reward reward) => Display(reward, _endgameRewardsContent);
    public void ClearInGameRewards() => ClearRewardsIn(_inGameRewardsContent);
    public void ClearEndgameRewards() => ClearRewardsIn(_endgameRewardsContent);

    void Display(Reward reward, Transform container)
    {
        Dictionary<RewardDefinition, RewardView> entries = EntriesFor(container);
        if (!entries.TryGetValue(reward.RewardDefn, out var rView))
        {
            RewardView prefab = container == _endgameRewardsContent ? _rcCardPrefab : _rcPrefab;
            entries[reward.RewardDefn] = rView = Instantiate(prefab, container);
        }
        rView.Display(reward.RewardDefn.image, reward.Amount);
    }

    void ClearRewardsIn(Transform container)
    {
        Dictionary<RewardDefinition, RewardView> entries = EntriesFor(container);
        foreach (RewardView rView in entries.Values)
        {
            // Unparent first: Destroy is deferred to end of frame, and a pending
            // child would still be counted by the layout group this frame.
            rView.transform.SetParent(null);
            Destroy(rView.gameObject);
        }
        entries.Clear();
    }

    private Dictionary<RewardDefinition, RewardView> EntriesFor(Transform container)
    {
        if (!_entries.TryGetValue(container, out var entries))
            _entries[container] = entries = new Dictionary<RewardDefinition, RewardView>();
        return entries;
    }
}
