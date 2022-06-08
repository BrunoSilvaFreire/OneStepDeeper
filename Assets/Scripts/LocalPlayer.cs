using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Singletons;
using Lunari.Tsuki2D.Platforming.Input;
using OSD.Gameplay.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OSD {
    public class LocalPlayer : Singleton<LocalPlayer> {
        [SerializeField, HideInInspector]
        private Entity pawn;
        public PlayerInput input;
        public int localCameraPriority = 100;

        [ShowInInspector]
        public Entity Pawn {
            get => pawn;
            set {
                TryDetachFromCurrentPawn();
                pawn = value;

                if (pawn.Access(out OSDPlatformerSource src)) {
                    Debug.Log("Attaching platformer source to local player", this);
                    src.Input = input;
                } else {
                    Debug.LogWarning("Unable to attach platformer source to local player", this);
                }
                if (pawn.Access(out Filmed filmed)) {
                    filmed.Camera.Priority = localCameraPriority;
                }
            }
        }

        private void TryDetachFromCurrentPawn() {
            if (pawn != null) {
                if (pawn.Access(out OSDPlatformerSource old)) {
                    if (old.Input == input) {
                        old.Input = null;
                        Debug.Log("Detached platformer source from local player", this);
                    } else {
                        Debug.LogWarning("Found input source was not the same as local player", this);
                    }
                } else {
                    Debug.LogWarning("Unable to detach platformer source from local player", this);
                }
            }
        }
    }
}