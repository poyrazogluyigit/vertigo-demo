using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsManager : MonoBehaviour
{
    public Dictionary<RewardSO, int> rewards = new Dictionary<RewardSO, int>();
    [SerializeField] private Button _exitButton;
    public void AddReward(RewardSO reward, int amt)
    {
        if (rewards.ContainsKey(reward))
            rewards[reward] += amt;
        else
            rewards[reward] = amt;
        UpdateRewardsPanel();
    }

    void UpdateRewardsPanel()
    {
        
    }
}
