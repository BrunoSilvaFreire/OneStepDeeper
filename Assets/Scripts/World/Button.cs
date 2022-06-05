using OSD.Movement;
using Sirenix.OdinInspector;
using UnityEngine;
namespace OSD.World {
    public class Button : MonoBehaviour, IInteractable {
        [Required]
        public Door door;

        public void Interact() {
            door.Open();
        }
    }
}