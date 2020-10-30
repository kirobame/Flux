using UnityEngine;

namespace Flux
{
    public class EnumReferencer : Referencer
    {
        public override string Address => address.Get();
        [SerializeField] private EnumAddress address;
    }
}