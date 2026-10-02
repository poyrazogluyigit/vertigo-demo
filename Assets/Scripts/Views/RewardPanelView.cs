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

    protected override void Subscribe()
    {
        Bus.Subscribe<GameReset>(OnGameReset);
        Bus.Subscribe<RewardsChanged>(OnRewardsChanged);
        Bus.Subscribe<CashedOut>(OnCashedOut);
    }

    protected override void Unsubscribe()
    {
        Bus.Unsubscribe<GameReset>(OnGameReset);
        Bus.Unsubscribe<RewardsChanged>(OnRewardsChanged);
        Bus.Unsubscribe<CashedOut>(OnCashedOut);
    }

    void OnGameReset(GameReset _)
    {
        ClearRewardsIn(_inGameRewardsContent);
        ClearRewardsIn(_endgameRewardsContent);
    }

    void OnRewardsChanged(RewardsChanged e) => DisplayAll(e.Earned, _inGameRewardsContent);
    void OnCashedOut(CashedOut e) => DisplayAll(e.Earned, _endgameRewardsContent);

    void DisplayAll(Reward[] rewards, Transform container)
    {
        foreach (Reward reward in rewards)
            Display(reward, container);
    }

    void Display(Reward reward, Transform container)
    {
        Dictionary<RewardDefinition, RewardView> entries = EntriesFor(container);
        if (!entries.TryGetValue(reward.RewardDefn, out var rView))
        {
            RewardView prefab = container == _endgameRewardsContent ? _rcCardPrefab : _rcPrefab;
            entries[reward.RewardDefn] = rView = Instantiate(prefab, container);
        }
        rView.Display(reward.RewardDefn.Image, reward.Amount);
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
