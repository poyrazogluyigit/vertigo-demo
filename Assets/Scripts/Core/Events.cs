namespace WheelSpin
{
    // Input events, published by InputManager
    public struct SpinPressed : IEvent { };
    public struct ExitPressed : IEvent { };
    public struct RestartPressed : IEvent { };

    // Game events, published by GameFlow
    // Which buttons may be pressed right now
    public struct ActionsAllowed : IEvent
    {
        public bool Spin;
        public bool Exit;

        public ActionsAllowed(bool spin, bool exit)
        {
            Spin = spin; Exit = exit;
        }
    }

    public struct GameReset : IEvent { };

    public struct LevelChangedTo : IEvent
    {
        public int Level;
        public LevelChangedTo(int level) => Level = level;
    }

    public struct RewardsReady : IEvent
    {
        public ZoneType Zone;
        public Reward[] Rewards;

        public RewardsReady(ZoneType zone, Reward[] rewards)
        {
            Zone = zone; Rewards = rewards;
        }
    }

    public struct SpinStarted : IEvent
    {
        public int Slot;
        public SpinStarted(int slot) => Slot = slot;
    }

    // Everything earned so far; empty after a restart
    public struct RewardsChanged : IEvent
    {
        public Reward[] Earned;
        public RewardsChanged(Reward[] earned) => Earned = earned;
    }

    public struct CashedOut : IEvent
    {
        public Reward[] Earned;
        public CashedOut(Reward[] earned) => Earned = earned;
    }

    public struct BombHit : IEvent { };

    // View replies. Exactly one view must publish SpinFinished after each SpinStarted, or the game stalls.
    public struct SpinFinished : IEvent { };
}
