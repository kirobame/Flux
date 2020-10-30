using UnityEngine;
using UnityEngine.Events;

namespace Flux
{
    public abstract class EnumListener<T,TEvent> : Listener<T,TEvent> where TEvent : UnityEvent<T>
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
    public abstract class EnumListener<T1,T2,TEvent> : Listener<T1,T2,TEvent> where TEvent : UnityEvent<T1,T2>
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
    public abstract class EnumListener<T1,T2,T3,TEvent> : Listener<T1,T2,T3,TEvent> where TEvent : UnityEvent<T1,T2,T3>
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
    public abstract class EnumListener<T1,T2,T3,T4,TEvent> : Listener<T1,T2,T3,T4,TEvent> where TEvent : UnityEvent<T1,T2,T3,T4>
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
}