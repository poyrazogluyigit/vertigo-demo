using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RewardPanelView : MonoBehaviour
{
    [SerializeField] private RewardView _rcPrefab;
    [SerializeField] private Transform _inGameRewardsContent;
    [SerializeField] private Transform _endgameRewardsContent;
    [SerializeField] private Button _exitButton;
    public static event System.Action ExitButtonClicked;

    // One view cache per container: the same reward shows up in both the in-game
    // list and the endgame list, and each list needs its own RewardView instance.
    private readonly Dictionary<Transform, Dictionary<RewardDefinition, RewardView>> _entries =
        new Dictionary<Transform, Dictionary<RewardDefinition, RewardView>>();

    void Awake()
    {
        _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
    }

    void OnEnable()
    {
        GameManager.RoundResolved += OnRoundResolved;
        RewardsManager.RewardsCollected += OnRewardsCollected;
        GameManager.GameRestarted += OnGameRestarted;
        GameManager.SpinStarted += OnSpinStarted;
    }

    void OnDisable()
    {
        GameManager.RoundResolved -= OnRoundResolved;
        RewardsManager.RewardsCollected -= OnRewardsCollected;
        GameManager.GameRestarted -= OnGameRestarted;
        GameManager.SpinStarted -= OnSpinStarted;
    }

    // Bailing out mid-spin would end the run on a result that is already
    // committed but not yet shown, so the exit stays locked until it lands.
    void OnSpinStarted(int slot) => _exitButton.interactable = false;

    void OnRoundResolved(SpinResult result)
    {
        _exitButton.interactable = true;
        if (result.IsBomb) return;
        Display(result.Reward, _inGameRewardsContent);
    }

    void OnGameRestarted()
    {
        _exitButton.interactable = true;
        Clear(_inGameRewardsContent);
    }

    void OnRewardsCollected(Reward[] rewards)
    {
        // The endgame list reflects a single run, so rebuild it from scratch.
        Clear(_endgameRewardsContent);
        foreach (var reward in rewards)
            Display(reward, _endgameRewardsContent);
    }

    public void Display(Reward reward, Transform container)
    {
        Dictionary<RewardDefinition, RewardView> entries = EntriesFor(container);
        if (!entries.TryGetValue(reward.RewardDefn, out var rView))
            entries[reward.RewardDefn] = rView = Instantiate(_rcPrefab, container);
        rView.Display(reward.RewardDefn.image, reward.Amount);
    }

    public void Clear(Transform container)
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
