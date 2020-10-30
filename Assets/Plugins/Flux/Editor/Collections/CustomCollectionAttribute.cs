using System;

namespace Flux.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomCollectionAttribute : Attribute
    {
        public CustomCollectionAttribute(Type target) => this.target = target;
        
        public Type Target => target;
        private Type target;
    }
}