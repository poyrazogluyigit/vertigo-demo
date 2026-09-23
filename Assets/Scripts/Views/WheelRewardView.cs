using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelRewardView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetReward(RewardData reward, int amount)
    {
        icon.sprite = reward != null ? reward.sprite : null;
        amountText.text = reward != null ? "x" + amount : "";
    }
}
