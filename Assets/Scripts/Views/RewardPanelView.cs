using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RewardPanelView : MonoBehaviour
{
    [SerializeField] private RewardView _rcPrefab;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private Button _exitButton;
    public static event System.Action ExitButtonClicked;

    private readonly Dictionary<RewardDefinition, RewardView> _entries = new Dictionary<RewardDefinition, RewardView>();

    void Awake()
    {
        _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
    }

    void OnEnable()
    {
        GameManager.RoundResolved += OnRoundResolved;
    }

    void OnDisable()
    {
        GameManager.RoundResolved -= OnRoundResolved;
    }

    void OnRoundResolved(SpinResult result)
    {
        if (result.IsBomb) return;
        Display(result.Reward);
    }

    public void Display(Reward reward)
    {
        if (!_entries.TryGetValue(reward.RewardDefn, out var rView))
            _entries[reward.RewardDefn] = rView = Instantiate(_rcPrefab, scrollViewContent);
        rView.Display(reward.RewardDefn.image, reward.Amount);
    }
}
