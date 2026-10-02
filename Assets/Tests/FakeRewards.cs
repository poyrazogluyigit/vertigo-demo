using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace WheelSpin.Tests
{
    // Pick() returns whatever outcome the test chose last; defaults to a reward.
    public class FakeRewards : IRewardService, IDisposable
    {
        private readonly RewardDefinition _prize = ScriptableObject.CreateInstance<RewardDefinition>();
        private readonly RewardDefinition _bomb = ScriptableObject.CreateInstance<RewardDefinition>();
        private bool _nextIsBomb;

        public FakeRewards() => _bomb.IsBomb = true;

        public Reward[] PossibleRewards() => new Reward[0];
        public Reward[] Earned() => new Reward[0];

        public void ReturnReward() => _nextIsBomb = false;
        public void ReturnBomb() => _nextIsBomb = true;

        public void ClearRewards() { }
        public void GenerateRewards(int level) { }
        public SpinResult Pick() => new SpinResult(0, new Reward(_nextIsBomb ? _bomb : _prize, 0));

        public void Dispose()
        {
            Object.DestroyImmediate(_prize);
            Object.DestroyImmediate(_bomb);
        }
    }
}
