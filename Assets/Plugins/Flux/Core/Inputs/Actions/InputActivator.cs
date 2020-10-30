using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flux
{
    public class InputActivator : MonoBehaviour
    {
        #region Encapsuled Types

        [Serializable]
        public struct MapLink
        {
            public string Name => name;
            public bool IsEnabled => isEnabled;
            
            [SerializeField] private string name;
            [SerializeField] private bool isEnabled;
        }
        #endregion
        
        [SerializeField] private InputActionAsset asset;
        [SerializeField] private MapLink[] links;
        
        void Awake() { foreach (var link in links) { if (link.IsEnabled) asset.FindActionMap(link.Name).Enable(); } }

        void OnEnable() => asset.Enable();
        void OnDisable() => asset.Disable();

        void OnDestroy() { foreach (var link in links) { asset.FindActionMap(link.Name).Disable(); } }
    }
}