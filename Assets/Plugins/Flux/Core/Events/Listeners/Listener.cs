using System;
using UnityEngine;
using UnityEngine.Events;

namespace Flux
{
    public abstract class Listener : MonoBehaviour
    {
        protected abstract string Address { get; }
    }
    public abstract class VoidListener : Listener
    {
        [SerializeField] private UnityEvent callback;
        
        void OnEnable() => Event.Register(Address, callback.Invoke);
        void OnDisable() => Event.Unregister(Address, callback.Invoke);
    }
    public abstract class Listener<T,TEvent> : Listener where TEvent : UnityEvent<T>
    {
        [SerializeField] private TEvent callback;
        
        void OnEnable()
        {
            Debug.Log(Address);
            Event.Register<T>(Address, callback.Invoke);
        }

        void OnDisable() => Event.Unregister<T>(Address, callback.Invoke);
    }
    public abstract class Listener<T1,T2,TEvent> : Listener where TEvent : UnityEvent<T1,T2>
    {
        [SerializeField] private TEvent callback;
        
        void OnEnable() => Event.Register<T1,T2>(Address, callback.Invoke);
        void OnDisable() => Event.Unregister<T1,T2>(Address, callback.Invoke);
    }
    public abstract class Listener<T1,T2,T3,TEvent> : Listener where TEvent : UnityEvent<T1,T2,T3>
    {
        [SerializeField] private TEvent callback;
        
        void OnEnable() => Event.Register<T1,T2,T3>(Address, callback.Invoke);
        void OnDisable() => Event.Unregister<T1,T2,T3>(Address, callback.Invoke);
    }
    public abstract class Listener<T1,T2,T3,T4,TEvent> : Listener where TEvent : UnityEvent<T1,T2,T3,T4>
    {
        [SerializeField] private TEvent callback;
        
        void OnEnable() => Event.Register<T1,T2,T3,T4>(Address, callback.Invoke);
        void OnDisable() => Event.Unregister<T1,T2,T3,T4>(Address, callback.Invoke);
    }
}