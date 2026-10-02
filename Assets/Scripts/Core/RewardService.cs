using System.Collections.Generic;

namespace WheelSpin
{
    public class RewardService : IRewardService
    {
        private readonly IWheelSchedule _schedule;
        private readonly IRewardPricing _pricing;
        private readonly IRandom _random;

        private readonly List<Reward> _earned = new List<Reward>();

        public RewardService(IWheelSchedule schedule, IRewardPricing pricing, IRandom random)
        {
            _schedule = schedule;
            _pricing = pricing;
            _random = random;
        }

        private Reward[] _possible = new Reward[0];

        public Reward[] PossibleRewards() => (Reward[])_possible.Clone();

        public Reward[] Earned() => _earned.ToArray();

        public void ClearRewards()
        {
            _earned.Clear();
        }

        public void GenerateRewards(int level)
        {
            RewardDefinition[] slices = _schedule.WheelFor(level).Slices;
            var rewards = new Reward[slices.Length];
            for (int i = 0; i < slices.Length; i++)
            {
                RewardDefinition slice = slices[i];
                // Amount 0 hides the label, so the bomb shows no "x1"
                int amount = slice.IsBomb ? 0 : _pricing.AmountFor(slice, level);
                rewards[i] = new Reward(slice, amount);
            }
            _possible = rewards;
        }

        // Decides and commits the outcome immediately; presentation happens later.
        public SpinResult Pick()
        {
            int slot = _random.Range(0, _possible.Length);
            var result = new SpinResult(slot, _possible[slot]);
            if (!result.IsBomb) Earn(result.Reward);
            return result;
        }

        // Adds to the reward's running total
        void Earn(Reward reward)
        {
            for (int i = 0; i < _earned.Count; i++)
            {
                if (_earned[i].Definition == reward.Definition)
                {
                    _earned[i] = new Reward(reward.Definition, _earned[i].Amount + reward.Amount);
                    return;
                }
            }
            _earned.Add(reward);
        }
    }
}
