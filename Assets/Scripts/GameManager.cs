using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager _lm;
    [SerializeField] private Button _button;
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private RewardsManager _rewardsManager;
    [SerializeField] private RewardSO _superReward;

    void Start()
    {
        _lm.SetLevel(_currentLevel);
        _button.onClick.AddListener(RunLevel);
    }
    async void RunLevel()
    {
        RewardSO reward = await _lm.Play();
        // (if reward is bomb) GameOver();
        _rewardsManager.AddReward(reward, 1);

    }


}
