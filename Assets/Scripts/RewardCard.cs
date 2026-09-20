using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCard : MonoBehaviour
{
    [SerializeField] private Image rewardImage;
    [SerializeField] private TextMeshProUGUI amountDisplay;
    public int amount;
    public void Display(RewardSO reward)
    {
        rewardImage.sprite = reward.sprite;
        amountDisplay.text = amount.ToString();
    }
}
