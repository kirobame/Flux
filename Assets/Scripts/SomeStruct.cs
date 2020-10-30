using System;
using UnityEngine;

namespace Flux
{
    [Serializable]
    public struct SomeStruct
    {
        [SerializeField] private int number;
        [SerializeField] private string text;
        [SerializeField] private SubStruct sub;
    }
}