using NUnit.Framework;

namespace WheelSpin.Tests
{
    public struct Ping : IEvent { }

    public class EventBusTests
    {
        EventBus _eventBus;

        [SetUp]
        public void SetUp()
        {
            _eventBus = new EventBus();
        }

        [Test] 
        public void Publish_CallsSubscribedHandler()
        {
            int calls = 0;
            _eventBus.Subscribe<Ping>(_ => calls++);

            _eventBus.Publish(new Ping());

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void Subscribe_TwoSubscribersCalled()
        {
            bool call1 = false;
            bool call2 = false;
            _eventBus.Subscribe<Ping>(_ => call1 = true);
            _eventBus.Subscribe<Ping>(_ => call2 = true);

            _eventBus.Publish(new Ping());

            Assert.IsTrue(call1);
            Assert.IsTrue(call2);
        }

        [Test]
        public void Subscribe_Unsubscribe_NotCalled()
        {
            bool called = false;
            System.Action<Ping> a = _ => called = true;

            _eventBus.Subscribe(a);
            _eventBus.Unsubscribe(a);
            _eventBus.Publish(new Ping());

            Assert.IsFalse(called);
        }

        [Test]
        public void Publish_NoSubscribers()
        {
            Assert.DoesNotThrow(() => { _eventBus.Publish(new Ping()); });
        }
    }
}
