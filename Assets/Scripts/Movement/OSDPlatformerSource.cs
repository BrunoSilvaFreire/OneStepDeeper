using Lunari.Tsuki2D.Platforming.Input;
using UnityEngine.InputSystem;
namespace OSD {
    public class OSDPlatformerSource : InputSystemPlatformerSource {
        private InputAction _interact;
        protected override void Start() {
            base.Start();
            if (Input != null) {
                _interact = Input.actions["Interact"];
            }
        }
        protected override void TransferTo(IPlatformerInput input) {
            if (input is not OSDPlatformerInput osd) {
                return;
            }
            base.TransferTo(input);
        }
        public bool GetInteract() {
            if (_interact == null) {
                return false;
            }
            return _interact.triggered || _interact.inProgress;
        }
    }
}