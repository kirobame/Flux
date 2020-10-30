using System;
using UnityEngine;

namespace Flux
{
    [Serializable]
    public struct SubStruct
    {
        [SerializeField] private ForceMode forceMode;
        [SerializeField] private bool state;
    }
}