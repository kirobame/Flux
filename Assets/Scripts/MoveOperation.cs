using System;
using UnityEngine;

namespace Flux
{
    [CreateAssetMenu(fileName = "NewMoveOperation", menuName = "Custom/Operations/Move")]
    public class MoveOperation : SingleOperation
    {
        [SerializeField] private float speed;

        [HideInInspector] public float runtimeSpeed;

        public override void Initialize(OperationHandler operationHandler)
        {
            base.Initialize(operationHandler);

            Event.Open(PlayerEvent.Move);
            runtimeSpeed = speed;
        }

        public override void OnStart(EventArgs inArgs) { if (inArgs is Vector2EventArgs vector2Args) Begin(vector2Args); }
        public override void OnUpdate(EventArgs inArgs)
        {
            if (!(inArgs is Vector2EventArgs vector2Args)) return;

            var move = new Vector3(vector2Args.value.x, 0f, vector2Args.value.y);
            operationHandler.transform.position += move * (runtimeSpeed * Time.deltaTime);
            Event.Call(PlayerEvent.Move);
            
            Prolong(vector2Args);
        }
        public override void OnEnd(EventArgs inArgs) { if (inArgs is Vector2EventArgs vector2Args) End(vector2Args); }

        public override string ToString() => base.ToString() + $" : {runtimeSpeed}";
    }
}