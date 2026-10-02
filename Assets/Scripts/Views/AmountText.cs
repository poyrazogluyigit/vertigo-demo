using System.Globalization;

namespace WheelSpin
{
    // How reward amounts are written everywhere they are shown: wheel slots, the in-game list, end-game cards.
    public static class AmountText
    {
        // "x950", "x1.2K", "x16K", "x1.5M"; empty for 0, so the bomb shows no "x0"
        public static string Label(int amount) => amount == 0 ? "" : "x" + Compact(amount);

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
}
