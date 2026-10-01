using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGameView
{
    void Clear();
    void DrawWheel(WheelSO wheel, Reward[] rewards);
    Task SpinWheel(int slot);
    void DisplayEarnedRewards(IReadOnlyDictionary<RewardDefinition, int> rewards);
    void DisplayExitScreen(IReadOnlyDictionary<RewardDefinition, int> rewards);
    void DisplayGameOverScreen();
    Task UpdateLevelIndicator(int level);
}
