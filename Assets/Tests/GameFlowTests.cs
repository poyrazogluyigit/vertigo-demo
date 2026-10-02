using NUnit.Framework;

public class GameFlowTests
{
    private FakeRewards _rewards;
    private EventBus _eventBus;
    private FakeView _view;
    private GameFlow _flow;

    // Null until GameFlow publishes one, so "never sent" can't pass as "all disabled"
    private ActionsAllowed? _allowed;

    [SetUp]
    public void SetUp()
    {
        _rewards = new FakeRewards();
        _eventBus = new EventBus();

        _allowed = null;
        _eventBus.Subscribe<ActionsAllowed>(e => _allowed = e);

        _view = new FakeView(_eventBus);
        _flow = new GameFlow(_eventBus, _rewards);
        _flow.Begin();
    }

    [TearDown]
    public void TearDown() => _flow.Dispose();

    // The latest ActionsAllowed; fails the test if GameFlow never published one.
    private ActionsAllowed Allowed
    {
        get
        {
            Assert.IsTrue(_allowed.HasValue, "GameFlow never published ActionsAllowed");
            return _allowed.Value;
        }
    }

    private void SpinToLevel(int level)
    {
        _rewards.ReturnReward();
        for (int i = 1; i < level; i++)
            _eventBus.Publish(new SpinPressed());
    }

    // Review: "the exit button is enabled on bronze zones". The scene's buttons
    // start interactable, so the first zone must say what is allowed.
    [Test]
    public void Begin_AllowsSpinButNotExitOnFirstZone()
    {
        Assert.IsTrue(Allowed.Spin);
        Assert.IsFalse(Allowed.Exit);
    }

    // Review: "after a bomb the buttons are enabled again".
    // Played on a safe zone so that Exit is enabled before the spin.
    [Test]
    public void Bomb_DisablesSpinAndExit()
    {
        SpinToLevel(5);
        Assert.IsTrue(Allowed.Spin && Allowed.Exit, "precondition: both enabled on a safe zone");

        _rewards.ReturnBomb();
        _eventBus.Publish(new SpinPressed());

        Assert.IsFalse(Allowed.Spin);
        Assert.IsFalse(Allowed.Exit);
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
            _eventBus.Publish(new ExitPressed());
        }
        else
        {
            _rewards.ReturnBomb();
            _eventBus.Publish(new SpinPressed());
        }
        _view.Log.Clear();

        _eventBus.Publish(new RestartPressed());

        CollectionAssert.AreEqual(new[] { "GameReset", "RewardsChanged", "LevelStarted(1)" },
            _view.Log.GetRange(0, 3));
        CollectionAssert.IsEmpty(_view.LastEarned, "the in-game list must be emptied on restart");
    }

    // The outcome is decided on press, but must not be shown before the wheel stops.
    [Test]
    public void Spin_WaitsForWheelBeforeShowingResult()
    {
        _view.FinishAnimations = false;
        _view.Log.Clear();

        _eventBus.Publish(new SpinPressed());
        CollectionAssert.DoesNotContain(_view.Log, "RewardsChanged");

        _eventBus.Publish(new SpinFinished());
        CollectionAssert.Contains(_view.Log, "RewardsChanged");
    }

    // Buttons are disabled mid-spin, but the flow must not trust the UI for that.
    [Test]
    public void SpinPressed_WhileSpinning_IsIgnored()
    {
        _view.FinishAnimations = false;
        _eventBus.Publish(new SpinPressed());
        _view.Log.Clear();

        _eventBus.Publish(new SpinPressed());

        CollectionAssert.IsEmpty(_view.Log);
    }

    [Test]
    public void LevelShown_WhenNotSettingUp_IsIgnored()
    {
        _view.Log.Clear();

        _eventBus.Publish(new LevelShown());

        CollectionAssert.IsEmpty(_view.Log);
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

        Assert.AreEqual(exitEnabled, Allowed.Exit);
    }
}
