using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelManager _lm;
    [SerializeField] private Button _button;
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private Transform _levelCounter;
    [SerializeField] private RewardsManager _rewardsManager;
    [SerializeField] private RewardSO _superReward;

    async void Start()
    {
        await SetLevel(1);
    }
    async void RunLevel()
    {
        RewardSO reward = await _lm.Play();
        // (if reward is bomb) GameOver();
        _rewardsManager.AddReward(reward, 1);
        await SetLevel(++_currentLevel);

    }

    async Task SetLevel(int level)
    {
        await MoveLevelIndicator(level);
        _lm.SetLevel(_currentLevel);
        _button.onClick.AddListener(RunLevel);
    }

    async Task MoveLevelIndicator(int level)
    {
        // calculate position based on level
        // initial position fixed for now, maybe automated later
        Vector3 xOffset = new Vector3(-(level - 1) * 46, 0, 0);
        Vector3 endPos = _levelCounter.position + xOffset;
        await _levelCounter.DOMove(endPos, 1f).AsyncWaitForCompletion();
    }


}
