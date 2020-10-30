using System;
using UnityEngine;

namespace Flux
{
    public class LightPool : Pool<Light, PoolableLight>
    {
        #region Encapsulated Types

        [Serializable]
        public class LightProvider : Provider<Light, PoolableLight> { }

        #endregion

        protected override Provider<Light, PoolableLight>[] Providers => providers;
        [SerializeField] private LightProvider[] providers;
    }
}