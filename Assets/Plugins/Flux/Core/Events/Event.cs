using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Flux
{
    public static class Event
    {
        #region Encapsulated Types

        private class Port
        {
            public Port(string address) => Address = address;

            public bool isBlocked;
            
            public readonly string Address;
        }
        private class VoidPort : Port
        {
            public VoidPort(string address) : base(address) { }

            private event Action callback;

            public void Invoke()
            {
                if (isBlocked) return;
                callback?.Invoke();
            }

            public void Register(Action action) => callback += action;
            public void Unregister(Action action) => callback += action;
        }
        private class Port<T> : Port
        {
            public Port(string address) : base(address) { }

            private event Action<T> callback;

            public void Invoke(T arg)
            {
                if (isBlocked) return;
                callback?.Invoke(arg);
            }

            public void Register(Action<T> action) => callback += action;
            public void Unregister(Action<T> action) => callback += action;
        }
        private class Port<T1,T2> : Port
        {
            public Port(string address) : base(address) { }

            private event Action<T1,T2> callback;

            public void Invoke(T1 arg1, T2 arg2)
            {
                if (isBlocked) return;
                callback?.Invoke(arg1, arg2);
            }

            public void Register(Action<T1,T2> action) => callback += action;
            public void Unregister(Action<T1,T2> action) => callback += action;
        }
        private class Port<T1,T2,T3> : Port
        {
            public Port(string address) : base(address) { }

            private event Action<T1,T2,T3> callback;

            public void Invoke(T1 arg1, T2 arg2, T3 arg3)
            {
                if (isBlocked) return;
                callback?.Invoke(arg1, arg2, arg3);
            }

            public void Register(Action<T1,T2,T3> action) => callback += action;
            public void Unregister(Action<T1,T2,T3> action) => callback += action;
        }
        private class Port<T1,T2,T3,T4> : Port
        {
            public Port(string address) : base(address) { }

            private event Action<T1,T2,T3,T4> callback;

            public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                if (isBlocked) return;
                callback?.Invoke(arg1, arg2, arg3, arg4);
            }

            public void Register(Action<T1,T2,T3,T4> action) => callback += action;
            public void Unregister(Action<T1,T2,T3,T4> action) => callback += action;
        }
        #endregion
        
        private static Dictionary<string, Port> ports = new Dictionary<string, Port>();
        private static Dictionary<string, HashSet<object>> queuedCallbacks = new Dictionary<string, HashSet<object>>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Clear()
        {
            ports.Clear();
            queuedCallbacks.Clear();
        }

        public static bool Open(Enum address) => Open(address.GetNiceName());
        public static bool Open(object address)
        {
            var stringedAddress = address.ToString();
            if (ports.ContainsKey(stringedAddress)) return false;

            var port = new VoidPort(stringedAddress);
            ports.Add(stringedAddress, port);
            
            if (!queuedCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action castedCallback) port.Register(castedCallback);
            }
            queuedCallbacks.Remove(stringedAddress);

            return true;
        }

        public static bool Open<T>(Enum address) => Open<T>(address.GetNiceName());
        public static bool Open<T>(object address)
        {
            var stringedAddress = address.ToString();
            if (ports.ContainsKey(stringedAddress)) return false;
            
            var port = new Port<T>(stringedAddress);
            ports.Add(stringedAddress, port);
            
            if (!queuedCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action<T> castedCallback) port.Register(castedCallback);
            }
            queuedCallbacks.Remove(stringedAddress);

            return true;
        }
        
        public static bool Open<T1,T2>(Enum address) => Open<T1,T2>(address.GetNiceName());
        public static bool Open<T1,T2>(object address)
        {
            var stringedAddress = address.ToString();
            if (ports.ContainsKey(stringedAddress)) return false;
            
            var port = new Port<T1,T2>(stringedAddress);
            ports.Add(stringedAddress, port);
            
            if (!queuedCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action<T1,T2> castedCallback) port.Register(castedCallback);
            }
            queuedCallbacks.Remove(stringedAddress);

            return true;
        }
        
        public static bool Open<T1,T2,T3>(Enum address) => Open<T1,T2,T3>(address.GetNiceName());
        public static bool Open<T1,T2,T3>(object address)
        {
            var stringedAddress = address.ToString();
            if (ports.ContainsKey(stringedAddress)) return false;

            var port = new Port<T1,T2,T3>(stringedAddress);
            ports.Add(stringedAddress, port);
           
            if (!queuedCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action<T1,T2,T3> castedCallback) port.Register(castedCallback);
            }
            queuedCallbacks.Remove(stringedAddress);

            return true;
        }
        
        public static bool Open<T1,T2,T3,T4>(Enum address) => Open<T1,T2,T3,T4>(address.GetNiceName());
        public static bool Open<T1,T2,T3,T4>(object address)
        {
            var stringedAddress = address.ToString();
            if (ports.ContainsKey(stringedAddress)) return false;

            var port = new Port<T1,T2,T3,T4>(stringedAddress);
            ports.Add(stringedAddress, port);

            if (!queuedCallbacks.TryGetValue(stringedAddress, out var hashSet)) return true;
            foreach (var callback in hashSet)
            {
                if (callback is Action<T1,T2,T3,T4> castedCallback) port.Register(castedCallback);
            }
            queuedCallbacks.Remove(stringedAddress);

            return true;
        }

        public static bool Close(Enum address) => Close(address.GetNiceName());
        public static bool Close(object address) => ports.Remove(address.ToString());
        
        public static void Block(object address) => ports[address.ToString()].isBlocked = true;
        public static void Unblock(object address) => ports[address.ToString()].isBlocked = false;

        public static void Call(Enum address) => Call(address.GetNiceName());
        public static void Call(object address) => ((VoidPort)ports[address.ToString()]).Invoke();

        public static void Call<T>(Enum address, T arg) => Call(address.GetNiceName(), arg);
        public static void Call<T>(object address, T arg) => ((Port<T>)ports[address.ToString()]).Invoke(arg);

        public static void Call<T1, T2>(Enum address, T1 arg1, T2 arg2) => Call(address.GetNiceName(), arg1, arg2);
        public static void Call<T1,T2>(object address, T1 arg1, T2 arg2) => ((Port<T1,T2>)ports[address.ToString()]).Invoke(arg1, arg2);

        public static void Call<T1, T2, T3>(Enum address, T1 arg1, T2 arg2, T3 arg3) => Call(address.GetNiceName(), arg1, arg2, arg3);
        public static void Call<T1,T2,T3>(object address, T1 arg1, T2 arg2, T3 arg3) => ((Port<T1,T2,T3>)ports[address.ToString()]).Invoke(arg1, arg2, arg3);

        public static void Call<T1, T2, T3, T4>(Enum address, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => Call(address.GetNiceName(), arg1, arg2, arg3, arg4);
        public static void Call<T1,T2,T3,T4>(object address, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => ((Port<T1,T2,T3,T4>)ports[address.ToString()]).Invoke(arg1, arg2, arg3, arg4);

        public static bool Register(Enum address, Action callback) => Register(address.GetNiceName(), callback);
        public static bool Register(object address, Action callback)
        {
            var stringAddress = address.ToString();

            if (ports.ContainsKey(stringAddress))
            {
                ((VoidPort)ports[stringAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueCallback(address, callback);
                return false;
            }
        }
        public static bool Register<T>(Enum address, Action<T> callback) => Register(address.GetNiceName(), callback);
        public static bool Register<T>(object address, Action<T> callback)
        {
            var stringAddress = address.ToString();

            if (ports.ContainsKey(stringAddress))
            {
                ((Port<T>)ports[stringAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueCallback(address, callback);
                return false;
            }
        }
        public static bool Register<T1,T2>(Enum address, Action<T1,T2> callback) => Register(address.GetNiceName(), callback);
        public static bool Register<T1,T2>(object address, Action<T1,T2> callback)
        {
            var stringAddress = address.ToString();

            if (ports.ContainsKey(stringAddress))
            {
                ((Port<T1,T2>)ports[stringAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueCallback(address, callback);
                return false;
            }
        }
        public static bool Register<T1,T2,T3>(Enum address, Action<T1,T2,T3> callback) => Register(address.GetNiceName(), callback);
        public static bool Register<T1,T2,T3>(object address, Action<T1,T2,T3> callback)
        {
            var stringAddress = address.ToString();

            if (ports.ContainsKey(stringAddress))
            {
                ((Port<T1,T2,T3>)ports[stringAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueCallback(address, callback);
                return false;
            }
        }
        public static bool Register<T1,T2,T3,T4>(Enum address, Action<T1,T2,T3,T4> callback) => Register(address.GetNiceName(), callback);
        public static bool Register<T1,T2,T3,T4>(object address, Action<T1,T2,T3,T4> callback)
        {
            var stringAddress = address.ToString();

            if (ports.ContainsKey(stringAddress))
            {
                ((Port<T1,T2,T3,T4>)ports[stringAddress]).Register(callback);
                return true;
            }
            else
            {
                QueueCallback(address, callback);
                return false;
            }
        }
        private static void QueueCallback(object address, object callback)
        {
            var stringAddress = address.ToString();
            
            if (queuedCallbacks.TryGetValue(stringAddress, out var hashSet)) hashSet.Add(callback);
            else queuedCallbacks.Add(stringAddress, new HashSet<object>() {callback});
        }

        public static void Unregister(Enum address, Action callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister(object address, Action callback)
        {
            var stringAddress = address.ToString();
            
            if (!ports.ContainsKey(stringAddress)) return;
            ((VoidPort)ports[stringAddress]).Unregister(callback);
        }
        public static void Unregister<T>(Enum address, Action<T> callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister<T>(object address, Action<T> callback)
        {
            var stringAddress = address.ToString();
            
            if (!ports.ContainsKey(stringAddress)) return;
            ((Port<T>)ports[stringAddress]).Unregister(callback);
        }
        public static void Unregister<T1,T2>(Enum address, Action<T1,T2> callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister<T1,T2>(object address, Action<T1,T2> callback)
        {
            var stringAddress = address.ToString();
            
            if (!ports.ContainsKey(stringAddress)) return;
            ((Port<T1, T2>)ports[stringAddress]).Unregister(callback);
        }
        public static void Unregister<T1,T2,T3>(Enum address, Action<T1,T2,T3> callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister<T1,T2,T3>(object address, Action<T1,T2,T3> callback)
        {
            var stringAddress = address.ToString();
            
            if (!ports.ContainsKey(stringAddress)) return;
            ((Port<T1, T2, T3>)ports[stringAddress]).Unregister(callback);
        }
        public static void Unregister<T1,T2,T3,T4>(Enum address, Action<T1,T2,T3,T4> callback) => Unregister(address.GetNiceName(), callback);
        public static void Unregister<T1,T2,T3,T4>(object address, Action<T1,T2,T3,T4> callback)
        {
            var stringAddress = address.ToString();
            
            if (!ports.ContainsKey(stringAddress)) return;
            ((Port<T1, T2, T3, T4>)ports[stringAddress]).Unregister(callback);
        }
    }
}

