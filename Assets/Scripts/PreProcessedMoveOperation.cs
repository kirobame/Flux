using UnityEngine;

namespace Flux
{
    [CreateAssetMenu(fileName = "NewPreProcessedMoveOperation", menuName = "Custom/Operations/Pre Processed/Move")]
    public class PreProcessedMoveOperation : PreProcessedOperation<MoveOperation, MoveOperationPreProcessor> { }
}