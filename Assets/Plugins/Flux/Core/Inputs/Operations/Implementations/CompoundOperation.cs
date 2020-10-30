using UnityEngine;

namespace Flux
{
    [CreateAssetMenu(fileName = "NewCompoundOperation", menuName = "Flux/Operations/Compound")]
    public class CompoundOperation : Operation
    {
        [SerializeField] private Operation parentOperation;
        [SerializeField] private Operation[] subOperations;

        protected Operation runtimeParentOperation => runtimeOperations[0];
        private Operation[] runtimeOperations;

        public override void Initialize(OperationHandler operationHandler)
        {
            base.Initialize(operationHandler);
            
            runtimeOperations = new Operation[subOperations.Length + 1];
            runtimeOperations[0] = Instantiate(parentOperation);
            runtimeParentOperation.Initialize(operationHandler);

            for (var i = 1; i < runtimeOperations.Length; i++)
            {
                var runtimeSubOperation = Instantiate(subOperations[i - 1]);
                runtimeSubOperation.Initialize(operationHandler);
                runtimeSubOperation.Bind(runtimeParentOperation);
                
                runtimeOperations[i] = runtimeSubOperation;
            }
        }

        public override void Bind(InputHandler inputHandler) => runtimeParentOperation.Bind(inputHandler);
        public override void Bind(Operation operation) => runtimeParentOperation.Bind(operation);
        public override void Unbind() => runtimeParentOperation.Unbind();
    }
}