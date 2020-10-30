using System;

namespace Flux
{
    public abstract class TrackReferencingAttribute : Attribute
    {
        public abstract TrackedReference[] Track(Type type);
    }
}