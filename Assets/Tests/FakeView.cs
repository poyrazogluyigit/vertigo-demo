using System.Collections.Generic;
using System.Threading.Tasks;

// Records every call in order, so tests can assert what was shown and when.
public class FakeView : IGameView
{
    public readonly List<string> Log = new List<string>();

    // Replace with a TaskCompletionSource's Task to hold the wheel mid-spin.
    public Task SpinTask = Task.CompletedTask;

    public void Clear() => Log.Add("Clear");
    public void DrawWheel(WheelSO wheel, Reward[] rewards) => Log.Add("DrawWheel");
    public void DisplayEarnedRewards(IReadOnlyDictionary<RewardDefinition, int> rewards) => Log.Add("DisplayEarnedRewards");
    public void DisplayExitScreen(IReadOnlyDictionary<RewardDefinition, int> rewards) => Log.Add("DisplayExitScreen");
    public void DisplayGameOverScreen() => Log.Add("DisplayGameOverScreen");

    public Task SpinWheel(int slot)
    {
        Log.Add($"SpinWheel({slot})");
        return SpinTask;
    }

    public Task UpdateLevelIndicator(int level)
    {
        Log.Add($"UpdateLevelIndicator({level})");
        return Task.CompletedTask;
    }
}
