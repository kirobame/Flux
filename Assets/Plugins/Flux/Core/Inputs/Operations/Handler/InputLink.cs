using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Flux
{
    [Serializable]
    public class InputLink
    {
        public IEnumerable<InputHandler> Handlers => groups.Select(group => group.InputHandler);
        
        [SerializeField] private InputActionReference reference;
        [SerializeField] private OperationGroup[] groups;

        public void Initialize(OperationHandler operationHandler) { foreach (var group in groups) group.Initialize(operationHandler, reference.action); }
    }
}