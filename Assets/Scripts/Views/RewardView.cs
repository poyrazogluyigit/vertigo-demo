using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RewardView : MonoBehaviour
{
    [FormerlySerializedAs("icon")]
    [SerializeField] private Image _icon;
    [FormerlySerializedAs("amountText")]
    [SerializeField] private TextMeshProUGUI _amountText;

    public void Display(Sprite sprite, int amount)
    {
        _icon.sprite = sprite;
        _amountText.text = AmountText.Label(amount);
    }
}
