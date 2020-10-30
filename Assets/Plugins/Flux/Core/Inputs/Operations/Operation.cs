using System;
using UnityEngine;

namespace Flux
{
    public abstract class Operation : ScriptableObject
    {
        public event Action<EventArgs> onStart;
        public event Action<EventArgs> onUpdate;
        public event Action<EventArgs> onEnd;
        
        protected OperationHandler operationHandler { get; private set; }
        protected bool isBinded;

        public virtual void Initialize(OperationHandler operationHandler) => this.operationHandler = operationHandler;
        protected virtual void OnDestroy() => Unbind();
        
        public abstract void Bind(InputHandler inputHandler);
        public abstract void Bind(Operation operation);
        public abstract void Unbind();
        
        protected void Begin(EventArgs outArgs) => onStart?.Invoke(outArgs);
        protected void Prolong(EventArgs outArgs) => onUpdate?.Invoke(outArgs);
        protected void End(EventArgs outArgs) => onEnd?.Invoke(outArgs);
    }
}