using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : MonoBehaviour
{
    [SerializeField] private Image rewardImage;
    [SerializeField] private TextMeshProUGUI amountDisplay;
    private int _amount = 0;
    public void Display(RewardData reward)
    {
        rewardImage.sprite = reward.sprite;
        amountDisplay.text = "x" + _amount;
    }
}
