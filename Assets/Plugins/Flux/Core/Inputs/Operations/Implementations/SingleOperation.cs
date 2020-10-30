using System;
using UnityEngine;

namespace Flux
{
    public abstract class SingleOperation : Operation
    {
        protected InputHandler bindedHandler;
        protected Operation parentOperation;
        
        public override void Bind(InputHandler inputHandler)
        {
            if (isBinded) Unbind();
            bindedHandler = inputHandler;

            inputHandler.onStart += OnStart;
            inputHandler.onUpdate += OnUpdate;
            inputHandler.onEnd += OnEnd;

            isBinded = true;
        }
        public override void Bind(Operation operation)
        {
            if (isBinded) Unbind();
            parentOperation = operation;

            operation.onStart += OnStart;
            operation.onUpdate += OnUpdate;
            operation.onEnd += OnEnd;
            
            isBinded = true;
        }
        public override void Unbind()
        {
            if (bindedHandler != null)
            {
                bindedHandler.onStart -= OnStart;
                bindedHandler.onUpdate -= OnUpdate;
                bindedHandler.onEnd -= OnEnd;

                bindedHandler = null;
            }
            else if (parentOperation != null)
            {
                parentOperation.onStart -= OnStart;
                parentOperation.onUpdate -= OnUpdate;
                parentOperation.onEnd -= OnEnd;

                parentOperation = null;
            }

            isBinded = false;
        }
        
        public virtual void OnStart(EventArgs inArgs) { }
        public virtual void OnUpdate(EventArgs inArgs) { }
        public virtual void OnEnd(EventArgs inArgs) { }
    }
}