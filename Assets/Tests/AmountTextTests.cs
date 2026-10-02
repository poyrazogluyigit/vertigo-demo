using NUnit.Framework;

public class AmountTextTests
{
    [TestCase(0, "")]
    [TestCase(1, "x1")]
    [TestCase(950, "x950")]
    [TestCase(1_000, "x1K")]
    [TestCase(1_290, "x1.2K")]      // truncates, never rounds up
    [TestCase(9_999, "x9.9K")]
    [TestCase(16_400, "x16K")]      // no decimal from 10K up
    [TestCase(1_500_000, "x1.5M")]
    public void Label_WritesCompactAmount(int amount, string expected)
    {
        Assert.AreEqual(expected, AmountText.Label(amount));
    }
}
