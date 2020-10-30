using System;

namespace Flux
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EffectAttribute : Attribute
    {
        public EffectAttribute(string path) => this.path = path;

        public string Path => path;
        private string path;
    }
}