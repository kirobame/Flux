using UnityEngine;

namespace Flux
{
    [Effect("Utility/Jumps/Loop")]
    public class Loop : ConditionalJump
    {
        [SerializeField] private int repetitions;

        private int countdown;

        public override void Initialize() => countdown = repetitions + 1;

        protected override bool IsConditionMet()
        {
            countdown--;
            return countdown > 0;
        }
    }
}