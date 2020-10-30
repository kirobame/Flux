using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    public abstract class Effect : MonoBehaviour
    {
        public int Index => index;
        private int index;

        public void Bootup(int index) => this.index = index;
        public virtual void Initialize() { }
        
        public virtual int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            prolong = true;
            return advancement + 1;
        }
    }
}