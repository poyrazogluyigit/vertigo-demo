using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

// The brief's bomb rule: exactly one bomb on normal zones, none on safe and super zones.
public class WheelScheduleTests
{
    private readonly List<Object> _created = new List<Object>();

    [TearDown]
    public void TearDown()
    {
        foreach (Object o in _created) Object.DestroyImmediate(o);
        _created.Clear();
    }

    [TestCase(ZoneType.Normal, 1, true)]
    [TestCase(ZoneType.Normal, 0, false)]
    [TestCase(ZoneType.Normal, 2, false)]
    [TestCase(ZoneType.Safe, 0, true)]
    [TestCase(ZoneType.Safe, 1, false)]
    [TestCase(ZoneType.Super, 0, true)]
    [TestCase(ZoneType.Super, 1, false)]
    public void CheckWheel_EnforcesBombCountPerZone(ZoneType zone, int bombs, bool valid)
    {
        WheelSO wheel = WheelWithBombs(bombs);

        string problem = WheelSchedule.CheckWheel(wheel, zone);

        Assert.AreEqual(valid, problem == null, problem);
    }

    [Test]
    public void CheckWheel_MissingWheel_IsAProblem()
    {
        Assert.IsNotNull(WheelSchedule.CheckWheel(null, ZoneType.Safe));
    }

    private WheelSO WheelWithBombs(int bombs)
    {
        var wheel = Create<WheelSO>();
        for (int i = 0; i < WheelSO.SliceCount; i++)
        {
            var slice = Create<RewardDefinition>();
            slice.Id = i;
            slice.IsBomb = i < bombs;
            wheel.Slices[i] = slice;
        }
        return wheel;
    }

    private T Create<T>() where T : ScriptableObject
    {
        var o = ScriptableObject.CreateInstance<T>();
        _created.Add(o);
        return o;
    }
}
