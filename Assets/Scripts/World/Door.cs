using OSD.Movement;
using Unity.Netcode;
using UnityEngine;
namespace OSD.World {
    public class Door : NetworkBehaviour {
        public new Collider2D collider;
        public new SpriteRenderer renderer;

        public void Open() {
            DisableDoor();
            if (IsOwner) {
                NotifyOpenClientRpc();
            }
        }
        private void DisableDoor() {
            collider.enabled = false;
            renderer.enabled = false;
        }
        [ClientRpc]
        private void NotifyOpenClientRpc() {
            DisableDoor();
        }

        public void Close() {
            collider.enabled = true;
            renderer.enabled = true;
        }

    }
}