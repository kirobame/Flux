using UnityEngine;

namespace Flux
{
    public abstract class PreProcessor : ScriptableObject
    {
        public abstract void Affect(SingleOperation operation);
        public abstract void Undo(SingleOperation operation);
    }
    public abstract class PreProcessor<TOp> : PreProcessor where TOp : SingleOperation
    {
        public override void Affect(SingleOperation operation) => Affect((TOp)operation);
        public abstract void Affect(TOp operation);
        
        public override void Undo(SingleOperation operation)=> Undo((TOp)operation);
        public abstract void Undo(TOp operation);
    }
}