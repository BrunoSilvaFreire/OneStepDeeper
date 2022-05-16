using Lunari.Tsuki2D.Platforming.Input;
using Lunari.Tsuki2D.Runtime.Input;
using UnityEngine;
namespace OSD {
    public class OSDPlatformerInput : EntityInput<OSDPlatformerSource>, IPlatformerInput {
        [SerializeField]
        private float horizontal;
        [SerializeField]
        private EntityAction jump;

        public float Horizontal {
            get => horizontal;
            set => horizontal = value;
        }

        public EntityAction Jump => jump;
        protected override void Transfer(OSDPlatformerSource src) {
            horizontal = src.GetHorizontal();
            jump.Current = src.GetJump();
        }
    }
}