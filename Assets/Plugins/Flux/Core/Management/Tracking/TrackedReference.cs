using System;

namespace Flux
{
    public struct TrackedReference : IEquatable<TrackedReference>
    {
        public TrackedReference(string address, bool isReferenced, Referencer referencer)
        {
            Address = address;
            IsReferenced = isReferenced;
            Referencer = referencer;
        }
        
        public string Address { get; private set; }
        public bool IsReferenced { get; private set; }
        public Referencer Referencer { get; private set; }

        public bool Equals(TrackedReference other) => Address == other.Address;
        public override bool Equals(object obj) => obj is TrackedReference other && Equals(other);
        public override int GetHashCode() => Address != null ? Address.GetHashCode() : 0;
    }
}