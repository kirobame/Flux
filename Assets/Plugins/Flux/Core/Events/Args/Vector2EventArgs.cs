using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    public class Vector2EventArgs : EventArgs
    {
        public Vector2EventArgs(Vector2 value) => this.value = value;
        
        public readonly Vector2 value;
    }
}