using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Display(Sprite sprite, int amount)
    {
        icon.sprite = sprite;
        amountText.text = "x" + amount;
    }
}
