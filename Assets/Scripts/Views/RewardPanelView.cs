using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RewardPanelView : MonoBehaviour
{
    [SerializeField] private RewardView _rcPrefab;
    [SerializeField] private RewardIconLibrary _iconLibrary;

    [SerializeField] private Button _exitButton;
    public static event System.Action ExitButtonClicked;

    private readonly Dictionary<int, RewardView> _entries = new Dictionary<int, RewardView>();

    void Awake()
    {
        _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
    }

    void OnEnable()
    {
        RewardsManager.EarnedRewardsChanged += Display;
    }

    void OnDisable()
    {
        RewardsManager.EarnedRewardsChanged -= Display;
    }

    public void Display(IReadOnlyDictionary<int, int> rewards)
    {
        foreach (var pair in rewards)
        {
            if (!_entries.TryGetValue(pair.Key, out var rView))
                _entries[pair.Key] = rView = Instantiate(_rcPrefab, transform);
            rView.Display(_iconLibrary.GetSprite(pair.Key), pair.Value);
        }
    }
}
