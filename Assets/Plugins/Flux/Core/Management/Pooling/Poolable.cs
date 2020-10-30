using UnityEngine;

namespace Flux
{
    public abstract class Poolable : MonoBehaviour { }
    public abstract class Poolable<T> : Poolable
    {
        public Poolable<T> Key => key;
        
        public T Value => value;
        [SerializeField] private T value;
        
        private Poolable<T> key;
        protected Pool<T> origin;
        
        protected virtual void OnDisable()
        {
            origin.Stock(this);

            key = null;
            origin = null;
        }
            
        public void SetOrigin(Pool<T> origin, Poolable<T> key)
        {
            this.key = key;
            this.origin = origin;
        }
        
        public virtual void Reboot() { }
    }
}