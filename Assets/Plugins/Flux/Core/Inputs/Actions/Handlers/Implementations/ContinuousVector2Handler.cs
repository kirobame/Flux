using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    [CreateAssetMenu(fileName = "NewContinuousVector2Handler", menuName = "Flux/Input/Handlers/Continuous Vector2")]
    public class ContinuousVector2Handler : ContinuousHandler<Vector2>
    {
        protected override void OnStart(Vector2 input)
        {
            base.OnStart(input);
            Begin(new Vector2EventArgs(input));
        }
        protected override void OnCanceled(Vector2 input)
        {
            End(new Vector2EventArgs(input));
            base.OnCanceled(input);
        }

        protected override void HandleInput(Vector2 input) => Prolong(new Vector2EventArgs(input));
    }
}