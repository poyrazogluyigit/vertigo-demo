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
        
    }
}