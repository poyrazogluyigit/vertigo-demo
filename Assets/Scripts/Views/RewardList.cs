using UnityEngine;

public class RewardList : View
{
    [SerializeField] private RewardView _itemPrefab;
    [SerializeField] private bool _drawOnRewardsChanged = true;   // off for the cash-out list, which EndgameView draws

    protected override void Subscribe()
    {
        if (_drawOnRewardsChanged) Bus.Subscribe<RewardsChanged>(OnRewardsChanged);
    }

    protected override void Unsubscribe()
    {
        if (_drawOnRewardsChanged) Bus.Unsubscribe<RewardsChanged>(OnRewardsChanged);
    }

    void OnRewardsChanged(RewardsChanged e) => Draw(e.Earned);

    public void Draw(Reward[] rewards)
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            RewardView item = i < transform.childCount
                ? transform.GetChild(i).GetComponent<RewardView>()
                : Instantiate(_itemPrefab, transform);
            item.gameObject.SetActive(true);
            item.Display(rewards[i].RewardDefn.Image, rewards[i].Amount);
        }

        // Hidden rather than destroyed: layout groups skip inactive children,
        // and they are reused the next time the list grows.
        for (int i = rewards.Length; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (_itemPrefab == null) Debug.LogError($"{name}: item prefab is not assigned!", this);
    }
#endif
}
