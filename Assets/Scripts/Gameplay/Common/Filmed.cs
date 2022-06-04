using Cinemachine;
using Lunari.Tsuki.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
namespace OSD.Gameplay.Common {
    [TraitLocation(TraitLocations.Root)]
    public class Filmed : Trait {
        [AssetsOnly]
        public CinemachineVirtualCamera cameraPrefab;

        public CinemachineVirtualCamera sceneCamera;
        private GameObject obj;
        public CinemachineVirtualCamera Camera { get; private set; }

        public override void Configure(TraitDescriptor descriptor) {
            if (descriptor.Initialize) {
                var entity = descriptor.Entity;

                if (sceneCamera == null) {
                    Camera = Instantiate(cameraPrefab);
                    obj = Camera.gameObject;
                    //obj.hideFlags = HideFlags.NotEditable;
                    obj.name = $"EntityCamera({entity.name})";
                } else {
                    Camera = sceneCamera;
                }

                Camera.Follow = entity.transform;
            }
        }
    }
}