using System.Collections.Generic;

// Input events
public struct SpinPressed : IEvent { };
public struct ExitPressed : IEvent { };
public struct RestartPressed : IEvent { };
public struct ActionsAllowed : IEvent
{
    public bool Spin;
    public bool Exit;

    public ActionsAllowed(bool spin, bool exit)
    {
        Spin = spin; Exit = exit;
    }
}

// Game events, published by GameFlow
public struct GameReset : IEvent { };

public struct LevelStarted : IEvent
{
    public int Level;
    public LevelStarted(int level) => Level = level;
}

public struct WheelReady : IEvent
{
    public WheelSO Wheel;
    public Reward[] Rewards;

    public WheelReady(WheelSO wheel, Reward[] rewards)
    {
        Wheel = wheel; Rewards = rewards;
    }
}

public struct SpinStarted : IEvent
{
    public int Slot;
    public SpinStarted(int slot) => Slot = slot;
}

public struct RewardsEarned : IEvent
{
    public IReadOnlyDictionary<RewardDefinition, int> Earned;
    public RewardsEarned(IReadOnlyDictionary<RewardDefinition, int> earned) => Earned = earned;
}

public struct CashedOut : IEvent
{
    public IReadOnlyDictionary<RewardDefinition, int> Earned;
    public CashedOut(IReadOnlyDictionary<RewardDefinition, int> earned) => Earned = earned;
}

public struct BombHit : IEvent { };
public struct LevelShown : IEvent { };
public struct SpinFinished : IEvent { };
