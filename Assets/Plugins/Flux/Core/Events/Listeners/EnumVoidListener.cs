using UnityEngine;

namespace Flux
{
    public class EnumVoidListener : VoidListener
    {
        protected override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
}