using System;
using System.Collections.Generic;
using Lunari.Entities.Network;
using Lunari.Tsuki;
using Lunari.Tsuki.Entities;
using OSD.Network;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace OSD.Gameplay {
    [ExecuteAlways]
    public class ColorTotem : NetworkTrait {
        public ColorBlindnessType colorBlindness;
        private Slot<ColorBlindness> _inUse;
        private Collider2D _trigger;
        public List<ParticleSystem> particles;
        public List<Light2D> lights;
        public ParticleSystem pickUpPrefab;
        private ColorBlindnessEntry _entry;
        public override void Configure(TraitDescriptor descriptor) {
            if (descriptor.RequiresComponent(out _trigger)) {
                FetchColorFromDatabase();
            }
            if (descriptor.Initialize) {
                _inUse = new Slot<ColorBlindness>().OnChangedNotNull(OnSet).OnChangedToNull(OnNull);
            }
        }

        public override void OnDestroy() {
            _inUse.Clear();
        }
        private void FetchColorFromDatabase() {
            if (ColorBlindnessDatabase.Instance.entries.TryGetValue(colorBlindness, out _entry)) {
                SetColor(_entry.characterColor);
                SetMaterial(_entry.material);
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
        private void SetColor(Color color) {
            foreach (var particle in particles) {
                var module = particle.main;
                module.startColor = color;
            }
            foreach (var l in lights) {
                l.color = color;
            }
        }
        private void OnValidate() {
            FetchColorFromDatabase();
        }

        private void SetMaterial(Material valueMaterial) {
            foreach (var particle in particles) {
                particle.GetComponent<ParticleSystemRenderer>().material = valueMaterial;
            }
        }
        private void OnNull() {
            SetEnabled(true);
        }
        private void SetEnabled(bool b) {
            foreach (var particle in particles) {
                if (particle == null) {
                    continue;
                }
                if (b) {
                    particle.Play();
                } else {
                    particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }

        private void OnSet(ColorBlindness user) {
            if (IsOwner) {
                // Check if the user has picked up another color
                user.currentType.Value = colorBlindness;
                _inUse.Listen(
                    user.currentType,
                    ClearCurrentOwner
                );
            }
            SetEnabled(false);

            var system = pickUpPrefab.Clone(transform.position);
            var main = system.main;
            main.startColor = _entry.characterColor;
        }
    }
}