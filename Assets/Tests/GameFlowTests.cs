using NUnit.Framework;

public class GameFlowTests
{
    private FakeRewards _rewards;
    private FakeInput _input;
    private FakeView _view;
    private GameFlow _flow;

    [SetUp]
    public void SetUp()
    {
        _rewards = new FakeRewards();
        _input = new FakeInput();
        _view = new FakeView();
        _flow = new GameFlow(_rewards, _input, _view);
        _flow.Begin();
    }

    [TearDown]
    public void TearDown() => _flow.Dispose();

    // The game starts at level 1 and every winning spin moves up one level.
    private void SpinToLevel(int level)
    {
        _rewards.ReturnReward();
        for (int i = 1; i < level; i++)
            _input.PressSpin();
    }

    // Review: "after a bomb the buttons are enabled again".
    // Played on a safe zone so that Exit is enabled before the spin.
    [Test]
    public void Bomb_DisablesSpinAndExit()
    {
        SpinToLevel(5);
        Assert.IsTrue(_input.SpinEnabled && _input.ExitEnabled, "precondition: both enabled on a safe zone");

        _rewards.ReturnBomb();
        _input.PressSpin();

        Assert.IsFalse(_input.SpinEnabled);
        Assert.IsFalse(_input.ExitEnabled);
    }

    // Review: "the previous game's rewards show on the end screen".
    // Covers both ways a game can end; the view must be cleared first thing on restart.
    [TestCase(true, TestName = "Restart_AfterCashOut_ClearsViewAndResetsLevel")]
    [TestCase(false, TestName = "Restart_AfterBomb_ClearsViewAndResetsLevel")]
    public void Restart_ClearsViewAndResetsLevel(bool cashOut)
    {
        SpinToLevel(5);
        if (cashOut)
        {
            _input.PressExit();
        }
        else
        {
            _rewards.ReturnBomb();
            _input.PressSpin();
        }
        _view.Log.Clear();

        _input.PressRestart();

        Assert.AreEqual("Clear", _view.Log[0]);
        Assert.AreEqual("UpdateLevelIndicator(1)", _view.Log[1]);
    }

    // Review: "players can cash out on bronze zones".
    // Exit is only allowed on safe (every 5th) and super (every 30th) zones.
    [TestCase(1, false)]
    [TestCase(2, false)]
    [TestCase(4, false)]
    [TestCase(5, true)]
    [TestCase(6, false)]
    [TestCase(10, true)]
    [TestCase(29, false)]
    [TestCase(30, true)]
    public void Exit_IsEnabledOnlyOnSafeAndSuperZones(int level, bool exitEnabled)
    {
        SpinToLevel(level);

        Assert.AreEqual(exitEnabled, _input.ExitEnabled);
    }

    [Test]
    public void Exit_OnBronzeZone_IsIgnored()
    {
        SpinToLevel(3);
        _input.PressExit();
        _view.
    }
}
