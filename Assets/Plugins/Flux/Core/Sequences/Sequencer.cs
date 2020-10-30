using System;
using UnityEngine;

namespace Flux
{
    public class Sequencer : MonoBehaviour
    {
        [SerializeField] private Effect[] effects;

        private bool isPlaying;
        private int advancement;

        void Start() { for (var i = 0; i < effects.Length; i++) effects[i].Bootup(i); }

        public void Play()
        {
            isPlaying = true;
            foreach (var effect in effects) effect.Initialize();
            
            advancement = 0;
            while (!effects[advancement].enabled)
            {
                advancement++;
                if (advancement < effects.Length) continue;
                
                Stop();
                return;
            }
        }
        public void Stop()
        {
            isPlaying = false;
            advancement = -1;
        }

        public void Pause() => isPlaying = false;
        public void Resume() => isPlaying = true;

        void Update()
        {
            if (!isPlaying || advancement < 0) return;
            Execute();
        }
        public void UpdateManually()
        {
            if (advancement < 0) advancement = 0;
            Execute();
        }

        private void Execute()
        {
            var prolong = true;
            while (prolong)
            {
                var next = effects[advancement].Evaluate(advancement, effects, Time.deltaTime, out prolong);
                if (next == advancement) continue;
                
                if (next < 0 || next >= effects.Length)
                {
                    Stop();
                    return;
                }

                while (!effects[next].enabled)
                {
                    next++;
                    if (next < effects.Length) continue;
                    
                    Stop();
                    return;
                }

                advancement = next;
            }
        }

        private bool TryBreak(int index, bool prolong)
        {
            if (index < 0 || index >= effects.Length)
            {
                Stop();
                return false;
            }

            return prolong;
        }
    }
}