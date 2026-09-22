using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RewardsManager : MonoBehaviour
{
    private Dictionary<RewardType, RewardCard> rewards = new Dictionary<RewardType, RewardCard>();
    [SerializeField] private Button _exitButton;
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private RewardCard _rcPrefab;
    public void AddReward(RewardType reward, int amt)
    {
        if (!rewards.ContainsKey(reward))
            rewards[reward] = Instantiate(_rcPrefab, _contentContainer);
        rewards[reward].AddAmount(amt);
        rewards[reward].Display(reward);
    }
}
