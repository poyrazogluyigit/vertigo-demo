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
    [SerializeField] private RewardType _superReward;
    private Vector3 _initialPosition;

    async void Start()
    {
        var rt = _levelCounter.GetComponent<RectTransform>();
        _initialPosition = rt.anchoredPosition;
        _button.onClick.AddListener(RunLevel);
        await SetLevel(1);
    }
    async void RunLevel()
    {
        _button.interactable = false;
        RewardType reward = await _lm.Play();
        // (if reward is bomb) GameOver();
        _rewardsManager.AddReward(reward, 1);
        await SetLevel(_currentLevel + 1);
        _button.interactable = true;

    }

    async Task SetLevel(int level)
    {
        await MoveLevelIndicator(level);
        _currentLevel = level;
        _lm.SetLevel(_currentLevel);
    }

    async Task MoveLevelIndicator(int level)
    {
        var rt = _levelCounter.GetComponent<RectTransform>();
        float xDelta = 135;
        Vector3 target = _initialPosition + new Vector3(-(level - 1) * xDelta, 0, 0);
        Debug.Log(target);
        await rt.DOAnchorPos(target, 1f).AsyncWaitForCompletion();
    }


}
