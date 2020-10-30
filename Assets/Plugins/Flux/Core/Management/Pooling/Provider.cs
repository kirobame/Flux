using System;
using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    public abstract class Provider { }
    [Serializable]
    public abstract class Provider<T, TPoolable> : Provider where TPoolable : Poolable<T>
    {
        public TPoolable Prefab => prefab;
        public int Count => count;

        public IReadOnlyList<TPoolable> Instances => instances;
            
        [SerializeField] private TPoolable prefab;
        [SerializeField] private int count = 1;
            
        [SerializeField] private TPoolable[] instances;
    }
}