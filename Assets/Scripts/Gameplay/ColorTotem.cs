using System;
using Lunari.Entities.Network;
using Lunari.Tsuki;
using Lunari.Tsuki.Entities;
using OSD.Network;
using Unity.Netcode;
using UnityEngine;
namespace OSD.Gameplay {
    [ExecuteAlways]
    public class ColorTotem : NetworkTrait {
        public ColorBlindnessType colorBlindness;
        private Slot<ColorBlindness> _inUse;
        private Collider2D _trigger;
        private SpriteRenderer _renderer;
        public override void Configure(TraitDescriptor descriptor) {
            if (descriptor.RequiresComponent(out _trigger, out _renderer)) {
                if (ColorBlindnessDatabase.Instance.entries.TryGetValue(colorBlindness, out var entry)) {
                    _renderer.color = entry.characterColor.SetAlpha(0.5F);
                    _renderer.material = entry.material;
                }
            }
            if (descriptor.Initialize) {
                _inUse = new Slot<ColorBlindness>().OnChangedNotNull(OnSet).OnChangedToNull(OnNull);
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (!IsOwner) {
                return;
            }
            if (_inUse.Value != null) {
                return;
            }
            if (!col.FindTrait(out ColorBlindness cb)) {
                return;
            }
            Apply(cb);
        }

        public void ClearCurrentOwner() {
            if (!IsOwner) {
                return;
            }
            _inUse.Clear();
            SetAvailableClientRpc();
        }

        public void Apply(ColorBlindness to) {
            _inUse.Value = to;
            SetInUseByClientRpc(to);
        }

        [ClientRpc]
        private void SetAvailableClientRpc() {
            _inUse.Clear();

        }

        [ClientRpc]
        private void SetInUseByClientRpc(NetworkBehaviourReference other) {
            if (other.TryGet(out ColorBlindness colorBlindness)) {
                _inUse.Value = colorBlindness;
            }
        }

        private void OnNull() {
            _renderer.enabled = true;
        }

        private void OnSet(ColorBlindness arg0) {
            if (IsOwner) {
                _inUse.Listen(
                    arg0.currentType,
                    ClearCurrentOwner
                );
                arg0.currentType.Value = colorBlindness;
            }
            _renderer.enabled = false;
        }
    }
}