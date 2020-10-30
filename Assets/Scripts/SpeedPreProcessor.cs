using UnityEngine;

namespace Flux
{
    [CreateAssetMenu(fileName = "NewSpeedPreProcessor", menuName = "Custom/Pre Processors/Speed")]
    public class SpeedPreProcessor : MoveOperationPreProcessor
    {
        [SerializeField] private float modification;
        
        private float cachedSpeed;
        
        public override void Affect(MoveOperation operation)
        {
            cachedSpeed = operation.runtimeSpeed;
            operation.runtimeSpeed += modification;
        }
        public override void Undo(MoveOperation operation) => operation.runtimeSpeed = cachedSpeed;
    }
}