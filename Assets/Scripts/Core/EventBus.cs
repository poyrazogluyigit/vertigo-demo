using System;
using System.Collections.Generic;

namespace WheelSpin
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler) where T : struct, IEvent;
        void Unsubscribe<T>(Action<T> handler) where T : struct, IEvent;
        void Publish<T>(T @event) where T : struct, IEvent;
    }

    public class EventBus : IEventBus
    {
        private Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

        public void Subscribe<T>(Action<T> handler) where T: struct, IEvent
        {
            Type key = typeof(T);
            if (_events.TryGetValue(key, out Delegate dg))
                _events[key] = Delegate.Combine(dg, handler);
            else
                _events[key] = handler;

        }
        public void Unsubscribe<T>(Action<T> handler) where T : struct, IEvent
        {
            Type key = typeof(T);
            if (_events.TryGetValue(key, out Delegate dg))
                _events[key] = Delegate.Remove(dg, handler);
        }
        public void Publish<T>(T @event) where T : struct, IEvent
        {
            Type key = typeof(T);
            if (_events.TryGetValue(key, out Delegate dg))
                (dg as Action<T>)?.Invoke(@event);
        }
    }
}
