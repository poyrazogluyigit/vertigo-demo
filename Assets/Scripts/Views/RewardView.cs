using System.Globalization;
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
        if (amount == 0) _amountText.text = "";
        else _amountText.text = "x" + Compact(amount);
    }

    // 950, 1.2K, 16K, 1.5M
    static string Compact(int n)
    {
        if (n >= 1_000_000) return Shorten(n, 1_000_000) + "M";
        if (n >= 1_000) return Shorten(n, 1_000) + "K";
        return n.ToString(CultureInfo.InvariantCulture);
    }

    // Truncates rather than rounds, so 1,290 shows 1.2K and never overstates the amount
    static string Shorten(int n, int unit)
    {
        int whole = n / unit;
        int tenth = n % unit / (unit / 10);
        if (whole >= 10 || tenth == 0) return whole.ToString(CultureInfo.InvariantCulture);
        return whole.ToString(CultureInfo.InvariantCulture) + "." + tenth.ToString(CultureInfo.InvariantCulture);
    }
}
