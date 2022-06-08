using Lunari.Tsuki.Entities;
using Lunari.Tsuki.Singletons;
using OSD.Network;
using UnityEngine;
namespace OSD.Gameplay {
    public class LevelOrigin : Singleton<LevelOrigin> {
        public void Teleport(Entity entity) {
            entity.GetComponent<ClientNetworkTransform>().SetPosition(transform.position);
        }
    }
}