using System;

public interface IGameInput
{
    event Action SpinPressed;
    event Action ExitPressed;
    event Action RestartPressed;

    void SetSpinEnabled(bool enabled);
    void SetExitEnabled(bool enabled);
    void SetRestartEnabled(bool enabled);

}