using Lunari.Entities.Network;
using Lunari.Tsuki.Entities;
using Unity.Netcode;
using UnityEngine;
namespace OSD.Gameplay {
    public class ColorBlindness : NetworkTrait {
        [SerializeField]
        public NetworkVariable<ColorBlindnessType> currentType;
        private SpriteRenderer _spriteRenderer;
        private Material originalMaterial;
        private Color originalColor;
        public override void Configure(TraitDescriptor descriptor) {
            descriptor.RequiresComponent(out _spriteRenderer);
            if (descriptor.Initialize) {
                originalColor = _spriteRenderer.color;
                originalMaterial = _spriteRenderer.material;
                currentType.OnValueChanged += OnBlindnessChanged;
            }
        }
        private void OnBlindnessChanged(ColorBlindnessType previousvalue, ColorBlindnessType newvalue) {
            if (ColorBlindnessDatabase.Instance.entries.TryGetValue(newvalue, out var val)) {
                _spriteRenderer.material = val.material;
                _spriteRenderer.color = val.characterColor;
            } else {
                _spriteRenderer.material = originalMaterial;
                _spriteRenderer.color = originalColor;
            }
        }
    }
}