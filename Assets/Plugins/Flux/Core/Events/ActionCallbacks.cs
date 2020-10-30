using System;

namespace Flux
{
    public class ActionCallbacks<T>
    {
        public ActionCallbacks() { }
        public ActionCallbacks(Action<T> onStart, Action<T> onUpdate, Action<T> onEnd)
        {
            this.onStart = onStart;
            this.onUpdate = onUpdate;
            this.onEnd = onEnd;
        }
        
        public Action<T> onStart;
        public Action<T> onUpdate;
        public Action<T> onEnd;
    }
}