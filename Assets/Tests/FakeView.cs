using System.Collections.Generic;

// Stands in for the scene's views on the bus: records every game event in order,
// and finishes level/spin animations instantly unless the test holds them.
public class FakeView
{
    public readonly List<string> Log = new List<string>();

    // false holds the animation, so the test can publish LevelShown/SpinFinished itself
    public bool FinishAnimations = true;

    // Payload of the latest RewardsChanged, null until one is published
    public Reward[] LastEarned;

    public FakeView(IEventBus bus)
    {
        bus.Subscribe<GameReset>(_ => Log.Add("GameReset"));
        bus.Subscribe<WheelReady>(_ => Log.Add("WheelReady"));
        bus.Subscribe<RewardsChanged>(e =>
        {
            Log.Add("RewardsChanged");
            LastEarned = e.Earned;
        });
        bus.Subscribe<CashedOut>(_ => Log.Add("CashedOut"));
        bus.Subscribe<BombHit>(_ => Log.Add("BombHit"));

        bus.Subscribe<LevelStarted>(e =>
        {
            Log.Add($"LevelStarted({e.Level})");
            if (FinishAnimations) bus.Publish(new LevelShown());
        });
        bus.Subscribe<SpinStarted>(e =>
        {
            Log.Add($"SpinStarted({e.Slot})");
            if (FinishAnimations) bus.Publish(new SpinFinished());
        });
    }
}
