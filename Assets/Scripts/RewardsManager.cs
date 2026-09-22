using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RewardsManager : MonoBehaviour
{
    private Dictionary<RewardData, RewardCard> rewards = new Dictionary<RewardData, RewardCard>();
    [SerializeField] private Button _exitButton;
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private RewardCard _rcPrefab;
    public void AddReward(RewardData reward, int amt)
    {
        if (reward == null) return;
        if (!rewards.ContainsKey(reward))
            rewards[reward] = Instantiate(_rcPrefab, _contentContainer);
        rewards[reward].AddAmount(amt);
        rewards[reward].Display(reward);
    }
}
