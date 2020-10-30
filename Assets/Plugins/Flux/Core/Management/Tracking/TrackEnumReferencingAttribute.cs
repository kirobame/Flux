using System;
using System.Linq;
using Object = UnityEngine.Object;

namespace Flux
{
    public class TrackEnumReferencingAttribute : TrackReferencingAttribute
    {
        public override TrackedReference[] Track(Type type)
        {
            var enumValues = Enum.GetValues(type).Cast<Enum>().ToArray();
            var referencers = Object.FindObjectsOfType<EnumReferencer>().ToDictionary(referencer => referencer.Address);

            var trackedReferences = new TrackedReference[enumValues.Length];
            for (var i = 0; i < enumValues.Length; i++)
            {
                var address = enumValues[i].GetNiceName();
                if (referencers.TryGetValue(enumValues[i].GetNiceName(), out var referencer)) trackedReferences[i] = new TrackedReference(address, true, referencer);
                else trackedReferences[i] = new TrackedReference(address, false, null);
            }

            return trackedReferences;
        }
    }
}