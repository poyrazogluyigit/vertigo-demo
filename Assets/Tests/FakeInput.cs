using System;

public class FakeInput : IGameInput
{
    public event Action SpinPressed, ExitPressed, RestartPressed;
    public bool SpinEnabled, ExitEnabled, RestartEnabled;
    public void SetSpinEnabled(bool e) => SpinEnabled = e;
    public void SetExitEnabled(bool e) => ExitEnabled = e;
    public void SetRestartEnabled(bool e) => RestartEnabled = e;
    public void PressSpin() => SpinPressed?.Invoke();
    public void PressExit() => ExitPressed?.Invoke();
    public void PressRestart() => RestartPressed?.Invoke();
}
