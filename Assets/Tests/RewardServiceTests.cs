using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class RewardServiceTests
{
    private const int Amount = 10;

    private readonly List<Object> _created = new List<Object>();
    private RewardDefinition _cash, _gold, _bomb;
    private StubRandom _random;
    private RewardService _service;

    [SetUp]
    public void SetUp()
    {
        _cash = Definition(id: 1, isBomb: false);
        _bomb = Definition(id: 2, isBomb: true);
        _gold = Definition(id: 3, isBomb: false);

        var wheel = Create<WheelSO>();
        wheel.Slices = new[] { _cash, _bomb, _gold };

        _random = new StubRandom();
        _service = new RewardService(new StubSchedule(wheel), new StubPricing(), _random);
        _service.GenerateRewards(1);
    }

    [TearDown]
    public void TearDown()
    {
        foreach (Object o in _created) Object.DestroyImmediate(o);
        _created.Clear();
    }

    [Test]
    public void Pick_SameRewardTwice_AddsUpEarnings()
    {
        _random.Next(0, 0);

        _service.Pick();
        SpinResult second = _service.Pick();

        Assert.AreEqual(2 * Amount, second.EarnedTotal);
        Assert.AreEqual(2 * Amount, EarnedOf(_cash));
    }

    [Test]
    public void Pick_Bomb_AddsNothingAndKeepsEarlierEarnings()
    {
        _random.Next(0, 1);

        _service.Pick();
        SpinResult bomb = _service.Pick();

        Assert.IsTrue(bomb.IsBomb);
        Assert.AreEqual(1, _service.Earned().Length, "the bomb must not get an entry");
        Assert.AreEqual(Amount, EarnedOf(_cash));
    }

    // Views redraw the list from scratch each time, so its order has to come from here.
    [Test]
    public void SnapshotEarned_KeepsFirstWonOrder()
    {
        _random.Next(2, 0, 2);

        _service.Pick();
        _service.Pick();
        _service.Pick();

        Reward[] earned = _service.Earned();
        Assert.AreEqual(_gold, earned[0].RewardDefn);
        Assert.AreEqual(_cash, earned[1].RewardDefn);
        Assert.AreEqual(2 * Amount, earned[0].Amount);
    }

    [Test]
    public void SnapshotEarned_IsACopy()
    {
        _random.Next(0, 0);
        _service.Pick();
        Reward[] before = _service.Earned();

        _service.Pick();

        Assert.AreEqual(Amount, before[0].Amount);
    }

    private int EarnedOf(RewardDefinition defn)
    {
        foreach (Reward r in _service.Earned())
            if (r.RewardDefn == defn) return r.Amount;
        return 0;
    }

    private RewardDefinition Definition(int id, bool isBomb)
    {
        var d = Create<RewardDefinition>();
        d.Id = id;          // equality and hashing go by Id
        d.IsBomb = isBomb;
        return d;
    }

    private T Create<T>() where T : ScriptableObject
    {
        var o = ScriptableObject.CreateInstance<T>();
        _created.Add(o);
        return o;
    }

    private class StubSchedule : IWheelSchedule
    {
        private readonly WheelSO _wheel;
        public StubSchedule(WheelSO wheel) => _wheel = wheel;
        public WheelSO WheelFor(int level) => _wheel;
    }

    private class StubPricing : IRewardPricing
    {
        public int AmountFor(RewardDefinition reward, int level) => Amount;
    }

    // Returns the queued slots in order
    private class StubRandom : IRandom
    {
        private readonly Queue<int> _slots = new Queue<int>();
        public void Next(params int[] slots) { foreach (int s in slots) _slots.Enqueue(s); }
        public int Range(int minInclusive, int maxExclusive) => _slots.Dequeue();
    }
}
