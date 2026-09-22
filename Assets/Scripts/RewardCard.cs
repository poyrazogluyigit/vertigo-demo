using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] private Image rewardImage;
    [SerializeField] private TextMeshProUGUI amountDisplay;
    private int _amount = 0;
    public void Display(RewardType reward)
    {
        rewardImage.sprite = reward.sprite;
        amountDisplay.text = "x" + _amount;
    }

    public void AddAmount(int amt) => _amount += amt;
}
