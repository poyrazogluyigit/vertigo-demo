using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RewardsManager : MonoBehaviour
{
    public Dictionary<RewardSO, RewardCard> rewards = new Dictionary<RewardSO, RewardCard>();
    [SerializeField] private Button _exitButton;
    [SerializeField] private RewardCard _rcPrefab;
    public void AddReward(RewardSO reward, int amt)
    {
        if (rewards.ContainsKey(reward))
            rewards[reward].amount += amt;
        else
        {
            var rewardCard = Instantiate(_rcPrefab, transform);
            rewardCard.amount = amt;
            rewards[reward] = rewardCard;
        }
        rewards[reward].Display(reward);
    }
}
