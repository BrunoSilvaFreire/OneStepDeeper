using System.Linq;
using Lunari.Tsuki;
using Lunari.Tsuki.Entities;
using Lunari.Tsuki2D.Runtime.Movement;
using Lunari.Tsuki2D.Runtime.Movement.Attachments;
using UnityEngine;
namespace OSD.Movement {
    public interface IInteractable {
        void Interact();
    }

    public class InteractionAttachment : MotorAttachmentWithInput<OSDPlatformerInput> {
        public float interactionRadius = 3;
        public LayerMask interactableMask;
        public override void Tick(Motor motor, OSDPlatformerInput input, ref Vector2 velocity) {
            if (input.Interact.Consume()) {
                var results = new Collider2D[4];
                var size = Physics2D.OverlapCircleNonAlloc(transform.position, interactionRadius, results, interactableMask);
                if (size <= 0) {
                    return;
                }

                var interactables = results
                    .Take(size)
                    .Select(col => col.GetComponentInChildren<IInteractable>())
                    .Where(interactable => interactable != null)
                    .ToArray();

                if (interactables.IsNullOrEmpty()) {
                    return;
                }

                interactables.MinBy(DistanceToThis).Interact();
            }
        }
        private float DistanceToThis(IInteractable col) {
            return Vector2.Distance(((Component) col).transform.position, transform.position);
        }
    }
}