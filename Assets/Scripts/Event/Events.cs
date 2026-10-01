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