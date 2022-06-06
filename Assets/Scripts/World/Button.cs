using OSD.Movement;
using Sirenix.OdinInspector;
using UnityEngine;
namespace OSD.World {
    public class Button : MonoBehaviour, IInteractable {
        [Required]
        public Door door;
        [Required]
        public Door[] toClose;
        public void Interact() {
            door.Open();
            foreach (var d in toClose) {
                d.Close();
            }
        }
    }
}