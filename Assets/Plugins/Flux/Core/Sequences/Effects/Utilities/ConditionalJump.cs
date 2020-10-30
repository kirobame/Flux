using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    public abstract class ConditionalJump : Effect
    {
        [SerializeField] private Marker target;
        
        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            if (IsConditionMet())
            {
                prolong = true;
                
                if (target.Index < advancement) for (var i = target.Index; i < advancement; i++) registry[i].Initialize();
                return target.Index;
            }
            else return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }

        protected abstract bool IsConditionMet();
    }
}