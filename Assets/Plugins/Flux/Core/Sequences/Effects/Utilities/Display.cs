using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    [Effect("Utility/Display")]
    public class Display : Effect
    {
        [SerializeField] private string message;
        
        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            Debug.Log(message);
            return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }
    }
}