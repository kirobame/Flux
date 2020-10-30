using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Flux
{
    [Serializable]
    public class OperationGroup
    {
        public InputHandler InputHandler => runtimeInputHandler;
        
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private Operation[] operations;

        private InputHandler runtimeInputHandler;
        private Operation[] runtimeOperations;
        
        public void Initialize(OperationHandler operationHandler, InputAction inputAction)
        {
            runtimeInputHandler = Object.Instantiate(inputHandler);
            runtimeInputHandler.Initialize();
            runtimeInputHandler.Bind(inputAction);
            
            runtimeOperations = new Operation[operations.Length];
            for (var i = 0; i < operations.Length; i++)
            {
                var runtimeOperation = Object.Instantiate(operations[i]);
                runtimeOperation.Initialize(operationHandler);
                runtimeOperation.Bind(runtimeInputHandler);

                runtimeOperations[i] = runtimeOperation;
            }
        }
    }
}