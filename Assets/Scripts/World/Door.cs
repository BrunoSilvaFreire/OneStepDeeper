using OSD.Movement;
using Unity.Netcode;
using UnityEngine;
namespace OSD.World {
    public class Door : NetworkBehaviour {
        public new Collider2D collider;
        public new SpriteRenderer renderer;


        public void Open() {
            if (IsOwner) {
                SetEnabledClientRpc(false);
            } else {
                SetComponentsEnabled(false);
                RemoteEnabledServerRpc(false);
            }
        }

        public void Close() {
            if (IsOwner) {
                SetEnabledClientRpc(true);
            } else {
                SetComponentsEnabled(true);
                RemoteEnabledServerRpc(true);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void RemoteEnabledServerRpc(bool v) {
            if (!v) {
                Open();
            } else {
                Close();
            }
        }

        [ClientRpc]
        private void SetEnabledClientRpc(bool enabled) {
            SetComponentsEnabled(enabled);
        }
        private void SetComponentsEnabled(bool enabled) {
            collider.enabled = enabled;
            renderer.enabled = enabled;
        }

    }
}