using UnityEngine;

namespace Flux
{
    public abstract class Referencer : MonoBehaviour
    {
        #region Encapsulated Types

        public enum Mode
        {
            Global,
            Local
        }
        #endregion
        
        public abstract string Address { get; }

        [SerializeField] private Object[] values;
        [SerializeField] private Mode mode;
        [SerializeField] private Object key;

        void OnEnable()
        {
            if (mode == Mode.Global) Repository.Reference(values, Address);
            else Repository.Reference(values, Address, key);
        }
        void OnDisable()
        {
            if (mode == Mode.Global) Repository.Dereference(values, Address);
            else Repository.Dereference(values, Address, key);
        }
    }
}