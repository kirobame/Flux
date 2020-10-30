using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Flux
{
    public class Helper : MonoBehaviour
    {
        #region Encapsulated Types

        [EnumAddress]
        public enum EventType
        {
            One,
            Two, 
            Three
        }

        [EnumAddress, TrackEnumReferencing]
        public enum Reference
        {
            Camera,
            Transform,
            Other,
        }
        #endregion

        void Start()
        {
            Debug.Log(EventType.Two);
            Event.Open<long>(EventType.Two);
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O)) Event.Call<long>(EventType.Two, 10L);
        }
    }
}