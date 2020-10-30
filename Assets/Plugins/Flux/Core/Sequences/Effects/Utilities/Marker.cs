using UnityEngine;

namespace Flux
{
    [Effect("Utility/Marker")]
    public class Marker : Effect
    {
        public string Name => name;
        [SerializeField] private new string name;
    }
}