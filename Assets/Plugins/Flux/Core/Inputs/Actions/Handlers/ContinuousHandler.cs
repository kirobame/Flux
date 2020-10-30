using UnityEngine;

namespace Flux
{
    public abstract class ContinuousHandler<T> : InputHandler<T>, IContinuousHandler
    {
        protected bool isActive;
        private T input;

        protected override void OnStart(T input)
        {
            isActive = true;
            this.input = input;
        }
        protected override void OnPerformed(T input) => this.input = input;
        protected override void OnCanceled(T input)
        {
            this.input = input;
            isActive = false;
        }

        void IContinuousHandler.Update()
        {
            if (!isActive) return;
            HandleInput(input);
        }
        protected abstract void HandleInput(T input);
    }
}