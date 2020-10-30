using System;
using UnityEngine;

namespace Flux
{
    [CreateAssetMenu(fileName = "NewDisplayOperation", menuName = "Custom/Operations/Display")]
    public class DisplayOperation : SingleOperation
    {
        [SerializeField] private string startMessage;
        [SerializeField] private string updateMessage;
        [SerializeField] private string endMessage;

        public override void OnStart(EventArgs inArgs) => Debug.Log($"{GetInstanceID()} / {startMessage}");
        public override void OnUpdate(EventArgs inArgs) => Debug.Log($"{GetInstanceID()} / {updateMessage}");
        public override void OnEnd(EventArgs inArgs) => Debug.Log($"{GetInstanceID()} / {endMessage}");
    }
}