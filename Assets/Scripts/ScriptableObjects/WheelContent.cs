using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WheelContent : ScriptableObject
{
    int _slots = 8;
    Reward[] rewards;
    bool randomize = true;

    void Randomize(RewardPool rewardPool, int level)
    {
        List<RewardDefinition> rewardDefns = rewardPool.Rewards.Where(r => r != null).ToList();

        // Fisher-Yates shuffle over all reward pool
        for (int i = rewardDefns.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            // Swap
            RewardDefinition temp = rewardDefns[i];
            rewardDefns[i] = rewardDefns[randomIndex];
            rewardDefns[randomIndex] = temp;
        }
        rewards = rewardDefns.Take(_slots)
                .Select(x => 
                new Reward(x, rewardPool.CalculateAmount(x, level)))
                .ToArray();;
    }
}
