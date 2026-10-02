using NUnit.Framework;

namespace WheelSpin.Tests
{
    public class ZonesTests
    {
        [TestCase(1, ZoneType.Normal)]
        [TestCase(4, ZoneType.Normal)]
        [TestCase(5, ZoneType.Safe)]
        [TestCase(10, ZoneType.Safe)]
        [TestCase(29, ZoneType.Normal)]
        [TestCase(30, ZoneType.Super)]
        [TestCase(35, ZoneType.Safe)]
        [TestCase(60, ZoneType.Super)]
        public void TypeOf_FollowsTheZoneRhythm(int level, ZoneType expected)
        {
            Assert.AreEqual(expected, Zones.TypeOf(level));
        }

        [TestCase(1, false)]
        [TestCase(5, true)]
        [TestCase(30, true)]
        [TestCase(31, false)]
        public void AllowsExit_OnlyOnSafeAndSuperZones(int level, bool expected)
        {
            Assert.AreEqual(expected, Zones.AllowsExit(level));
        }
    }
}
