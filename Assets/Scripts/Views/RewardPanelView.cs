using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RewardPanelView : MonoBehaviour
{
    [SerializeField] private RewardView _rcPrefab;

    [SerializeField] private Button _exitButton;
    public static event System.Action ExitButtonClicked;
    public void Display(Dictionary<RewardData, int> rewards)
    {
        foreach (var key in rewards.Keys)
        {
            var rView = Instantiate(_rcPrefab, transform);
            rView.Display(key, rewards[key]);
        }
    }
}