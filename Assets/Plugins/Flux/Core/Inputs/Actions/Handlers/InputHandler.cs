using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flux
{
    public abstract class InputHandler : ScriptableObject
    {
        public event Action<EventArgs> onStart;
        public event Action<EventArgs> onUpdate;
        public event Action<EventArgs> onEnd;
        
        protected InputAction bindedAction;

        public abstract void Initialize();
        protected virtual void OnDestroy() => Unbind();
        
        public virtual void Bind(InputAction inputAction)
        {
            if (bindedAction != null) Unbind();
            bindedAction = inputAction;
        }
        public abstract void Unbind();
        
        protected void Begin(EventArgs args) => onStart?.Invoke(args);
        protected void Prolong(EventArgs args) => onUpdate?.Invoke(args);
        protected void End(EventArgs args) => onEnd?.Invoke(args);
    }
    public abstract class InputHandler<TInput> : InputHandler
    {
        private ActionCallbacks<InputAction.CallbackContext> inputCallbacks;

        public override void Initialize()
        {
            inputCallbacks = new ActionCallbacks<InputAction.CallbackContext>()
            {
                onStart = ctxt => OnStart((TInput) Convert.ChangeType(ctxt.ReadValueAsObject(), typeof(TInput))),
                onUpdate = ctxt => OnPerformed((TInput) Convert.ChangeType(ctxt.ReadValueAsObject(), typeof(TInput))),
                onEnd = ctxt => OnCanceled((TInput) Convert.ChangeType(ctxt.ReadValueAsObject(), typeof(TInput)))
            };
        }

        public override void Bind(InputAction inputAction)
        {
            base.Bind(inputAction);
            
            inputAction.started += inputCallbacks.onStart;
            inputAction.performed += inputCallbacks.onUpdate;
            inputAction.canceled += inputCallbacks.onEnd;
        }
        public override void Unbind()
        {
            bindedAction.started -= inputCallbacks.onStart;
            bindedAction.performed -= inputCallbacks.onUpdate;
            bindedAction.canceled -= inputCallbacks.onEnd;
            
            bindedAction = null;
        }

        protected virtual void OnStart(TInput input) { }
        protected virtual void OnPerformed(TInput input) { }
        protected virtual void OnCanceled(TInput input) { }
    }
}