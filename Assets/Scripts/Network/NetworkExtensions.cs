using Lunari.Tsuki;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.Events;
namespace OSD.Network {
    public static class NetworkExtensions {
        public static Slot<T> Listen<T, A>(this Slot<T> slot, NetworkVariable<A> variable, NetworkVariable<A>.OnValueChangedDelegate listener) where T : class where A : unmanaged {
            variable.OnValueChanged += listener;
            slot.OnChanged(() =>
            {
                variable.OnValueChanged -= listener;
            });
            return slot;
        }
        public static Slot<T> Listen<T, A>(this Slot<T> slot, NetworkVariable<A> variable, UnityAction<A> listener) where T : class where A : unmanaged {

            return slot.Listen(variable, (_, newV) => listener.Invoke(newV));
        }

        public static Slot<T> ListenAndInvoke<T, A>(this Slot<T> slot, NetworkVariable<A> variable, UnityAction<A> listener) where T : class where A : unmanaged {
            slot.Listen(variable, listener);
            listener(variable.Value);
            return slot;
        }
        public static Slot<T> Listen<T, A>(this Slot<T> slot, NetworkVariable<A> variable, UnityAction listener) where T : class where A : unmanaged {

            return slot.Listen(variable, (_, _) => listener());
        }

        public static Slot<T> ListenAndInvoke<T, A>(this Slot<T> slot, NetworkVariable<A> variable, UnityAction listener) where T : class where A : unmanaged {
            slot.Listen(variable, listener);
            listener();
            return slot;
        }
    }
}