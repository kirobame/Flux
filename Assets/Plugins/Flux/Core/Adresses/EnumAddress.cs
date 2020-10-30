using System;
using UnityEngine;

namespace Flux
{
    [Serializable]
    public struct EnumAddress
    {
        public Type Type => Type.GetType(stringedType);

        [SerializeField] private string stringedType;
        [SerializeField] private int rawValue;
        
        public string Get() => ((Enum)Enum.ToObject(Type, rawValue)).GetNiceName();
    }
}