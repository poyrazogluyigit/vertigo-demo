using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class RewardServiceTests
{
    private const int Amount = 10;

    private readonly List<Object> _created = new List<Object>();
    private RewardDefinition _cash, _bomb;
    private StubRandom _random;
    private RewardService _service;

    [SetUp]
    public void SetUp()
    {
        _cash = Definition(id: 1, isBomb: false);
        _bomb = Definition(id: 2, isBomb: true);

        var wheel = Create<WheelSO>();
        wheel.Slices = new[] { _cash, _bomb };

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
        Assert.AreEqual(2 * Amount, _service.EarnedRewards[_cash]);
    }

    [Test]
    public void Pick_Bomb_AddsNothingAndKeepsEarlierEarnings()
    {
        _random.Next(0, 1);

        _service.Pick();
        SpinResult bomb = _service.Pick();

        Assert.IsTrue(bomb.IsBomb);
        Assert.IsFalse(_service.EarnedRewards.ContainsKey(_bomb));
        Assert.AreEqual(Amount, _service.EarnedRewards[_cash]);
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
