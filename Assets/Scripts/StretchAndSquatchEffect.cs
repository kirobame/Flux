using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    [Effect("Custom/Stretch & Squatch")]
    public class StretchAndSquatchEffect : Effect
    {
        [SerializeField] private Transform target;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float heightTarget;
        [SerializeField] private float time;

        private float heightStart;
        private float clock;

        void Awake() => heightStart = target.transform.position.y;

        public override void Initialize()
        {
            base.Initialize();
            clock = 0f;
        }

        public override int Evaluate(int advancement, IReadOnlyList<Effect> registry, float deltaTime, out bool prolong)
        {
            if (clock > time) clock = 0f;
            
            var position = target.transform.position;
            position.y = Mathf.Lerp(heightStart, heightTarget, curve.Evaluate(Mathf.Clamp01(clock / time)));
            target.transform.position = position;
            
            clock += deltaTime;
            return base.Evaluate(advancement, registry, deltaTime, out prolong);
        }
    }
}