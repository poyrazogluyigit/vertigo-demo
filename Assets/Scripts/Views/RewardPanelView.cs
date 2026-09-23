using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RewardPanelView : MonoBehaviour
{
    [SerializeField] private RewardView _rcPrefab;
    [SerializeField] private RewardIconLibrary _iconLibrary;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private Button _exitButton;
    public static event System.Action ExitButtonClicked;

    private readonly Dictionary<int, RewardView> _entries = new Dictionary<int, RewardView>();

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
        Display(result.Reward.Id, result.EarnedTotal);
    }

    public void Display(int rewardId, int total)
    {
        if (!_entries.TryGetValue(rewardId, out var rView))
            _entries[rewardId] = rView = Instantiate(_rcPrefab, scrollViewContent);
        rView.Display(_iconLibrary.GetSprite(rewardId), total);
    }
}
