using System;
using System.Collections;
using System.Collections.Generic;

namespace Flux
{
    public static class Repository 
    {
        private static Dictionary<string, List<object>> globalReferences = new Dictionary<string, List<object>>();
        private static Dictionary<string, Dictionary<int, List<object>>> localReferences = new Dictionary<string, Dictionary<int, List<object>>>();

        public static void Reference(object value, Enum address) => Reference(value, address.GetNiceName());
        public static void Reference(object value, string address)
        {
            if (globalReferences.TryGetValue(address, out var list)) list.Add(value);
            else globalReferences.Add(address, new List<object>() {value});
        }
        public static void Reference(IEnumerable<object> values, Enum address) => Reference(values, address.GetNiceName());
        public static void Reference(IEnumerable<object> values, string address)
        {
            if (globalReferences.TryGetValue(address, out var list)) foreach (var value in values) list.Add(value);
            else globalReferences.Add(address, new List<object>() {values});
        }

        public static void Reference(object value, Enum address, object key) => Reference(value, address.GetNiceName(), key);
        public static void Reference(object value, string address, object key)
        {
            if (localReferences.TryGetValue(address, out var subDictionary))
            {
                var hashCode = key.GetHashCode();
                
                if (subDictionary.TryGetValue(hashCode, out var list)) list.Add(value);
                else subDictionary.Add(hashCode, new List<object>(){value});
            }
            else
            {
                localReferences.Add(address, new Dictionary<int, List<object>>());
                localReferences[address].Add(key.GetHashCode(), new List<object>(){value});
            }
        }
        public static void Reference(IEnumerable<object> values, Enum address, object key) => Reference(values, address.GetNiceName(), key);
        public static void Reference(IEnumerable<object> values, string address, object key)
        {
            if (localReferences.TryGetValue(address, out var subDictionary))
            {
                var hashCode = key.GetHashCode();
                
                if (subDictionary.TryGetValue(hashCode, out var list)) foreach (var value in values) list.Add(value);
                else subDictionary.Add(hashCode, new List<object>(){values});
            }
            else
            {
                localReferences.Add(address, new Dictionary<int, List<object>>());
                localReferences[address].Add(key.GetHashCode(), new List<object>(){values});
            }
        }

        public static void Dereference(object value, Enum address) => Dereference(value, address.GetNiceName());
        public static void Dereference(object value, string address) { if (globalReferences.TryGetValue(address, out var list)) list.Remove(value); }
        public static void Dereference(IEnumerable<object> values, Enum address) => Dereference(values, address.GetNiceName());
        public static void Dereference(IEnumerable<object> values, string address) { if (globalReferences.TryGetValue(address, out var list)) foreach (var value in values) list.Remove(value); }

        public static void Dereference(object value, Enum address, object key) => Dereference(value, address.GetNiceName(), key);
        public static void Dereference(object value, string address, object key)
        {
            if (!localReferences.TryGetValue(address, out var subDictionary)) return;
            if (subDictionary.TryGetValue(key.GetHashCode(), out var list)) list.Remove(value);
        }
        public static void Dereference(IEnumerable<object> values, Enum address, object key) => Dereference(values, address.GetNiceName(), key);
        public static void Dereference(IEnumerable<object> values, string address, object key)
        {
            if (!localReferences.TryGetValue(address, out var subDictionary)) return;
            if (subDictionary.TryGetValue(key.GetHashCode(), out var list)) foreach (var value in values) list.Remove(value);
        }

        public static void SetSingle(object value, Enum address) => Set(value, 0, address.GetNiceName());
        public static void SetSingle(object value, string address) => Set(value, 0, address);
        
        public static void Set(object value, int index, Enum address) => Set(value, index, address.GetNiceName());
        public static void Set(object value, int index, string address) => globalReferences[address][index] = value;

        public static void SetSingle(object value, Enum address, object key) => Set(value, 0, address.GetNiceName(), key);
        public static void SetSingle(object value, string address, object key) => Set(value, 0, address, key);
        
        public static void Set(object value, int index, Enum address, object key) => Set(value, index, address.GetNiceName(), key);
        public static void Set(object value, int index, string address, object key) => localReferences[address][key.GetHashCode()][index] = value;

        public static T GetSingle<T>(Enum address) => Get<T>(address.GetNiceName(), 0);
        public static T GetSingle<T>(string address) => Get<T>(address, 0);
        
        public static T Get<T>(Enum address, int index) => Get<T>(address.GetNiceName(), index);
        public static T Get<T>(string address, int index) => (T)globalReferences[address][index];

        public static T GetSingle<T>(Enum address, object key) => Get<T>(address.GetNiceName(), 0, key);
        public static T GetSingle<T>(string address, object key) => Get<T>(address, 0, key);
        
        public static T Get<T>(Enum address, int index, object key) => Get<T>(address.GetNiceName(), index, key);
        public static T Get<T>(string address, int index, object key) => (T)localReferences[address][key.GetHashCode()][index];
    }
}